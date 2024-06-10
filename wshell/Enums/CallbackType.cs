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
        /// [returned from InvokeRequest] (ShellInfo[])
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
        /// Attributes:
        /// target : string (shellId)
        /// </summary>    
        SettingsParameterRequest,
        /// <summary>
        /// Ответ на запрос на получение значения параметра
        /// [returned from InvokeRequest] (SettingObject)
        /// Attributes:
        /// from : string (shellId)
        /// </summary>    
        SettingsParameterResponse,
        /// <summary>
        /// Оповещение об изменении настроек модуля
        /// [Invoke] (string ShellId)
        /// </summary>
        ShellPropertyChanged,
        /// <summary>
        /// Фокус на активную вкладку
        /// [Invoke] (Page)
        /// </summary>
        SetTabFocus,
        /// <summary>
        /// Запуск вcтроенного в компонент задачи метода
        /// [Invoke] (string targetShellId)
        /// </summary>
        StartTaskView,
        /// <summary>
        /// Кастомный запрос другому плагину
        /// [Invoke] (object)
        /// Attributes:
        /// target : string (shellId)
        /// </summary>
        CustomRequest,
        /// <summary>
        /// Кастомный ответ другому плагину
        /// [Invoke] (object)
        /// Attributes:
        /// target : string (shellId)
        /// </summary>
        CustomResponse,
        /// <summary>
        /// Кастомный вызов другому плагину
        /// [Invoke] (object)
        /// Attributes:
        /// target : string (shellId)
        /// </summary>
        CustomInvoke,
        /// <summary>
        /// Тип схемы для пользовательских вызовов
        /// [Data]
        /// </summary>
        ShemaValue,
    }
}
