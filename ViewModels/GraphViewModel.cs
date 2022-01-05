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

        private Graph _graph;

        public GraphViewModel(Graph graph) 
        {
            _graph = graph;

            Vertexes = new ObservableCollection<VertexViewModel>();
            Edges = new ObservableCollection<EdgeViewModel>();

            foreach (var vertex in graph.Vertexes) 
            {
                VertexViewModel vvm = new VertexViewModel(vertex);
                Vertexes.Add(vvm);
            }
            foreach( var edge in graph.Edges)
            {
                EdgeViewModel evm = new EdgeViewModel(edge);
                Edges.Add(evm);
            }
        }
    }
}
