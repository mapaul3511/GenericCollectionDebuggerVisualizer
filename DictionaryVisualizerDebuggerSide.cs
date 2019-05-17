using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

[assembly: System.Diagnostics.DebuggerVisualizer(typeof(GenericCollectionDebuggerVisualizer.DictionaryVisualizerDebuggerSide), 
                                                 typeof(VisualizerObjectSource), 
                                                 Target = typeof(Dictionary<,>), 
                                                 Description = "Dictionary Visualizer")]
namespace GenericCollectionDebuggerVisualizer
{
    public class DictionaryVisualizerDebuggerSide : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
                // Get the dictionary
                IDictionary dict = (IDictionary)objectProvider.GetObject();

                DictionaryVisualizerForm form = new DictionaryVisualizerForm();
                form.ProcessingDict = dict;
                
                windowService.ShowDialog(form);
            
        }
    }
}
