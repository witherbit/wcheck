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
        public ShellInfo ShellInfo { get; }

        public ShellState State { get; private set; }
        public ShellCallback Callback { get; internal set; }

        public ShellSettings Settings { get; protected set; }

        public Guid ContractId { get; internal set; }

        public event EventHandler<Guid> ShellRun;
        public event EventHandler<Guid> ShellStop;
        public event EventHandler<Guid> ShellPause;
        public event EventHandler<Guid> ShellResume;

        public ShellBase(ShellInfo info)
        {
            ShellInfo = info;
            State = ShellState.Stopped;
        }

        public ExitCode Stop(ExitCode exitCode = ExitCode.Default)
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

        public void Run()
        {
            if (State == ShellState.Stopped)
            {
                State = ShellState.Running;
                ShellRun?.Invoke(this, ContractId);
                OnRun();
            }
        }

        public void Pause()
        {
            if (State == ShellState.Running)
            {
                State = ShellState.Paused;
                ShellPause?.Invoke(this, ContractId);
                OnPause();
            }
        }

        public void Resume()
        {
            if (State == ShellState.Paused)
            {
                State = ShellState.Running;
                ShellResume?.Invoke(this, ContractId);
                OnResume();
            }
        }

        public abstract void OnRun();
        public abstract void OnPause();
        public abstract void OnStop();
        public abstract void OnResume();

        public abstract Schema OnHostCallback(Schema schema);
        public abstract void OnSettingsEdit(SettingsObject obj, PropertyEventArgs propertyEventArgs);
    }
}
