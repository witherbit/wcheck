using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using pwither.formatter;
using pwither.IO;
using wcheck.Statistic.Enums;
using wcheck.Statistic.Items;
using wcheck.Statistic.Nodes;
using wcheck.Statistic.Styles;
using wcheck.Statistic.Templates;
using wshell.Utils;

namespace wshell.Net.Nodes
{

    [BitSerializable]
    public class Node
    {
        public string Tag { get; set; }
        public object Child { get; set; }
        public Dictionary<string, string> Attributes { get; set; }

        public Node(string tag, object child, Dictionary<string, string> attributes)
        {
            Tag = tag;
            Child = child;
            Attributes = attributes;
        }
        public Node(string tag, object child)
        {
            Tag = tag;
            Child = child;
            Attributes = new Dictionary<string, string>();
        }
        public Node(string tag, Dictionary<string, string> attributes)
        {
            Tag = tag;
            Attributes = attributes;
        }
        public Node(string tag)
        {
            Tag = tag;
            Attributes = new Dictionary<string, string>();
        }
        public Node() 
        {
            Tag = null;
            Attributes = new Dictionary<string, string>();
        }

        public virtual byte[] Pack(SocketInitializeParameters parameters)
        {
            var packedData = BitSerializer.SerializeNative(this,
                typeof(Node),
                            typeof(Dictionary<string, string>),
                            typeof(TextStatisticNode),
                            typeof(TextAligment),
                            typeof(CeilItem),
                            typeof(ImageItem),
                            typeof(PieItem),
                            typeof(BreakStatisticNode),
                            typeof(ImageStatisticNode),
                            typeof(PieStatisticNode),
                            typeof(TableStatisticNode),
                            typeof(TextStatisticNode),
                            typeof(ImageNodeStyle),
                            typeof(TableNodeStyle),
                            typeof(TextNodeStyle),
                            typeof(IStatisticNode),
                            typeof(IStatisticTemplate),
                            typeof(List<IStatisticNode>),
                            typeof(FilePart),
                            typeof(NetHandleStatisticTemplate),
                            typeof(WpfThinkness));
            if (parameters.UseEncryption)
                switch (parameters.AuthType)
                {
                    case Enums.SocketAuthType.Aes:
                        return packedData.EncryptAES(parameters.Key);
                }
            return packedData;
            //return JsonConvert.SerializeObject(this).ConvertFromUTF8();
        }

        public static Node Unpack(byte[] arr, SocketInitializeParameters parameters)
        {
            if (parameters.UseEncryption)
                try
                {
                    switch (parameters.AuthType)
                    {
                        case Enums.SocketAuthType.Aes:
                            var decryptData = arr.DecryptAES(parameters.Key);
                            return BitSerializer.DeserializeNative<Node>(decryptData,
                            typeof(Node),
                            typeof(Dictionary<string, string>),
                            typeof(TextStatisticNode),
                            typeof(TextAligment),
                            typeof(CeilItem),
                            typeof(ImageItem),
                            typeof(PieItem),
                            typeof(BreakStatisticNode),
                            typeof(ImageStatisticNode),
                            typeof(PieStatisticNode),
                            typeof(TableStatisticNode),
                            typeof(TextStatisticNode),
                            typeof(ImageNodeStyle),
                            typeof(TableNodeStyle),
                            typeof(TextNodeStyle),
                            typeof(IStatisticNode),
                            typeof(IStatisticTemplate),
                            typeof(List<IStatisticNode>),
                            typeof(FilePart),
                            typeof(NetHandleStatisticTemplate),
                            typeof(WpfThinkness));
                    }
                }
                catch (Exception ex)
                {
                    return new Node("bad encryption", ex, new Dictionary<string, string>
                    {
                        { "code", "401" }
                    });
                }
            return BitSerializer.DeserializeNative<Node>(arr,
                typeof(Node),
                            typeof(Dictionary<string, string>),
                            typeof(TextStatisticNode),
                            typeof(TextAligment),
                            typeof(CeilItem),
                            typeof(ImageItem),
                            typeof(PieItem),
                            typeof(BreakStatisticNode),
                            typeof(ImageStatisticNode),
                            typeof(PieStatisticNode),
                            typeof(TableStatisticNode),
                            typeof(TextStatisticNode),
                            typeof(ImageNodeStyle),
                            typeof(TableNodeStyle),
                            typeof(TextNodeStyle),
                            typeof(IStatisticNode),
                            typeof(IStatisticTemplate),
                            typeof(List<IStatisticNode>),
                            typeof(FilePart),
                            typeof(NetHandleStatisticTemplate),
                            typeof(WpfThinkness));
            //return JsonConvert.DeserializeObject<Node>(arr.ConvertToUTF8());
        }

        public string GetAttribute(string attribute)
        {
            if (Attributes.ContainsKey(attribute))
            {
                return Attributes[attribute];
            }
            return default;
        }
        public Node SetAttribute(string attribute, string value)
        {
            if (!Attributes.ContainsKey(attribute))
            {
                Attributes.Add(attribute, value);
            }
            return this;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
