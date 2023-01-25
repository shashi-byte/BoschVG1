using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoschVG1.Models.InExcelSheet
{
    [AttributeUsage(AttributeTargets.All)]
    public class Column : Attribute
    {
        public string ColumnName { get; set; }
        public int ColumnIndex { get; set; }
        public Columntype ColumnType { get; set; }

        public enum Columntype : short
        {
            Mandatory,
            Optional
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        public Column(int column)
        {
            ColumnIndex = column;
            ColumnType = Columntype.Mandatory;
        }

        public Column(string name)
        {
            ColumnName = name;
            ColumnType = Columntype.Mandatory;
        }

        public Column(int column, Columntype type)
        {
            ColumnIndex = column;
            ColumnType = type;
        }

        public Column(string name, Columntype type)
        {
            ColumnName = name;
            ColumnType = type;
        }
    }
}
