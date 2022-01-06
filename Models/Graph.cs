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

        private void _clearVertexsesAfterSearch()
        {
            foreach (var vert in Vertexes)
                vert.Visited = false;
        }

        // Labs

        public int BreadthFistSearch( Vertex start, Vertex target)
        {
            if (!Vertexes.Exists(x => x == start))
                throw new Exception("Start point doesn't exist");
            if (!Vertexes.Exists(x => x == target))
                throw new Exception("Target point doesn't exist");

            if (start == target)
                return 0;

            bool found = false;

            Queue<Vertex> vertexes = new Queue<Vertex>();
            int length = 0;

            start.Visited = true;
            vertexes.Enqueue(start);
            while( vertexes.Count > 0 && !found)
            {
                Vertex v = vertexes.Dequeue();
                length++;

                foreach( var ed in v.ConnectedEdges)
                {
                    if (!ed.ConnectedVert.Visited)
                    {
                        ed.ConnectedVert.Visited = true;
                        vertexes.Enqueue(ed.ConnectedVert);

                        if( ed.ConnectedVert == target)
                        {
                            found = true;
                            break;
                        }
                    }
                }
            }

            _clearVertexsesAfterSearch();
            
            return length;
        }

        private Vertex evristic(List<Vertex> verts)
        {
            if (verts.Count == 0)
                throw new Exception("Emty vert list");
            Vertex ret = verts[0];
            int curConnected = ret.ConnectedEdges.Count;
            foreach( var vert in verts)
            {
                if( vert.ConnectedEdges.Count > curConnected )
                {
                    ret = vert;
                    curConnected = vert.ConnectedEdges.Count;
                }
            }
            return ret;
        }

        public int BestFirstSearch(Vertex start, Vertex target)
        {
            if (!Vertexes.Exists(x => x == start))
                throw new Exception("Start point doesn't exist");
            if (!Vertexes.Exists(x => x == target))
                throw new Exception("Target point doesn't exist");

            if (start == target)
                return 0;

            bool found = false;

            Queue<Vertex> vertexes = new Queue<Vertex>();
            int length = 0;

            start.Visited = true;
            vertexes.Enqueue(start);
            while (vertexes.Count > 0 && !found)
            {
                Vertex v = vertexes.Dequeue();
                length++;

                List<Vertex> curVerts = new List<Vertex>();
                foreach (var ed in v.ConnectedEdges)
                {
                    curVerts.Add(ed.ConnectedVert);
                }
                while(curVerts.Count > 0)
                {
                    Vertex curvert = evristic(curVerts);
                    curVerts.Remove(curvert);
                    if (!curvert.Visited)
                    {
                        curvert.Visited = true;
                        vertexes.Enqueue(curvert);

                        if (curvert == target)
                        {
                            found = true;
                            break;
                        }
                    }
                }
            }

            _clearVertexsesAfterSearch();

            return length;
        }


        class dijksrtasVert : IComparable
        {
            public Vertex Vert;
            public int Length;
            public bool Visited => Vert.Visited;
            public dijksrtasVert(Vertex vert, int length)
            {
                Vert = vert;
                Length = length;
            }

            public int CompareTo(object obj)
            {
                return Length.CompareTo((obj as dijksrtasVert).Length);
            }
        }

        public Dictionary<Vertex, int> DijkstrasAlgorithm( Vertex start)
        {
            if (!Vertexes.Exists(x => x == start))
                throw new Exception("Start point doesn't exist");

            List<dijksrtasVert> dv = new List<dijksrtasVert>();
            foreach (var vert in Vertexes)
            {
                if(vert == start)
                    dv.Add(new dijksrtasVert(vert, 0));
                else
                    dv.Add(new dijksrtasVert(vert, Int32.MaxValue));
            }



            while ( Vertexes.Exists( x => !x.Visited ) )
            {
                List<dijksrtasVert> leftedList = dv.FindAll(x => !x.Visited);
                dijksrtasVert cur = leftedList.Min();
                cur.Vert.Visited = true;
                foreach( Edge ed in cur.Vert.ConnectedEdges)
                {
                    Vertex analyseVert = ed.ConnectedVert;
                    if (analyseVert.Visited == true)
                        continue;
                    var v = dv.Find(x => x.Vert == analyseVert);
                    if (v.Length > ed.Weight + cur.Length)
                        v.Length = ed.Weight + cur.Length;
                }
            }
            

            _clearVertexsesAfterSearch();

            Dictionary<Vertex, int> retlist = new Dictionary<Vertex, int>();
            foreach( var a in dv )
            {
                retlist.Add(a.Vert, a.Length);
            }
            return retlist;
        }
    }
}
