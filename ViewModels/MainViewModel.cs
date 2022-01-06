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
        public ICommand SetPositionToolCommand { get; set; }
        public ICommand SetVertexCreationToolCommand { get; set; }
        public ICommand SetEdgeCreationToolCommand { get; set; }

        public CanvasClickedCommand CanvasClicked { get; set; }

        private Canvas _graphCanvas;
        public Canvas GraphCanvas
        {
            get => _graphCanvas;
            set
            {
                _graphCanvas = value;
                CanvasClicked = new CanvasClickedCommand(GraphCanvas);
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

        public MainViewModel(Graph graph)
        {
            GVM = new GraphViewModel(graph);
            TestC = new TestCommand();
            SetVertexCreationToolCommand = new ActionOnCommand(SetVertexCreationTool);
            SetPositionToolCommand = new ActionOnCommand(SetPositionTool);
            SetEdgeCreationToolCommand = new ActionOnCommand(SetEdgeCreationTool);

            GVM.UpdateSelectionInfo = UpdateSelectionInfo;

        }

        public void SetVertexCreationTool()
        {
            CanvasClicked.ActiveCommand = GVM.CreateVertex;
        }

        public void SetEdgeCreationTool()
        {
            if( GVM.EdgeCreationToolSelected == false )
            {
                GVM.ResetSelectedVertexesBuffer();
                GVM.EdgeCreationToolSelected = true;
            }
        }

        public void SetPositionTool()
        {
            CanvasClicked.ActiveCommand = GVM.ChangePosition;
        }

        public void UpdateSelectionInfo()
        {
            if (GVM.LastSelected == GraphViewModel.lastSelected.vertex)
            {
                CurrentObjectViewModel = GVM.SelectedVertexesBuffer[1];
            }
            else
            {
                CurrentObjectViewModel = GVM.SelectedEdgesBuffer[1];
            }
        }
    }
}
