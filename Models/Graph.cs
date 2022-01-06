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

        public Graph(List<Vertex> vertexes)
        {
            Recreate(vertexes);
        }

        public void Recreate( List<Vertex> vertexes ) 
        {
            Vertexes = new List<Vertex>();
            Edges = new List<Edge>();
            foreach (var vertex in vertexes)
                AddVertex(vertex);
        }

        public void UpdateEdges()
        {
            Edges = _createEdgeListWithoutUnconected(Edges, Vertexes);
        }

        public bool AddVertex( Vertex vertex)
        {
            if(!Vertexes.Exists(x => x.Name == vertex.Name))
            {
                foreach (var edge in vertex.ConnectedEdges)
                {
                    AddEdge(edge);
                }
                Vertexes.Add(vertex);
                return true;
            }
            return false;
        }

        public bool AddEdge(Edge edge)
        {
            if (!Edges.Exists(x => x == edge))
            {
                List<Edge> sameDirectedEdges = Edges.FindAll(x => { return x.RouteVert == edge.RouteVert && x.ConnectedVert == edge.ConnectedVert; });
                edge.EdgeNumber = sameDirectedEdges.Count;
                Edges.Add(edge);
                return true;
            }
            return false;
        }

        public bool RemoveVertex( Vertex vertex )
        {
            bool isSuccessfull = Vertexes.Remove(vertex);
            if (isSuccessfull)
                Edges = _createEdgeListWithoutUnconected(Edges, Vertexes);
            return isSuccessfull;
        }

        public bool RemoveEdge( Edge edge )
        {
            return Edges.Remove(edge);
        }

        private List<Edge> _createEdgeListWithoutUnconected( List<Edge> edges, List<Vertex> vertexes)
        {
            List<Edge> retList = new List<Edge>();
            foreach (var edge in edges)
            {
                if (Vertexes.Exists(x => x == edge.RouteVert ) && Vertexes.Exists(x => x == edge.ConnectedVert))
                    retList.Add(edge);
            }
            return retList;
        }
    }
}
