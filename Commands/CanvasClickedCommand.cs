using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Graphs2.Commands
{
    public class CanvasClickedCommand : CommandBase
    {
        public Canvas GraphCanvas;

        public Action<double, double> ActiveCommand;

        public CanvasClickedCommand(Canvas canv) 
        {
            GraphCanvas = canv;
            ActiveCommand = null;
        }

        public override void Execute(object parameter)
        {
            if (GraphCanvas is null)
                throw new Exception("Canvas not created!");
            if (ActiveCommand is null)
                return;

            Point pos = Mouse.GetPosition(GraphCanvas);
            try
            {
                ActiveCommand(pos.X, pos.Y);
            }
            catch( Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
