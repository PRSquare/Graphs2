using Graphs2.Commands;
using Graphs2.Models;
using Graphs2.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Graphs2.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        public GraphViewModel GVM { get; set; }

        public ICommand TestC { get; set; }
        public CanvasClickedCommand CanvasClicked { get; set; }

        private Canvas _graphCanvas;
        public Canvas GraphCanvas
        {
            get => _graphCanvas;
            set
            {
                _graphCanvas = value;
                CanvasClicked = new CanvasClickedCommand(GraphCanvas);
                CanvasClicked.CurrentCommand = testFunc;
            }
        }

        private BaseViewModel _currentObjectViewModel;
        public BaseViewModel CurrentObjectViewModel
        {
            get => _currentObjectViewModel;
            set
            {
                _currentObjectViewModel = value;
                OnPropertyChanged(nameof(CurrentObjectViewModel));
            }
        }
        

        private NavigationStore _objectInfo;

        public MainViewModel(Graph graph)
        {
            GVM = new GraphViewModel(graph);
            TestC = new TestCommand();
        }

        public void testFunc(double x, double y)
        {
            CurrentObjectViewModel = GVM.Vertexes[0];
        }
    }
}
