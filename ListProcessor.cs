using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace GenericCollectionDebuggerVisualizer
{
    public class ListProcessor
    {
        private IList list;

        public ListProcessor(IList l)
        {
            list = l;
        }

        public void GetList(DataGridView dgvList)
        {
            // Check if it is not empty
            if (list.Count > 0)
            {
                object firstItem = list[0];
                IDictionary<string, string> columns = new Dictionary<string, string>();

                Type typeObj = firstItem.GetType();

                if (typeObj.IsPrimitive == false && typeObj.Name != "String")
                {
                    // Retrieve the properties
                    PropertyInfo[] properties = firstItem.GetType().GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        dgvList.Columns.Add(property.Name, property.Name);
                        columns.Add(property.Name, "P");
                    }

                    // Retrieve the fields
                    FieldInfo[] fields = firstItem.GetType().GetFields();

                    foreach (FieldInfo field in fields)
                    {
                        dgvList.Columns.Add(field.Name, field.Name);
                        columns.Add(field.Name, "F");
                    }
                }

                // Value type
                if (columns.Count == 0)
                {
                    dgvList.Columns.Add(typeObj.Name, typeObj.Name);
                    columns.Add(typeObj.Name, "V");
                }

                // Retrieve the fields and properties values
                foreach (object item in list)
                {
                    object[] row = new object[columns.Count];

                    int countRow = 0;
                    foreach (KeyValuePair<string, string> column in columns)
                    {
                        string name = column.Key;
                        string type = column.Value;

                        if (type == "P")
                        {
                            // Retrieve property value
                            row[countRow] = item.GetType().GetProperty(name).GetValue(item, null);
                        }

                        if (type == "F")
                        {
                            // Retrieve field value
                            row[countRow] = item.GetType().GetField(name).GetValue(item);
                        }

                        if (type == "V")
                        {
                            // Retrieve value type
                            row[countRow] = item.ToString();
                        }

                        countRow++;
                    }
                    dgvList.Rows.Add(row);
                }
            }
        }
    }
}
