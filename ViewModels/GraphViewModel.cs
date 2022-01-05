using Graphs2.Commands;
using Graphs2.Models;
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
        public ObservableCollection<VertexViewModel> Vertexes { get; set; }
        public ObservableCollection<EdgeViewModel> Edges { get; set; }

        private BaseObject _selectedObject;

        private Graph _graph;

        public GraphViewModel(Graph graph) 
        {
            _graph = graph;

            Vertexes = new ObservableCollection<VertexViewModel>();
            Edges = new ObservableCollection<EdgeViewModel>();

            foreach (var vertex in graph.Vertexes) 
            {
                VertexViewModel vvm = new VertexViewModel(vertex);
                vvm.OnSelection = ChangeSelected;
                vvm.OnUnselection = DisableSelection;
                vvm.OnDelete = DeleteVertex;
                Vertexes.Add(vvm);
            }
            foreach( var edge in graph.Edges)
            {
                EdgeViewModel evm = new EdgeViewModel(edge);
                evm.OnSelection = ChangeSelected;
                evm.OnUnselection = DisableSelection;
                evm.OnDelete = DeleteEdge;
                Edges.Add(evm);
            }

            _selectedObject = null;
        }

        public void ChangeSelected(BaseObject obj) 
        {
            _disableOtherSelection(obj);
            _selectedObject = obj;
        }

        public void DisableSelection() 
        {
            _selectedObject = null;
        }

        private void _disableOtherSelection(BaseObject obj) 
        {
            foreach (var vertex in Vertexes)
                if( vertex != obj )
                    vertex.DisableSelection();
            foreach (var edge in Edges)
                if(edge != obj)
                    edge.DisableSelection();
        }

        public void ChangePosition( double x, double y ) 
        {
            if (_selectedObject == null)
                throw new Exception("Object not selected");

            _selectedObject.ChangePosition(x, y);

            foreach (var edge in Edges)
                edge.UpdateCords();
        }

        public void DeleteVertex(VertexViewModel vertex)
        {
            Vertexes.Remove(vertex);
        }

        public void DeleteEdge(EdgeViewModel edge)
        {
            Edges.Remove(edge);
        }
    }
}
