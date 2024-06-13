using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wcheck.Statistic.Nodes;
using wcheck.Statistic.Styles;

namespace wcheck.Statistic.Templates
{
    public interface IStatisticTemplate
    {
        List<IStatisticNode> Nodes { get; }
        TextNodeStyle HeaderStyle { get; }
        string? Header { get; }
        bool UseBreakAfterTemplate { get; }
    }
}
