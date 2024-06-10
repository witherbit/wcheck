using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using wcheck.wshell.Enums;

namespace wcheck.wshell.Objects
{
    public sealed class Schema
    {
        public CallbackType Type { get; set; } //тип схемы
        private object _providing { get; set; } //объект предоставления
        private Dictionary<string, string> _attributes { get; set; } //атрибуты схемы

        public Schema(CallbackType type) 
        {
            Type = type;
            _attributes = new Dictionary<string, string>();
        }

        public Schema SetProviding<T>(T providing) //устанавливает объект предоставления
        {
            this._providing = providing;
            return this;
        }
        public T GetProviding<T>() //получает объект предоставления
        {
            return (T)_providing;
        }

        public Type GetProvidingType() //получает тип объекта предоставления
        {
            return _providing.GetType();
        }

        public Schema SetAttribute(string attribute, string value) //устанавливает значение атрибута
        {
            if (!_attributes.ContainsKey(attribute))
            {
                _attributes.Add(attribute, value);
            }
            return this;
        }

        public string? GetAttribute(string attribute) //получает значение атрибута
        {
            if (_attributes.ContainsKey(attribute))
            {
                return _attributes[attribute];
            }
            return null;
        }
    }
}
