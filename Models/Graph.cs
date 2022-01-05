using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs2.Models
{
    public class Graph
    {
        public List<Vertex> Vertexes;
        public List<Edge> Edges;

        public Graph( List<Vertex> vertexes, List<Edge> edges) 
        {
            Vertexes = vertexes;
            Edges = edges;
        }
    }
}
