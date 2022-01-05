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

        public delegate void ActiveCommand(double x, double y);
        public ActiveCommand CurrentCommand;

        public CanvasClickedCommand(Canvas canv) 
        {
            GraphCanvas = canv;
            CurrentCommand = null;
        }

        public override void Execute(object parameter)
        {
            if (GraphCanvas is null)
                throw new Exception("Canvas not created!");
            if (CurrentCommand is null)
                throw new Exception("Command not set!");

            Point pos = Mouse.GetPosition(GraphCanvas);
            CurrentCommand(pos.X, pos.Y);
        }
    }
}
