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
        public CallbackType Type { get; set; }
        private object _providing { get; set; }
        private Dictionary<string, string> _attributes { get; set; }

        public Schema(CallbackType type) 
        {
            Type = type;
            _attributes = new Dictionary<string, string>();
        }

        public Schema SetProviding<T>(T providing)
        {
            this._providing = providing;
            return this;
        }
        public T GetProviding<T>()
        {
            return (T)_providing;
        }

        public Schema SetAttribute(string attribute, string value)
        {
            if (!_attributes.ContainsKey(attribute))
            {
                _attributes.Add(attribute, value);
            }
            return this;
        }

        public string? GetAttribute(string attribute)
        {
            if (_attributes.ContainsKey(attribute))
            {
                return _attributes[attribute];
            }
            return null;
        }
    }
}
