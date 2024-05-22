using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wshell.Enums;

namespace wshell.Objects
{
    public struct ShellInfo
    {

        public byte[] Image {  get; set; }
        public string Name { get; }
        public string Description { get; }
        public string Author { get; }

        public ShellType Type { get; }

        public Guid Id { get; }

        public ShellVersion Version { get; }

        public ShellInfo(string name, ShellVersion version, Guid id, ShellType shellType, string description = null, string author = null)
        {
            Name = name;
            Description = description;
            Author = author;
            Version = version;
            Id = id;
            Type = shellType;
        }

        public ShellInfo(string name, string version, Guid id, ShellType shellType, string description = null, string author = null)
        {
            Name = name;
            Description = description;
            Author = author;
            Version = new ShellVersion(version);
            Id = id;
            Type = shellType;
        }
    }
}
