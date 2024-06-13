using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DocumentFormat.OpenXml.Packaging;
using DocxEngine;
using pwither.formatter;

namespace wcheck.Statistic.Nodes
{
    public interface IStatisticNode
    {
        string GetCSV();
        void InsertDocx(Docx doc);
        void Render(StackPanel panel);
    }
}
