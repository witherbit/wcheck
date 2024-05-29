using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wcheck.wshell.Enums
{
    public enum CallbackType
    {
        /// <summary>
        /// Пустой ответ
        /// [returned from InvokeRequest]
        /// </summary>    
        EmptyResponse,
        /// <summary>
        /// Ошибка
        /// [returned from InvokeRequest] (Exception)
        /// </summary>    
        Exception,
        /// <summary>
        /// Регистрация страницы на фрейме вкладки
        /// [Invoke] (Page)
        /// </summary>
        RegisterPage,
        /// <summary>
        /// Удаление вкладки
        /// [Invoke] (Page)
        /// </summary>    
        UnregisterPage,
        /// <summary>
        /// Запрос перезагрузки страницы
        /// [InvokeRequest] (Page)
        /// </summary>    
        ReloadPageRequest,
        /// <summary>
        /// Ответ на запрос перезагрузки страницы
        /// [returned from InvokeRequest] (Page)
        /// </summary>    
        ReloadPageResponse,
        /// <summary>
        /// Запрос на получение ShellInfos
        /// [InvokeRequest] (ShellInfo)
        /// </summary>    
        ShellInfosRequest,
        /// <summary>
        /// Ответ на запрос на получение ShellInfos
        /// [returned from InvokeRequest] (ShellInfos[])
        /// </summary>    
        ShellInfosResponse,
        /// <summary>
        /// Запрос на получение объекта модуля
        /// [InvokeRequest] (ShellInfo)
        /// </summary>    
        GetShellInstanceRequest,
        /// <summary>
        /// Ответ на запрос на получение объекта модуля
        /// [returned from InvokeRequest] (ShellBase)
        /// </summary>    
        GetShellInstanceResponse,
        /// <summary>
        /// Запрос на получение значения параметра
        /// [InvokeRequest] (string ParameterName)
        /// </summary>    
        SettingsParameterRequest,
        /// <summary>
        /// Ответ на запрос на получение значения параметра
        /// [returned from InvokeRequest] (SettingObject)
        /// </summary>    
        SettingsParameterResponse,
    }
}
