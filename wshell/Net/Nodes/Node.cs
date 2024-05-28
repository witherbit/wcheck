using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using pwither.formatter;

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
        public Node() { }

        public virtual byte[] Pack()
        {
            return BitSerializer.SerializeNative(this,
                typeof(Node),
                typeof(Dictionary<string, string>));
            //return JsonConvert.SerializeObject(this).ConvertFromUTF8();
        }

        public static Node Unpack(byte[] arr)
        {
            return BitSerializer.DeserializeNative<Node>(arr,
                typeof(Node),
                typeof(Dictionary<string, string>));
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
        public void SetAttribute(string attribute, string value)
        {
            if (!Attributes.ContainsKey(attribute))
            {
                Attributes.Add(attribute, value);
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
