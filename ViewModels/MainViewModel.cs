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

        public ICommand RunBreadthFirstSearchCommand { get; set; }
        public ICommand RunBestFirstSearchCommand { get; set; }
        public ICommand RunDijkstrasAlgorithmCommand { get; set; }

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
            
            TestC = new ActionOnCommand(REMOVE_LATER_ShowGraphInfo);

            SetVertexCreationToolCommand = new ActionOnCommand(SetVertexCreationTool);
            SetPositionToolCommand = new ActionOnCommand(SetPositionTool);
            SetEdgeCreationToolCommand = new ActionOnCommand(SetEdgeCreationTool);

            RunBreadthFirstSearchCommand = new ActionOnCommand(RunBreadthFirstSearch);
            RunBestFirstSearchCommand = new ActionOnCommand(RunBestFirstSearch);
            RunDijkstrasAlgorithmCommand = new ActionOnCommand(RunDijkstrasAlgorithm);

            GVM.UpdateSelectionInfo = UpdateSelectionInfo;

        }

        public void SetVertexCreationTool()
        {
            CanvasClicked.ActiveCommand = GVM.CreateVertex;
        }

        public void SetEdgeCreationTool()
        {
            if (GVM.MultipleVertexTool == null)
            {
                GVM.ResetSelectedVertexesBuffer();
                GVM.MultipleVertexTool = GVM.CreateEdge;
            }
            else
            {
                MessageBox.Show("Some another tool is already in use");
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

        public void RunBreadthFirstSearch()
        {
            if (GVM.MultipleVertexTool == null)
            {
                MessageBox.Show("Select two vertexes please");
                GVM.ResetSelectedVertexesBuffer();
                GVM.MultipleVertexTool = GVM.RunBreadthFirstSearch;
            }
            else
            {
                MessageBox.Show("Some another tool is already in use");
            }
        }

        public void RunBestFirstSearch()
        {
            if (GVM.MultipleVertexTool == null)
            {
                MessageBox.Show("Select two vertexes please");
                GVM.ResetSelectedVertexesBuffer();
                GVM.MultipleVertexTool = GVM.RunBestFirstSearch;
            }
            else
            {
                MessageBox.Show("Some another tool is already in use");
            }
        }

        public void RunDijkstrasAlgorithm()
        {
            if (GVM.MultipleVertexTool == null)
            {
                MessageBox.Show("Select vertex please");
                GVM.ResetSelectedVertexesBuffer();
                GVM.MultipleVertexTool = GVM.RunDijkstrasAlgorithm;
            }
            else
            {
                MessageBox.Show("Some another tool is already in use");
            }
        }




        public void REMOVE_LATER_ShowGraphInfo()
        {
            GVM.REMOVE_LATER_showMessageBoxWithGraphInfo();
        }
    }
}
