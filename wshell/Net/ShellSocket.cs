﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml.Linq;
using wshell.Abstract;
using wshell.Net.Nodes;

namespace wshell.Net
{
    internal class ShellSocket : IDisposable
    {
        private bool disposedValue;

        public delegate Node NodeRequestHandler(ShellSocket socket, Node schema); //делегат для обработки запросов
        public event NodeRequestHandler NodeRequest; //событие обработки запроса
        private TcpListener _listenerSocket { get; set; } //объект слушателя Tcp
        private CancellationTokenSource _cancellationTokenSource { get; set; }
        public CancellationToken CancellationToken { get; private set; } //токен отмены

        public SocketInitializeParameters InitializeParameters { get; private set; } //параметры инициализации

        public ShellSocket(SocketInitializeParameters initializeParameters)
        {
            InitializeParameters = initializeParameters;
        }

        private void Listen() //обработка прослушиваний входящих соединений
        {
            if (_cancellationTokenSource != null)
            {
                try
                {
                    _listenerSocket = new TcpListener(IPAddress.Any, InitializeParameters.Port);
                    _listenerSocket.Start();

                    while (!CancellationToken.IsCancellationRequested)
                    {
                        var client = _listenerSocket.AcceptTcpClient();
                        Task.Run(async () => await HandleResponse(client)); //обрабатывает запрос в отдельном потоке
                    }
                    Close();
                }
                catch (Exception ex)
                {
                    Close();
                }
            }
        }
        private byte[] ReadStream(TcpClient client, CancellationToken token)
        {
            var stream = client.GetStream();
            var response = new List<byte>();
            do
            {
                response.Add((byte)stream.ReadByte());
            }
            while (stream != null && stream.DataAvailable && !token.IsCancellationRequested && client != null && client.Connected);
            return response.ToArray();
        }

        public Node Receive(TcpClient client, CancellationToken token) //получает Node
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var update = ReadStream(client, token);
                    return Node.Unpack(update, InitializeParameters);
                }
                catch (Exception ex)
                {
                    return new Node("exception", ex, new Dictionary<string, string>
                    {
                        { "code", "500" }
                    });
                }
            }
            return new Node("empty", new Dictionary<string, string>
                {
                    { "code", "501" }
                });
        }

        private async Task HandleResponse(TcpClient client) //обработка ответа на запрос
        {
            try
            {
                var stream = client.GetStream();
                var node = Receive(client, CancellationToken);
                if (!CancellationToken.IsCancellationRequested)
                    if (node != null && node.Tag != "exception" && node.Tag != "empty")
                    {
                        node.SetAttribute("from", ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                        var data = NodeRequest?.Invoke(this, node).Pack(InitializeParameters);
                        await stream.WriteAsync(data, 0, data.Length);
                    }
                    else
                    {
                        var data = new Node("bad request", new Dictionary<string, string>
                        {
                            { "code", "400" }
                        }).Pack(InitializeParameters);
                        await stream.WriteAsync(data, 0, data.Length);
                    }
            }
            catch (Exception ex)
            {
            }
            client.Close();
        }

        public void Open() //запуск прослушивания Tcp соединений
        {
            if (_cancellationTokenSource == null)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                CancellationToken = _cancellationTokenSource.Token;
                Thread receiveThread = new Thread(new ThreadStart(Listen));
                receiveThread.Start();
            }
        }
        public void Close() //остановка прослушивания Tcp соединений
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
                _listenerSocket.Stop();
            }
        }

        public async Task<Node> RequestAsync(IPAddress address, Node node, CancellationToken cancellationToken) //запрос
        {
            var client = new TcpClient();
            try
            {
                await client.ConnectAsync(address, InitializeParameters.Port);
                var stream = client.GetStream();

                if (!cancellationToken.IsCancellationRequested)
                {
                    var data = node.Pack(InitializeParameters);
                    await stream.WriteAsync(data, 0, data.Length);
                }
                else
                {
                    return new Node("cancel", new Dictionary<string, string>
                {
                    { "code", "400" }
                });
                }

                var response = Receive(client, cancellationToken);
                client.Close();
                return response;
            }
            catch (Exception ex)
            {
                return new Node("exception", ex, new Dictionary<string, string>
                {
                    { "code", "500" }
                });
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Close();
                    NodeRequest = null;
                }
                disposedValue = true;
            }
        }

        ~ShellSocket()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public ShellClientProviding CreateClientProviding()
        {
            return new ShellClientProviding()
            {
                _socket = this
            };
        }
    }
}
