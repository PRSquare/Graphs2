using Graphs2.Commands;
using Graphs2.Models;
using Graphs2.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Graphs2.ViewModels
{
    public class GraphViewModel : BaseViewModel
    {
        private ObservableCollection<VertexViewModel> _vertexes;
        public ObservableCollection<VertexViewModel> Vertexes
        {
            get => _vertexes;
            set
            {
                _vertexes = value;
                OnPropertyChanged(nameof(Vertexes));
            }
        }

        private ObservableCollection<EdgeViewModel> _edges;
        public ObservableCollection<EdgeViewModel> Edges
        {
            get => _edges;
            set
            {
                _edges = value;
                OnPropertyChanged(nameof(Edges));
            }
        }

        public BaseObject SelectedObject;
        public VertexViewModel[] SelectedVertexesBuffer = new VertexViewModel[2];
        public EdgeViewModel[] SelectedEdgesBuffer = new EdgeViewModel[2];
        public enum lastSelected { vertex, edge };
        public lastSelected LastSelected = lastSelected.vertex;

        public Action UpdateSelectionInfo;

        public Action MultipleVertexTool = null;

        private Graph _graph;

        public GraphViewModel(Graph graph)
        {
            _graph = graph;

            Vertexes = _createVertexesCollection(_graph.Vertexes);
            Edges = _createEdgesCollection(_graph.Edges);

            ResetSelectedEdgesBuffer();
            ResetSelectedVertexesBuffer();

            SelectedObject = null;
        }

        private ObservableCollection<VertexViewModel> _createVertexesCollection(List<Vertex> vertexes)
        {
            ObservableCollection<VertexViewModel> retColl = new ObservableCollection<VertexViewModel>();
            foreach (var vertex in vertexes)
            {
                VertexViewModel vvm = new VertexViewModel(vertex);
                vvm.OnSelection = ChangeSelected;
                vvm.OnUnselection = DisableSelection;
                vvm.OnDelete = DeleteVertex;
                retColl.Add(vvm);
            }
            return retColl;
        }

        private ObservableCollection<EdgeViewModel> _createEdgesCollection(List<Edge> edges)
        {
            ObservableCollection<EdgeViewModel> retColl = new ObservableCollection<EdgeViewModel>();
            foreach (var edge in edges)
            {
                /*Edge existingEdge = edges.Find(x => x.IsOposite(edge));
                if(existingEdge != null)
                {
                    retColl.Find(x => x._edge == existingEdge).edges.Add(edge);
                    continue;
                }*/
                EdgeViewModel evm = new EdgeViewModel(edge);
                evm.OnSelection = ChangeSelected;
                evm.OnUnselection = DisableSelection;
                evm.OnDelete = DeleteEdge;
                retColl.Add(evm);
            }
            return retColl;
        }

        public void ResetSelectedVertexesBuffer()
        {
            DisableSelection();
            SelectedVertexesBuffer = new VertexViewModel[2] { null, null };
        }

        public void ResetSelectedEdgesBuffer()
        {
            SelectedEdgesBuffer = new EdgeViewModel[2] { null, null };
        }

        private void _writeInBuffer(VertexViewModel vertex)
        {
            SelectedVertexesBuffer[0] = SelectedVertexesBuffer[1];
            SelectedVertexesBuffer[1] = vertex;
        }

        private void _writeInBuffer(EdgeViewModel edge)
        {
            SelectedEdgesBuffer[0] = SelectedEdgesBuffer[1];
            SelectedEdgesBuffer[1] = edge;
        }

        public void ChangeSelected(VertexViewModel obj)
        {
            _disableOtherSelection(obj);
            SelectedObject = obj;
            _writeInBuffer(obj);
            if (MultipleVertexTool != null)
            {
                MultipleVertexTool();
            }
            LastSelected = lastSelected.vertex;

            UpdateSelectionInfo();
        }

        public void ChangeSelected(EdgeViewModel obj)
        {
            _disableOtherSelection(obj);
            SelectedObject = obj;
            _writeInBuffer(obj);
            LastSelected = lastSelected.edge;

            UpdateSelectionInfo();
        }

        public void DisableSelection()
        {
            SelectedObject?.setDefaultState();
            SelectedObject = null;
        }

        private void _disableOtherSelection(BaseObject obj)
        {
            foreach (var vertex in Vertexes)
                if (vertex != obj)
                    vertex.DisableSelection();
            foreach (var edge in Edges)
                if (edge != obj)
                    edge.DisableSelection();
        }

        public void ChangePosition(double x, double y)
        {
            if (SelectedObject == null)
                throw new Exception("Object not selected");

            SelectedObject.ChangePosition(x, y);

            foreach (var edge in Edges)
                edge.UpdateCords();

            // REMOVE_LATER_showMessageBoxWithGraphInfo();
        }

        // TEMP!!!!
        public void REMOVE_LATER_showMessageBoxWithGraphInfo()
        {
            string buffer = "";
            foreach (var vert in _graph.Vertexes)
            {
                buffer += vert.Name + ": ";
                foreach (var ed in vert.ConnectedEdges)
                    buffer += ed.ConnectedVert.Name + " ";
                buffer += "\n";
            }
            MessageBox.Show(buffer);
        }
        // ========

        public void DeleteVertex(VertexViewModel vertex)
        {
            _graph.RemoveVertex(vertex._vert);

            Vertexes = _createVertexesCollection(_graph.Vertexes);
            Edges = _createEdgesCollection(_graph.Edges);
        }

        public void DeleteEdge(EdgeViewModel edge)
        {
            _graph.RemoveEdge(edge._edge);
            Edges = _createEdgesCollection(_graph.Edges);
        }

        public void CreateVertex(double x, double y)
        {
            Vertex vertex = new Vertex(GraphUtils.GetNewVerteName());
            vertex.X = x; vertex.Y = y;
            while (_graph.AddVertex(vertex) == false)
                vertex.Name = GraphUtils.GetNewVerteName();
            Vertexes = _createVertexesCollection(_graph.Vertexes);
        }

        public void CreateEdge()
        {
            if (SelectedVertexesBuffer[0] != null && SelectedVertexesBuffer[1] != null)
            {
                Edge edge = new Edge(GraphUtils.GetNewEdgeName());
                edge.RouteVert = SelectedVertexesBuffer[0]._vert;
                edge.ConnectedVert = SelectedVertexesBuffer[1]._vert;
                edge.ConnectVertexes();
                _graph.AddEdge(edge);
                Edges = _createEdgesCollection(_graph.Edges);
                MultipleVertexTool = null;
            }
        }

        public void RunBreadthFirstSearch()
        {
            if (SelectedVertexesBuffer[0] != null && SelectedVertexesBuffer[1] != null)
            {
                DisableSelection();
                List<Vertex> result = _graph.BreadthFristSearch(SelectedVertexesBuffer[0]._vert, SelectedVertexesBuffer[1]._vert);
                if (result == null)
                    MessageBox.Show($"There is no path between {SelectedVertexesBuffer[0].Name} and {SelectedVertexesBuffer[1].Name}");
                else
                {
                    foreach (var vert in Vertexes)
                    {
                        if (result.Exists(x => x == vert._vert))
                            vert.ObjectColor = new SolidColorBrush(Colors.Green);
                    }
                    for (int i = 0; i < result.Count - 1; ++i)
                    {
                        Edge fEdge = result[i + 1].ConnectedEdges.Find(x => x.ConnectedVert == result[i]);
                        foreach (var ed in Edges)
                        {
                            if (ed._edge == fEdge)
                                ed.ObjectColor = new SolidColorBrush(Colors.Green);
                        }
                    }
                    MessageBox.Show($"Path length between {SelectedVertexesBuffer[0].Name} and {SelectedVertexesBuffer[1].Name} equals {result.Count - 1}");
                }

                MultipleVertexTool = null;

                string retBuff = "";
                if (!(result is null))
                    foreach (var vert in result)
                        retBuff += vert.Name + "\n";

                FileUtils.SaveToFile("out.txt", retBuff);
            }
        }

        public void RunBestFirstSearch()
        {
            if (SelectedVertexesBuffer[0] != null && SelectedVertexesBuffer[1] != null)
            {
                DisableSelection();
                List<Vertex> result = _graph.BestFirstSearch(SelectedVertexesBuffer[0]._vert, SelectedVertexesBuffer[1]._vert);
                if (result == null)
                    MessageBox.Show($"There is no path between {SelectedVertexesBuffer[0].Name} and {SelectedVertexesBuffer[1].Name}");
                else
                {
                    foreach (var vert in Vertexes)
                    {
                        if (result.Exists(x => x == vert._vert))
                            vert.ObjectColor = new SolidColorBrush(Colors.Green);
                    }
                    for (int i = 0; i < result.Count - 1; ++i)
                    {
                        Edge fEdge = result[i + 1].ConnectedEdges.Find(x => x.ConnectedVert == result[i]);
                        foreach (var ed in Edges)
                        {
                            if (ed._edge == fEdge)
                                ed.ObjectColor = new SolidColorBrush(Colors.Green);
                        }
                    }
                    MessageBox.Show($"Path length between {SelectedVertexesBuffer[0].Name} and {SelectedVertexesBuffer[1].Name} equals {result.Count - 1}");
                }
                MultipleVertexTool = null;

                string retBuff = "";
                if (!(result is null))
                    foreach (var vert in result)
                        retBuff += vert.Name + "\n";

                FileUtils.SaveToFile("out.txt", retBuff);
            }
        }

        public void RunDijkstrasAlgorithm()
        {
            if (SelectedVertexesBuffer[1] != null)
            {
                DisableSelection();
                Dictionary<Vertex, int> result = _graph.DijkstrasAlgorithm(SelectedVertexesBuffer[1]._vert);
                string buffer = "Path lengths: \n";
                foreach (var d in result)
                    buffer += d.Key.Name + ": " + d.Value.ToString() + "\n";
                MessageBox.Show(buffer);

                MultipleVertexTool = null;

                FileUtils.SaveToFile("out.txt", buffer);
            }
        }

        public void RunAStar()
        {
            if (SelectedVertexesBuffer[0] != null && SelectedVertexesBuffer[1] != null)
            {
                DisableSelection();

                Dictionary<Vertex, double> result = _graph.AStar(SelectedVertexesBuffer[0]._vert, SelectedVertexesBuffer[1]._vert);

                List<Vertex> res = new List<Vertex>();
                foreach (var k in result.Keys)
                    res.Add(k);

                if (!(result is null))
                {
                    foreach (var vert in Vertexes)
                    {
                        if (res.Exists(x => x == vert._vert))
                            vert.ObjectColor = new SolidColorBrush(Colors.Green);
                    }
                    for (int i = 0; i < res.Count - 1; ++i)
                    {
                        Edge fEdge = res[i + 1].ConnectedEdges.Find(x => x.ConnectedVert == res[i]);
                        foreach (var ed in Edges)
                        {
                            if (ed._edge == fEdge)
                                ed.ObjectColor = new SolidColorBrush(Colors.Green);
                        }
                    }

                    string buffer = "Path lengths: \n";
                    foreach (var d in result)
                        buffer += d.Key.Name + ": " + d.Value.ToString() + "\n";
                    MessageBox.Show($"Path lenhth between {SelectedVertexesBuffer[0].Name} and {SelectedVertexesBuffer[1].Name} equals {result.First().Value}\n{buffer}");
                    FileUtils.SaveToFile("out.txt", buffer);
                }
                else
                {
                    MessageBox.Show($"There is no path between {SelectedVertexesBuffer[0].Name} and {SelectedVertexesBuffer[1].Name}");
                }

                MultipleVertexTool = null;

            }
        }

        public void RunFindRadDim()
        {
            Vertex[] result;
            int radius; int diameter;
            try
            {
                result = _graph.RadDimFinder(out radius, out diameter);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

            string centersOut = "Centers: ";
            for( int i = 0; i < result.Length; ++i )
            {
                centersOut += result[i].Name + " ";
                foreach( var v in Vertexes)
                {
                    if (v._vert == result[i])
                    {
                        v.ObjectColor = new SolidColorBrush(Colors.Blue);
                        break;
                    }
                }
            }
            MessageBox.Show($"Radius equals {radius}.\nDiameter equals {diameter}\n{centersOut}");
        }
    }
}
