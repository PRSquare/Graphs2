using Graphs2.Commands;
using Graphs2.Models;
using Graphs2.Stores;
using Graphs2.Utils;
using Microsoft.Win32;
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
        private GraphViewModel _gvm;
        public GraphViewModel GVM
        {
            get => _gvm;
            set
            {
                _gvm = value;
                OnPropertyChanged(nameof(GVM));
            }
        }

        private AdjacentyMatrixViewModel _amvm;
        public AdjacentyMatrixViewModel AMVM
        {
            get => _amvm;
            set
            {
                _amvm = value;
                OnPropertyChanged(nameof(AMVM));
            }
        }

        public ICommand TestC { get; set; }
        public ICommand SetPositionToolCommand { get; set; }
        public ICommand SetVertexCreationToolCommand { get; set; }
        public ICommand SetEdgeCreationToolCommand { get; set; }

        public ICommand MakeEdgesWeightEqualsLengthCommand { get; set; }

        public ICommand RunBreadthFirstSearchCommand { get; set; }
        public ICommand RunBestFirstSearchCommand { get; set; }
        public ICommand RunDijkstrasAlgorithmCommand { get; set; }
        public ICommand RunAStarAlgorithmCommand { get; set; }
        public ICommand RunRadDiamFinderCommand { get; set; }
        public ICommand RunIsomorphCheckCommand { get; set;}
        public ICommand RunConnectionCheckCommand { get; set; }

        public ICommand ImportAdjacentyMatrixCommand { get; set; }
        public ICommand CreateFromGraphCodeFileCommand { get; set; }

        public ICommand SaveToGraphCodeFileCommand { get; set; }

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
            _recreate(graph);
        }

        private void _recreate( Graph graph)
        {
            GVM = new GraphViewModel(graph);
            AMVM = new AdjacentyMatrixViewModel(new AdjacentyMatrix(graph));

            TestC = new ActionOnCommand(REMOVE_LATER_ShowGraphInfo);

            SetVertexCreationToolCommand = new ActionOnCommand(SetVertexCreationTool);
            SetPositionToolCommand = new ActionOnCommand(SetPositionTool);
            SetEdgeCreationToolCommand = new ActionOnCommand(SetEdgeCreationTool);

            MakeEdgesWeightEqualsLengthCommand = new ActionOnCommand(MakeEdgesWeightEqualsLength);

            RunBreadthFirstSearchCommand = new ActionOnCommand(RunBreadthFirstSearch);
            RunBestFirstSearchCommand = new ActionOnCommand(RunBestFirstSearch);
            RunDijkstrasAlgorithmCommand = new ActionOnCommand(RunDijkstrasAlgorithm);
            RunAStarAlgorithmCommand = new ActionOnCommand(RunAStarAlgorithm);
            RunRadDiamFinderCommand = new ActionOnCommand(RunRadDiamFinder);
            RunIsomorphCheckCommand = new ActionOnCommand(RunIsomorphCheck);
            RunConnectionCheckCommand = new ActionOnCommand(RunConnectionCheck);

            ImportAdjacentyMatrixCommand = new ActionOnCommand(CreateFromAdjacentyMatrix);
            CreateFromGraphCodeFileCommand = new ActionOnCommand(CreateFromGraphCodeFile);

            SaveToGraphCodeFileCommand = new ActionOnCommand(SaveToGraphCodeFile);

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

        public void RunAStarAlgorithm()
        {
            if (GVM.MultipleVertexTool == null)
            {
                MessageBox.Show("Select two vertexes please");
                GVM.ResetSelectedVertexesBuffer();
                GVM.MultipleVertexTool = GVM.RunAStar;
            }
            else
            {
                MessageBox.Show("Some another tool is already in use");
            }
        }

        public void RunRadDiamFinder()
        {
            GVM.RunFindRadDim();
        }

        public void RunIsomorphCheck()
        {
            MessageBox.Show("Chose a file with graph adjacenty matrix");
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Text files (*.txt)|*.txt";
            String fileName = "";
            if (fd.ShowDialog() == true)
                fileName = fd.FileName;

            String buff = FileUtils.ReadFile(fileName);

            GVM.IsomorphCheck(GraphUtils.ImportFromAdjacentyMatrix(buff));
        }

        public void RunConnectionCheck()
        {
            GVM.ConnectionCheck();
        }

        public void CreateFromAdjacentyMatrix()
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Text files (*.txt)|*.txt";
            String fileName = "";
            if (fd.ShowDialog() == true)
                fileName = fd.FileName;

            String buff = FileUtils.ReadFile(fileName);

            _recreate(GraphUtils.ImportFromAdjacentyMatrix(buff));
        }

        public void CreateFromGraphCodeFile()
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Text files (*.txt)|*.txt";
            String fileName = "";
            if (fd.ShowDialog() == true)
                fileName = fd.FileName;

            String buff = FileUtils.ReadFile(fileName);

            _recreate(GraphUtils.CreateGraphGromEdgVertList(buff));
        }

        public void SaveToGraphCodeFile()
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Text files (*.txt)|*.txt";
            String fileName = "";
            if (fd.ShowDialog() == true)
                fileName = fd.FileName;

            if(fileName != "" && !(fileName is null))
                GVM.SaveToGraphCodeFile(fileName);
        }

        public void MakeEdgesWeightEqualsLength()
        {
            GVM.MakeEdgesWeightEqualsLength();
        }

        public void REMOVE_LATER_ShowGraphInfo()
        {
            GVM.REMOVE_LATER_showMessageBoxWithGraphInfo();
            //MessageBox.Show(AMVM.AdjMat.ToString());
        }
    }
}
