using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BoschVG1.Models.InExcelSheet
{
    public class HeaderColumn
    {
        public PropertyInfo Property { get; set; }
        public string ColumnName { get; set; }
        public int ColumnIndex { get; set; }
        public Column.Columntype Columntype { get; set; }
    }
}
