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

        public void Recreate(List<Vertex> vertexes)
        {
            Vertexes = new List<Vertex>();
            Edges = new List<Edge>();
            foreach (var vertex in vertexes)
                AddVertex(vertex);
        }

        public void UpdateEdges()
        {
            Edges = _createEdgeListWithoutUnconected(Edges, Vertexes);
            CheckEdgesDirection();
        }

        public void CheckEdgesDirection()
        {
            foreach (var ed in Edges)
            {
                if (ed.ConnectedVert.ConnectedEdges.Exists(x => x.IsOposite(ed) == true))
                    ed.IsDirected = false;
                else
                    ed.IsDirected = true;
            }
        }

        public bool AddVertex(Vertex vertex)
        {
            if (!Vertexes.Exists(x => x.Name == vertex.Name))
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

                CheckEdgesDirection();

                return true;
            }
            return false;
        }

        public bool RemoveVertex(Vertex vertex)
        {
            bool isSuccessfull = Vertexes.Remove(vertex);
            if (isSuccessfull)
                UpdateEdges();
            return isSuccessfull;
        }

        public bool RemoveEdge(Edge edge)
        {
            edge.RouteVert.RemoveConnectedEdge(edge);
            bool success = Edges.Remove(edge);
            CheckEdgesDirection();
            return success;
        }

        private List<Edge> _createEdgeListWithoutUnconected(List<Edge> edges, List<Vertex> vertexes)
        {
            List<Edge> retList = new List<Edge>();
            foreach (var edge in edges)
            {
                if (Vertexes.Exists(x => x == edge.RouteVert) && Vertexes.Exists(x => x == edge.ConnectedVert))
                    retList.Add(edge);
                else
                    edge.RouteVert?.RemoveConnectedEdge(edge);
            }
            return retList;
        }

        public void _clearVertexsesAfterSearch()
        {
            foreach (var vert in Vertexes)
                vert.Visited = false;
        }

        public object Clone()
        {
            List<Vertex> verts = new List<Vertex>();
            foreach (var ed in Edges)
            {
                Vertex v1 = verts.Find(x => x.Name == ed.RouteVert.Name);
                Vertex v2 = verts.Find(x => x.Name == ed.ConnectedVert.Name);
                if (v1 is null)
                {
                    v1 = new Vertex(ed.RouteVert.Name);
                    verts.Add(v1);
                }
                if (v2 is null)
                {
                    v2 = new Vertex(ed.ConnectedVert.Name);
                    verts.Add(v2);
                }

                Edge edge = new Edge(ed.Name, ed.Weight);
                edge.RouteVert = v1; edge.ConnectedVert = v2;
                edge.ConnectVertexes();
            }
            return new Graph(verts);
        }

        public void MakeEdgesWeightEqualsLength()
        {
            foreach (var ed in Edges)
            {
                double xl = ed.ConnectedVert.X - ed.RouteVert.X;
                double yl = ed.ConnectedVert.Y - ed.RouteVert.Y;
                ed.Weight = (int)Math.Sqrt(xl * xl + yl * yl);
            }
        }

        public static Graph CreateNotConnected(int count)
        {
            List<Vertex> verts = new List<Vertex>();
            for (int i = 0; i < count; ++i)
                verts.Add(new Vertex($"Vertex_{i}"));
            return new Graph(verts);
        }
        public static Graph CreateNotConnected(Graph graph)
        {
            return CreateNotConnected(graph.Vertexes);
        }
        public static Graph CreateNotConnected(List<Vertex> allverts)
        {
            List<Vertex> verts = new List<Vertex>();
            foreach (var v in allverts)
                verts.Add(new Vertex(v.Name));
            return new Graph(verts);
        }

        public static Graph CreateConnected(Graph graph)
        {
            Graph thisGraph = Graph.CreateNotConnected(graph);
            foreach(var vert1 in thisGraph.Vertexes)
            {
                foreach(var vert2 in thisGraph.Vertexes)
                {
                    if(vert1 != vert2)
                    {
                        Edge ed = new Edge();
                        ed.RouteVert = vert1;
                        ed.ConnectedVert = vert2;
                        ed.ConnectVertexes();
                        thisGraph.AddEdge(ed);
                    }
                }
            }
            return thisGraph;
        }
    }
}