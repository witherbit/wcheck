using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pwither.formatter;
using wcheck.Statistic.Nodes;
using wcheck.Statistic.Styles;

namespace wcheck.Statistic.Templates
{
    [BitSerializable]
    public class NetHandleStatisticTemplate : IStatisticTemplate
    {
        public List<IStatisticNode> Nodes {  get; private set; }

        public TextNodeStyle HeaderStyle { get; private set; }

        public string? Header { get; private set; }

        public bool UseBreakAfterTemplate { get; private set; }

        public static NetHandleStatisticTemplate ConvertToNetworkingTemplate(IStatisticTemplate template)
        {
            return new NetHandleStatisticTemplate
            {
                HeaderStyle = template.HeaderStyle,
                Header = template.Header,
                Nodes = template.Nodes,
                UseBreakAfterTemplate = template.UseBreakAfterTemplate,
            };
        }
    }
}
