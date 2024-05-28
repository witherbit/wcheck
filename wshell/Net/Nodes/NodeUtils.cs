using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wcheck.wshell.Enums;

namespace wshell.Net.Nodes
{
    public static class NodeUtils
    {
        public static byte[] RandomBytes(int size)
        {
            Random rnd = new Random((short)DateTime.Now.Ticks);
            byte[] buffer = new byte[size];
            rnd.NextBytes(buffer);
            return buffer;
        }

        public static Node GetChild(this Node node, string tag, ContentStorageStyle contentStorageStyle = ContentStorageStyle.NodeTree)
        {
            if (contentStorageStyle == ContentStorageStyle.NodeTree)
            {
                var temp = node.Child as Node;
                while (true)
                {
                    if (temp == null || temp.Child == null)
                        break;
                    if (temp.Tag == tag)
                        return temp;
                    else
                        temp = temp.Child as Node;
                }
            }
            else if (contentStorageStyle == ContentStorageStyle.NodeArray)
            {
                if (node.Child is Node[] nodes)
                {
                    return nodes.FirstOrDefault(x => x.Tag == tag);
                }
                return null;
            }
            else if (contentStorageStyle == ContentStorageStyle.NodeList)
            {
                if (node.Child is List<Node> nodes)
                {
                    return nodes.FirstOrDefault(x => x.Tag == tag);
                }
                return null;
            }
            else if (contentStorageStyle == ContentStorageStyle.ObjectArray)
            {
                if (node.Child is object[] nodes)
                {
                    var cntnt = node.Child as object[];
                    foreach (object obj in cntnt)
                    {
                        if (obj is Node)
                            if (((Node)obj).Tag == tag)
                                return obj as Node;
                    }
                }
                return null;
            }
            else if (contentStorageStyle == ContentStorageStyle.ObjectList)
            {
                if (node.Child is List<object> nodes)
                {
                    var cntnt = node.Child as List<object>;
                    foreach (object obj in cntnt)
                    {
                        if (obj is Node)
                            if (((Node)obj).Tag == tag)
                                return obj as Node;
                    }
                }
                return null;
            }
            return null;
        }
    }
}
