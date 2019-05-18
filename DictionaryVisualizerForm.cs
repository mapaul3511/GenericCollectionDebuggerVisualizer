using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace GenericCollectionDebuggerVisualizer
{
    public partial class DictionaryVisualizerForm : Form
    {
        private IDictionary dict;

        public IDictionary ProcessingDict
        {
            get 
            { 
                return dict; 
            }
            set
            {
                this.dict = value;
            }
        }

        private Dictionary<TreeNode, IList> dicNodeDataListMap;
        private Dictionary<TreeNode, object> dicNodeDataObjectMap;

        public DictionaryVisualizerForm()
        {
            InitializeComponent();

            grid.DoubleBuffered(true);
        }

        private void DictionaryVisualizerForm_Load(object sender, EventArgs e)
        {
            dicNodeDataListMap = new Dictionary<TreeNode, IList>();
            dicNodeDataObjectMap = new Dictionary<TreeNode, object>();

            AddToTreeView(this.dict);

            treeView.ExpandAll();
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;

            if (node.Text.Contains("List") && dicNodeDataListMap.ContainsKey(node))
            {
                IList list = dicNodeDataListMap[node];

                DumpListContentOnGrid(list);
            }
            else if (dicNodeDataObjectMap.ContainsKey(node))
            {
                object dataObject = dicNodeDataObjectMap[node];

                if (IsDirectPrintableType(dataObject) || dataObject is string)
                {
                    ShowPrimitiveValueOnGrid(dataObject);
                }
                else if (dataObject is object)
                {
                    DumpDataObjectContentOnGrid(dataObject);
                }
            }
        }

        private void AddToTreeView(IDictionary dic)
        {
            AddToTreeView(dic, parentNode: null);
        }

        private void AddToTreeView(IDictionary dic, TreeNode parentNode)
        {
            var nodes = (parentNode == null) ? treeView.Nodes : parentNode.Nodes;

            foreach (var key in dic.Keys)
            {
                TreeNode node = nodes.Add(GenerateNodeText(key));
                dicNodeDataObjectMap.Add(node, key);

                object value = dic[key];

                if (value is IList)
                {
                    AddToTreeView(value as IList, node.Nodes.Add(GenerateNodeText(value)));
                }
                else if (dic[key] is IDictionary)
                {
                    AddToTreeView(value as IDictionary, node.Nodes.Add(GenerateNodeText(value)));
                }
                else
                {
                    TreeNode newNode = node.Nodes.Add(GenerateNodeText(value));

                    if (value != null)
                    {
                        dicNodeDataObjectMap.Add(newNode, value);
                    }
                }
            }
        }

        private void AddToTreeView(IList list, TreeNode parentNode)
        {
            dicNodeDataListMap.Add(parentNode, list);

            foreach (var item in list)
            {
                TreeNode newNode = parentNode.Nodes.Add(GenerateNodeText(item));

                if (item != null)
                {
                    dicNodeDataObjectMap.Add(newNode, item);
                }
            }
        }

        private void DumpDataObjectContentOnGrid(object dataObject)
        {
            ClearGrid();

            grid.Columns.Add("Property_Field_Name", "Property/Field");
            grid.Columns.Add("Property_Field_Value", "Value");

            PropertyInfo[] properties = dataObject.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                int rowIndex = grid.Rows.Add();
                grid.Rows[rowIndex].Cells[0].Value = prop.Name;

                object value = dataObject.GetType().GetProperty(prop.Name).GetValue(dataObject, null);
                OutputValueToGridCell(value, rowIndex);
            }

            FieldInfo[] fields = dataObject.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                int rowIndex = grid.Rows.Add();
                grid.Rows[rowIndex].Cells[0].Value = field.Name;

                object value = dataObject.GetType().GetField(field.Name).GetValue(dataObject);
                OutputValueToGridCell(value, rowIndex);
            }

            grid.AutoResizeColumns();
        }

        private void OutputValueToGridCell(object value, int rowIndex)
        {
            if (value == null)
            {
                grid.Rows[rowIndex].Cells[1].Value = "{NULL}";
            }
            else if (IsDirectPrintableType(value) || value is string)
            {
                grid.Rows[rowIndex].Cells[1].Value = value;
            }
            else
            {
                grid.Rows[rowIndex].Cells[1].Value = "{" + value.GetType().Name + "}";
            }
        }

        private void DumpListContentOnGrid(IList list)
        {
            ClearGrid();

            if (list.Count == 0)
                return;

            object firstItem = list[0];
            IDictionary<string, string> columns = new Dictionary<string, string>();

            Type typeObj = firstItem.GetType();

            if (typeObj.IsPrimitive == false && typeObj.Name != "String")
            {
                // Retrieve the properties
                PropertyInfo[] properties = firstItem.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    grid.Columns.Add(property.Name, property.Name);
                    columns.Add(property.Name, "P");
                }

                // Retrieve the fields
                FieldInfo[] fields = firstItem.GetType().GetFields();

                foreach (FieldInfo field in fields)
                {
                    grid.Columns.Add(field.Name, field.Name);
                    columns.Add(field.Name, "F");
                }
            }

            // Value type
            if (columns.Count == 0)
            {
                grid.Columns.Add(typeObj.Name, typeObj.Name);
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

                grid.Rows.Add(row);
            }
        }

        private string GenerateNodeText(object value)
        {
            string nodeText = "[" + value.GetType().Name + "]";

            if (IsDirectPrintableType(value))
            {
                nodeText += " " + value.ToString();
            }
            else if (value is string)
            {
                nodeText += " \"" + value.ToString() + "\"";
            }

            return nodeText;
        }

        private bool IsDirectPrintableType(object value)
        {
            return value.GetType().IsPrimitive || value.GetType().IsEnum || value is decimal || value is double || value is float || value is DateTime;
        }

        private void ShowPrimitiveValueOnGrid(object dataObject)
        {
            ClearGrid();

            grid.Columns.Add("Value", "Value");
            grid.Rows.Add();
            grid[0, 0].Value = dataObject.ToString();
        }

        private void ClearGrid()
        {
            grid.Rows.Clear();
            grid.Columns.Clear();
        }
    }
}
