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

namespace wcheck.Statistic
{
    public interface IStatisticNode
    {
        string Name { get; }
        string GetCSV();
        void InsertDocx(Docx doc);
        object Render();
    }
}
