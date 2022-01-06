﻿using System;
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

        // ================================================= Labs ================================================================

        // ==== 2 ====

        /// <summary>
        /// Breadth Fist Search
        /// </summary>
        /// <param name="start">Start vertex</param>
        /// <param name="target">Vertex to find</param>
        /// <returns>Count of edges to vert or -1 if there is no path</returns>
        public int BreadthFistSearch( Vertex start, Vertex target)
        {
            if (!Vertexes.Exists(x => x == start))
                throw new Exception("Start point doesn't exist");
            if (!Vertexes.Exists(x => x == target))
                throw new Exception("Target point doesn't exist");

            if (start == target)
                return 0;

            Queue<Vertex> vertexes = new Queue<Vertex>();
            
            int length = 0; // Length of path (passed edges count)

            start.Visited = true; // Start vertex is visited
            vertexes.Enqueue(start); // Pushing start vertex in queue

            while( vertexes.Count > 0)
            {
                Vertex v = vertexes.Dequeue(); // Dequeueing vertex
                length++; // Increasing path length

                foreach( var ed in v.ConnectedEdges) // For each connected vertex
                {
                    if (!ed.ConnectedVert.Visited) // If vertex wasn't visited
                    {
                        ed.ConnectedVert.Visited = true; // Now it is visited
                        vertexes.Enqueue(ed.ConnectedVert); // Pushing this vertex in queue

                        if( ed.ConnectedVert == target) // Vertex found!
                        {
                            _clearVertexsesAfterSearch(); // Clearing vertexes visited state
                            return length;
                        }
                    }
                }
            }

            _clearVertexsesAfterSearch(); // Clearing vertexes visited state
            return -1; // If vertex was not found returns -1
        }


        // ==== 3 ====

        private Vertex evristic(List<Vertex> verts) // Evtistic function to find more preferable node
        {
            if (verts.Count == 0)
                throw new Exception("Emty vert list");

            Vertex ret = verts[0]; // take first Vertex (node)
            int curConnected = ret.ConnectedEdges.Count; // count of connected edges (vertexes)
            foreach( var vert in verts)
            {
                if( vert.ConnectedEdges.Count > curConnected ) // If vertex has more connections then current take it as current
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


        // ==== 4 ====


        // Class to store twos - Vert and Length for Dijksrtas algorythm
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

            List<dijksrtasVert> dv = new List<dijksrtasVert>(); // Creating list of vertexes with markers (length)
            foreach (var vert in Vertexes)
            {
                if(vert == start)
                    dv.Add(new dijksrtasVert(vert, 0)); // If start vert length = 0
                else
                    dv.Add(new dijksrtasVert(vert, Int32.MaxValue)); // Else it is set to infinity (maxint)
            }



            while ( Vertexes.Exists( x => !x.Visited ) ) // While not all vertexes were visited
            {
                List<dijksrtasVert> leftedList = dv.FindAll(x => !x.Visited); // List of not visited vertexes
                dijksrtasVert cur = leftedList.Min(); // Take vertex with minimal length
                cur.Vert.Visited = true; // It is visited now
                foreach( Edge ed in cur.Vert.ConnectedEdges) // Finding path length to all connected vertexes 
                {
                    Vertex analyseVert = ed.ConnectedVert;
                    
                    if (analyseVert.Visited == true) // Do not check visited vertexes
                        continue;

                    var v = dv.Find(x => x.Vert == analyseVert); // Take vertex from list of vertexes with markers (List<dijksrtasVert> dv)
                    if (v.Length > ed.Weight + cur.Length) // If there is shorter path, rewriting length
                        v.Length = ed.Weight + cur.Length;
                }
            }
            

            _clearVertexsesAfterSearch(); // Clearing vertexes visited state

            // Translating from List<dijksrtasVert> to Dictionary<Vertex, int>

            Dictionary<Vertex, int> retlist = new Dictionary<Vertex, int>();
            foreach( var a in dv )
            {
                retlist.Add(a.Vert, a.Length);
            }
            return retlist;
        }
    }
}
