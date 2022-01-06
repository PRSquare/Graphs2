﻿using Graphs2.Commands;
using Graphs2.Models;
using Graphs2.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

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
        public bool EdgeCreationToolSelected = false;

        public Action UpdateSelectionInfo;

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
            SelectedVertexesBuffer = new VertexViewModel[2] { null, null };
        }

        public void ResetSelectedEdgesBuffer()
        {
            SelectedEdgesBuffer = new EdgeViewModel[2] { null, null };
        }

        private void _writeInBuffer( VertexViewModel vertex)
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
            if(EdgeCreationToolSelected == true)
            {
                CreateEdge();
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
        }

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
            if(SelectedVertexesBuffer[0] != null && SelectedVertexesBuffer[1] != null)
            {
                Edge edge = new Edge(GraphUtils.GetNewEdgeName());
                edge.RouteVert = SelectedVertexesBuffer[0]._vert;
                edge.ConnectedVert = SelectedVertexesBuffer[1]._vert;
                _graph.AddEdge(edge);
                Edges = _createEdgesCollection(_graph.Edges);
                EdgeCreationToolSelected = false;
            }
        }
    }
}
