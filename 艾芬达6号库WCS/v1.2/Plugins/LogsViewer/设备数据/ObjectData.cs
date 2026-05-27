using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Wcs.App.Plugins.LogsViewer.设备数据
{
    public class ObjectData : DeviceStatusData
    {
        public DataRow DataRow { get; set; }
        public ObjectData(DataRow row)
            : base(row)
        {
            this.DataRow = row;
        }
        public override string ToDetails()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataColumn column in DataRow.Table.Columns)
            {
                if (Convert.IsDBNull(DataRow[column.ColumnName]) || String.IsNullOrWhiteSpace(Convert.ToString(DataRow[column.ColumnName])))
                {
                    continue;
                }

                var name = getFriendlyColumnName(column.ColumnName);

                sb.AppendFormat("{0}：{1}\n", name, DataRow[column.ColumnName]);
            }

            return sb.ToString();
        }

        String getFriendlyColumnName(String columnName)
        {
            if (columnName.Length < 2)
            {
                return columnName;
            }

            var index = columnName.IndexOf('_');
            if (index < 3)
            {
                return columnName;
            }


            if (index == columnName.Length - 1)
            {
                return columnName.Substring(0, index);
            }

            return columnName.Substring(index+1);
        }
    }
}
