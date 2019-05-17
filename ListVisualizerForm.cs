using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GenericCollectionDebuggerVisualizer
{
    public partial class ListVisualizerForm : Form
    {
        public ListVisualizerForm()
        {
            InitializeComponent();

            dgvList.DoubleBuffered(true);
        }
    }
}
