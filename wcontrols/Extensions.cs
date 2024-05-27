using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace wcheck.wcontrols
{
    public static class Extensions
    {
        public static SolidColorBrush GetBrush(this string hex)
        {
            if (hex[0] != '#') hex = hex.Insert(0, "#");
            return (SolidColorBrush)new BrushConverter().ConvertFrom(hex);
        }
        public static Color GetColor(this string hex)
        {
            if (hex[0] != '#') hex = hex.Insert(0, "#");
            return (Color)ColorConverter.ConvertFromString(hex);
        }
        public static void Invoke(this ContentControl instanse, Action action)
        {
            instanse.Dispatcher?.BeginInvoke(DispatcherPriority.Background, action);
        }
        public static void Invoke(this Page instanse, Action action)
        {
            instanse.Dispatcher?.BeginInvoke(DispatcherPriority.Background, action);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static string GenerateRandomColor(this int min)
        {
            var random = new Random(Guid.NewGuid().GetHashCode() + Environment.TickCount);
            return string.Format("#{0:X6}", random.Next(min));
        }
    }
}
