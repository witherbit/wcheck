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
    public class ShellSettings
    {
        public string AppName { get; }
        public List<SettingsObject> Settings {  get; }
        public ShellSettings(List<SettingsObject> defaultSettings, string appName)
        {
            Settings = defaultSettings;
            AppName = appName;
        }

        public T GetValue<T>(string name)
        {
            var obj = Settings.FirstOrDefault(x => x.Name == name);
            return (T)obj.Value;
        }
        public SettingsObject Get(string name)
        {
            var obj = Settings.FirstOrDefault(x => x.Name == name);
            return obj;
        }

        public void Save()
        {
            System.IO.Directory.CreateDirectory(@"C:\ProgramData\Witherbit\Settings");
            System.IO.File.WriteAllBytes($@"C:\ProgramData\Witherbit\Settings\{AppName}.binf", BitSerializer.SerializeNative(this, typeof(ShellSettings), typeof(SettingsObject), typeof(List<SettingsObject>)));
        }

        public static ShellSettings Load(string appName, List<SettingsObject> defaultSettings)
        {
            var ms = new ShellSettings(defaultSettings, appName);
            if (System.IO.File.Exists($@"C:\ProgramData\Witherbit\Settings\{appName}.binf"))
            ms = BitSerializer.DeserializeNative<ShellSettings>(System.IO.File.ReadAllBytes($@"C:\ProgramData\Witherbit\Settings\{appName}.binf"), typeof(ShellSettings), typeof(SettingsObject), typeof(List<SettingsObject>));
            ms.Save();
            return ms;
        }
    }
}
