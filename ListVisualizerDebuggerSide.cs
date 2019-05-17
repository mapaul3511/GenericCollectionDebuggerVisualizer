using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

[assembly: System.Diagnostics.DebuggerVisualizer(typeof(GenericCollectionDebuggerVisualizer.ListVisualizerDebuggerSide), 
                                                 typeof(VisualizerObjectSource), 
                                                 Target = typeof(List<>), 
                                                 Description = "List Visualizer")]
namespace GenericCollectionDebuggerVisualizer
{
    public class ListVisualizerDebuggerSide : DialogDebuggerVisualizer
    {
        //private DataGridView dgvList;
        //private Form form;
        private ListProcessor listProcessor;

        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            ListVisualizerForm form = new ListVisualizerForm();

            try
            {
                // Get the list in the grid view
                IList list = (IList)objectProvider.GetObject();
                listProcessor = new ListProcessor(list);
                listProcessor.GetList(form.dgvList);
            }
            catch { }

            // Show the grid with the list
            windowService.ShowDialog(form);
        }
    }
}
