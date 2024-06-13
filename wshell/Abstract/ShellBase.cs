using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wcheck.wcontrols;
using wcheck.wshell.Objects;
using wshell.Core;
using wshell.Enums;
using wshell.Objects;

namespace wshell.Abstract
{
    public abstract class ShellBase
    {
        internal EventRedirect? _eventsRedirect;
        public ShellInfo ShellInfo { get; } //информация о модуле

        public ShellState State { get; private set; } //текущее состояние модуля
        public ShellCallback Callback { get; internal set; } //объект общения с программой

        public ShellSettings Settings { get; protected set; } //настройки модуля

        public Guid ContractId { get; internal set; } //идентификатор контракта

        public event EventHandler<Guid> ShellRun;
        public event EventHandler<Guid> ShellStop;
        public event EventHandler<Guid> ShellPause;
        public event EventHandler<Guid> ShellResume;

        public ShellBase(ShellInfo info)
        {
            ShellInfo = info;
            State = ShellState.Stopped;
        }

        public ExitCode Stop(ExitCode exitCode = ExitCode.Default) //останавливает работу модуля
        {
            if (State != ShellState.Stopped)
            {
                OnStop();
                State = ShellState.Stopped;
                ShellStop?.Invoke(this, ContractId);
                return exitCode;
            }
            return ExitCode.AlreadyStopped;
        }

        public void Run() //запускает работу модуля
        {
            if (State == ShellState.Stopped)
            {
                State = ShellState.Running;
                ShellRun?.Invoke(this, ContractId);
                OnRun();
            }
        }

        public void Pause() //ставит работу модуля на паузу
        {
            if (State == ShellState.Running)
            {
                State = ShellState.Paused;
                ShellPause?.Invoke(this, ContractId);
                OnPause();
            }
        }

        public void Resume() //снимает работу модуля с паузы
        {
            if (State == ShellState.Paused)
            {
                State = ShellState.Running;
                ShellResume?.Invoke(this, ContractId);
                OnResume();
            }
        }

        public abstract void OnRun(); //реализуемый метод при запуске
        public abstract void OnPause();  //реализуемый метод при паузе
        public abstract void OnStop();  //реализуемый метод при остановке
        public abstract void OnResume();  //реализуемый метод при возобновлении

        public abstract Schema OnHostCallback(Schema schema);  //реализуемый метод при обращении к модулю через callback
        public abstract void OnSettingsEdit(SettingsObject obj, PropertyEventArgs propertyEventArgs);  //реализуемый метод при изменении настроек модуля извне
    }
}
