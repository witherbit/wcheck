using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pwither.formatter;

namespace wcheck.wshell.Objects
{
    [BitSerializable]
    public class MainSettings
    {
        public List<SettingsObject> Settings {  get; }
        public Dictionary<Guid, List<SettingsObject>> ShellSettings { get; }
        public MainSettings(List<SettingsObject> defaultSettings)
        {
            Settings = defaultSettings;
            ShellSettings = new Dictionary<Guid, List<SettingsObject>>();
        }

        public T Get<T>(string name)
        {
            var obj = Settings.FirstOrDefault(x => x.Name == name);
            return (T)obj.Value;
        }

        public void Save()
        {
            System.IO.Directory.CreateDirectory(@"C:\ProgramData\Witherbit\Settings");
            System.IO.File.WriteAllBytes(@"C:\ProgramData\Witherbit\Settings\wcheck.binf", BitSerializer.Serialize(this));
        }

        public static MainSettings Load(List<SettingsObject> defaultSettings)
        {
            if (System.IO.File.Exists(@"C:\ProgramData\Witherbit\Settings\wcheck.binf"))
                return BitSerializer.Deserialize<MainSettings>(System.IO.File.ReadAllBytes(@"C:\ProgramData\Witherbit\Settings\wcheck.binf"));
            else
                return new MainSettings(defaultSettings);
        }
    }
}
