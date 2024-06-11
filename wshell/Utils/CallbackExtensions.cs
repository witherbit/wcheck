using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using wcheck;
using wcheck.wshell.Enums;
using wcheck.wshell.Objects;
using wshell.Abstract;
using wshell.Net;

namespace wshell.Utils
{
    public static class CallbackExtensions
    {
        #region ShellRequests

        /// <summary>
        ///  Запрос параметра приложения или модуля
        /// </summary>
        /// <param name="shell">Модуль</param>
        /// <param name="propertyName">Имя параметра</param>
        /// <param name="shellId">Id модуля</param>
        /// <returns></returns>
        public static SettingsObject RequestSettingsProperty(this ShellBase shell, string propertyName, string shellId = null)
        {
            var schema = new Schema(CallbackType.SettingsParameterRequest).SetProviding(propertyName);
            if (!string.IsNullOrEmpty(shellId))
                schema.SetAttribute("target", shellId);
            var response = shell.Callback.InvokeRequest(shell, schema);
            return response.GetProviding<SettingsObject>();
        }

        /// <summary>
        ///  Запрос параметра приложения или модуля
        /// </summary>
        /// <param name="shell">Модуль</param>
        /// <param name="propertyName">Имя параметра</param>
        /// <param name="shellId">Id модуля</param>
        /// <returns></returns>
        public static ShellClientProviding? RequestClientProviding(this ShellBase shell)
        {
            var schema = new Schema(CallbackType.ClientProvidingRequest);
            var response = shell.Callback.InvokeRequest(shell, schema);
            if (response.Type == CallbackType.ClientProvidingResponse)
                return response.GetProviding<ShellClientProviding>();
            else return null;
        }

        /// <summary>
        /// Пользовательский Callback запрос
        /// </summary>
        /// <param name="shell">Модуль</param>
        /// <param name="targetShellId">Id модуля назначения</param>
        /// <param name="providing">Объект предоставления</param>
        public static Schema InvokeCustomRequest<T>(this ShellBase shell, string targetShellId, T providing)
        {
            return shell.Callback.InvokeRequest(shell, new Schema(CallbackType.CustomRequest).SetAttribute("target", targetShellId).SetProviding(providing));
        }

        #endregion

        #region ShellInvokes

        /// <summary>
        /// Отправление оповещения об изменении параметров определенному модулю
        /// </summary>
        /// <param name="shell">Модуль</param>
        /// <param name="targetShellId">Id модуля назначения</param>
        public static void InvokeSettingsPropertyChanged(this ShellBase shell, string targetShellId)
        {
            shell.Callback.Invoke(shell, new Schema(CallbackType.ShellPropertyChanged).SetProviding(targetShellId));
        }
        /// <summary>
        /// Открывает (ставит фокус) на вкладку, которой пренадлежит страница
        /// </summary>
        /// <param name="shell">Модуль</param>
        /// <param name="targetPage">Страница, привязанная к вкладке</param>
        public static void InvokeTabFocus(this ShellBase shell, Page targetPage)
        {
            shell.Callback.Invoke(shell, new Schema(CallbackType.SetTabFocus).SetProviding(targetPage));
        }
        /// <summary>
        /// Запуск задачи на стороне компонента задачи
        /// </summary>
        /// <param name="shell">Модуль</param>
        /// <param name="targetShellId">Id модуля назначения</param>
        public static void InvokeStartTaskView(this ShellBase shell, string targetShellId)
        {
            shell.Callback.Invoke(shell, new Schema(CallbackType.StartTaskView).SetProviding(targetShellId));
        }

        /// <summary>
        /// Пользовательский Callback вызов модулю
        /// </summary>
        /// <param name="shell">Модуль</param>
        /// <param name="targetShellId">Id модуля назначения</param>
        /// <param name="providing">Объект предоставления</param>
        public static void InvokeCustom<T>(this ShellBase shell, string targetShellId, T providing)
        {
            shell.Callback.Invoke(shell, new Schema(CallbackType.CustomInvoke).SetAttribute("target", targetShellId).SetProviding(providing));
        }

        #endregion

        #region HostInvokes

        /// <summary>
        /// Отправление оповещения об изменении параметров определенному модулю
        /// </summary>
        /// <param name="host">Хост</param>
        /// <param name="targetShellId">Id модуля назначения</param>
        internal static void InvokeSettingsPropertyChanged(this IHost host, string targetShellId)
        {
            var store = host.Controller.ContractStore;
            var shell = host.Controller.GetShellById(targetShellId);
            var storeInfo = store.Get(shell);
            if(storeInfo.Contract != null)
            {
                storeInfo.Contract.Invoke(new Schema(CallbackType.ShellPropertyChanged).SetProviding(targetShellId));
            }
        }
        /// <summary>
        /// Отправление оповещения об изменении параметров определенному модулю
        /// </summary>
        /// <param name="host">Хост</param>
        /// <param name="targetShellId">Id модуля назначения</param>
        internal static void InvokeStartTaskView(this IHost host, string targetShellId)
        {
            var store = host.Controller.ContractStore;
            var shell = host.Controller.GetShellById(targetShellId);
            var storeInfo = store.Get(shell);
            if (storeInfo.Contract != null)
            {
                var sendSchema = new Schema(CallbackType.StartTaskView).SetProviding(targetShellId);
                storeInfo.Contract.Invoke(sendSchema);
            }
        }
        /// <summary>
        /// Пользовательский вызов модулю
        /// </summary>
        /// <param name="host">Хост</param>
        /// <param name="targetShellId">Id модуля назначения</param>
        internal static void InvokeCustom(this IHost host, string targetShellId, Schema schema)
        {
            var store = host.Controller.ContractStore;
            var shell = host.Controller.GetShellById(targetShellId);
            var storeInfo = store.Get(shell);
            if (storeInfo.Contract != null)
            {
                storeInfo.Contract.Invoke(schema);
            }
        }

        internal static Schema InvokeCustomResponse(this IHost host, string targetShellId, Schema schema)
        {
            var store = host.Controller.ContractStore;
            var shell = host.Controller.GetShellById(targetShellId);
            var storeInfo = store.Get(shell);
            if (storeInfo.Contract != null)
            {
                return storeInfo.Contract.Invoke(schema);
            }
            return new Schema(CallbackType.EmptyResponse);
        }

        #endregion
    }
}
