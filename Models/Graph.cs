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
                return true;
            }
            return false;
        }

        public bool RemoveVertex(Vertex vertex)
        {
            bool isSuccessfull = Vertexes.Remove(vertex);
            if (isSuccessfull)
                Edges = _createEdgeListWithoutUnconected(Edges, Vertexes);
            return isSuccessfull;
        }

        public bool RemoveEdge(Edge edge)
        {
            edge.RouteVert.RemoveConnectedEdge(edge);
            return Edges.Remove(edge);
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
        public List<Vertex> BreadthFristSearch(Vertex start, Vertex target)
        {
            if (!Vertexes.Exists(x => x == start))
                throw new Exception("Start point doesn't exist");
            if (!Vertexes.Exists(x => x == target))
                throw new Exception("Target point doesn't exist");

            if (start == target)
            {
                List<Vertex> ret = new List<Vertex>();
                ret.Add(start);
                return ret;
            }

            Queue<Vertex> vertexes = new Queue<Vertex>();

            Dictionary<Vertex, Vertex> RouteVertsForVert = new Dictionary<Vertex, Vertex>();

            start.Visited = true; // Start vertex is visited
            vertexes.Enqueue(start); // Pushing start vertex in queue

            while (vertexes.Count > 0)
            {
                Vertex v = vertexes.Dequeue(); // Dequeueing vertex

                foreach (var ed in v.ConnectedEdges) // For each connected vertex
                {
                    if (!ed.ConnectedVert.Visited) // If vertex wasn't visited
                    {
                        ed.ConnectedVert.Visited = true; // Now it is visited
                        vertexes.Enqueue(ed.ConnectedVert); // Pushing this vertex in queue

                        RouteVertsForVert.Add(ed.ConnectedVert, v);

                        if (ed.ConnectedVert == target) // Vertex found!
                        {
                            _clearVertexsesAfterSearch(); // Clearing vertexes visited state

                            List<Vertex> retList = new List<Vertex>();
                            retList.Add(ed.ConnectedVert);

                            Vertex prevVert = RouteVertsForVert[ed.ConnectedVert];

                            while (prevVert != start)
                            {
                                retList.Add(prevVert);
                                prevVert = RouteVertsForVert[prevVert];
                            }


                            retList.Add(prevVert);

                            return retList;
                        }
                    }
                }
            }

            _clearVertexsesAfterSearch(); // Clearing vertexes visited state
            return null; // If vertex was not found returns -1
        }


        // ==== 3 ====

        private Vertex heuristicConnectionsCount(List<Vertex> verts) // Heuristic function to find more preferable node
        {
            if (verts.Count == 0)
                throw new Exception("Emty vert list");

            Vertex ret = verts[0]; // take first Vertex (node)
            int curConnected = ret.ConnectedEdges.Count; // count of connected edges (vertexes)
            foreach (var vert in verts)
            {
                if (vert.ConnectedEdges.Count > curConnected) // If vertex has more connections then current take it as current
                {
                    ret = vert;
                    curConnected = vert.ConnectedEdges.Count;
                }
            }
            return ret;
        }

        /*private Vertex heuristicPathLength(List<Vertex> verts, Vertex goal)
        {
            if (verts.Count == 0)
                throw new Exception("Emty vert list");

            if (goal == null)
                throw new Exception("Goal vertex is null");

            Vertex ret = verts[0];
            double curDist = Math.Abs(goal.X - ret.X) + Math.Abs(goal.Y - ret.Y);
            foreach( var vert in verts)
            {
                double dist = curDist = Math.Abs(goal.X - vert.X) + Math.Abs(goal.Y - vert.Y);
                if (dist < curDist)
                    ret = vert;
            }

            return ret;
        }*/

        private double heuristicPathLength(Vertex vert1, Vertex vert2)
        {
            return Math.Abs(vert1.X - vert2.X) + Math.Abs(vert1.Y - vert2.Y);
        }


        public List<Vertex> BestFirstSearch(Vertex start, Vertex target)
        {
            if (!Vertexes.Exists(x => x == start))
                throw new Exception("Start point doesn't exist");
            if (!Vertexes.Exists(x => x == target))
                throw new Exception("Target point doesn't exist");

            if (start == target)
            {
                List<Vertex> ret = new List<Vertex>();
                ret.Add(start);
                return ret;
            }

            bool found = false;

            Queue<Vertex> vertexes = new Queue<Vertex>();
            Dictionary<Vertex, Vertex> RouteVertsForVert = new Dictionary<Vertex, Vertex>();

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
                    Vertex curvert = heuristicConnectionsCount(curVerts);
                    curVerts.Remove(curvert);
                    if (!curvert.Visited)
                    {
                        curvert.Visited = true;
                        vertexes.Enqueue(curvert);

                        RouteVertsForVert.Add(curvert, v);

                        if (curvert == target)
                        {
                            _clearVertexsesAfterSearch(); // Clearing vertexes visited state

                            List<Vertex> retList = new List<Vertex>();
                            retList.Add(curvert);

                            Vertex prevVert = RouteVertsForVert[curvert];

                            while (prevVert != start)
                            {
                                retList.Add(prevVert);
                                prevVert = RouteVertsForVert[prevVert];
                            }

                            retList.Add(prevVert);

                            return retList;
                        }
                    }
                }
            }

            _clearVertexsesAfterSearch();

             return null;
        }


        // ==== 4 ====


        // Class to store twos - Vert and Length for Dijksrtas algorythm
        class vertWithPathLength : IComparable
        {
            public Vertex Vert;
            public double Length;
            public bool Visited => Vert.Visited;
            public vertWithPathLength(Vertex vert, double length)
            {
                Vert = vert;
                Length = length;
            }

            public int CompareTo(object obj)
            {
                return Length.CompareTo((obj as vertWithPathLength).Length);
            }
        }


        
        public Dictionary<Vertex, int> DijkstrasAlgorithm( Vertex start)
        {
            if (!Vertexes.Exists(x => x == start))
                throw new Exception("Start point doesn't exist");

            foreach (var edge in Edges)
                if (edge.Weight < 0)
                    throw new Exception($"{edge.Name} has negative weight");

            List<vertWithPathLength> dv = new List<vertWithPathLength>(); // Creating list of vertexes with markers (length)
            foreach (var vert in Vertexes)
            {
                if(vert == start)
                    dv.Add(new vertWithPathLength(vert, 0)); // If start vert length = 0
                else
                    dv.Add(new vertWithPathLength(vert, Int32.MaxValue)); // Else it is set to infinity (maxint)
            }



            while ( Vertexes.Exists( x => !x.Visited ) ) // While not all vertexes were visited
            {
                List<vertWithPathLength> leftedList = dv.FindAll(x => !x.Visited); // List of not visited vertexes
                vertWithPathLength cur = leftedList.Min(); // Take vertex with minimal length
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
                if(a.Length != Int32.MaxValue)
                    retlist.Add(a.Vert, (int)a.Length);

            return retlist;
        }

        // 5

        class vertWithPLAndHeuristic : vertWithPathLength, IComparable
        {
            public double H;
            public vertWithPLAndHeuristic(Vertex vert, double pl, double h) : base(vert, pl)
            {
                H = h;
            }
            public int ConempareTo(object obj)
            {
                return Length.CompareTo((obj as vertWithPathLength).Length) + H.CompareTo((obj as vertWithPLAndHeuristic).H);
            }
        }

        public Dictionary<Vertex, double> AStar( Vertex start, Vertex target)
        {
            if (!Vertexes.Exists(x => x == start))
                throw new Exception("Start point doesn't exist");
            if (!Vertexes.Exists(x => x == target))
                throw new Exception("Target point doesn't exist");

            List<vertWithPLAndHeuristic> unvisited = new List<vertWithPLAndHeuristic>();
            Dictionary<vertWithPLAndHeuristic, vertWithPLAndHeuristic> VertWithParent = new Dictionary<vertWithPLAndHeuristic, vertWithPLAndHeuristic>();

            unvisited.Add(new vertWithPLAndHeuristic(start, 0, 0));

            while( unvisited.Count > 0)
            {
                vertWithPLAndHeuristic curVert = unvisited.Min();
                unvisited.Remove(curVert);
                curVert.Vert.Visited = true;

                List<Edge> connectedEdges = new List<Edge>(curVert.Vert.ConnectedEdges);

                while(connectedEdges.Count > 0)
                {
                    var e = connectedEdges.Min();
                    connectedEdges.Remove(e);
                    
                    var obsVert = e.ConnectedVert;

                    double Length = curVert.Length + e.Weight;
                    double h = heuristicPathLength(obsVert, target);

                    if (!obsVert.Visited)
                    {
                        vertWithPLAndHeuristic v = unvisited.Find(x => x.Vert == obsVert);
                        vertWithPLAndHeuristic newVert = new vertWithPLAndHeuristic(obsVert, Length, h);
                        if (v is null)
                        {
                            unvisited.Add(newVert);
                            VertWithParent.Add(newVert, curVert);
                        }
                        else if ( v.H > h)
                        {
                            v.H = h; v.Length = Length;
                            VertWithParent[v] = curVert;
                        }

                        if (obsVert == target)
                        {
                            Dictionary<Vertex, double> retDict = new Dictionary<Vertex, double>();
                            retDict.Add(newVert.Vert, newVert.Length);

                            vertWithPLAndHeuristic rootVert = VertWithParent[newVert];

                            var a = VertWithParent;

                            while (rootVert.Vert != start)
                            {
                                retDict.Add(rootVert.Vert, rootVert.Length);
                                rootVert = VertWithParent[rootVert];
                            }
                            retDict.Add(rootVert.Vert, rootVert.Length);

                            _clearVertexsesAfterSearch();

                            return retDict;
                        }
                    }
                }
                
            }
            _clearVertexsesAfterSearch();

            return null;
        }

        public Vertex[] RadDimFinder(out int radius, out int diameter)
        {
            List<vertWithPathLength> lenDict = new List<vertWithPathLength>();
            int tempDist = -1;
            int curMax = 0;
            int curMin = Int32.MaxValue;
            foreach (var vertex in Vertexes)
            {
                Dictionary<Vertex, int> curLenDist = DijkstrasAlgorithm(vertex);
                if (tempDist == -1)
                    tempDist = curLenDist.Count;

                if (curLenDist.Count != tempDist)
                    throw new Exception("Unsolid graph");

                int allLength = 0;
                foreach (var l in curLenDist.Values)
                    if (l > allLength)
                        allLength = l;

                if (allLength < curMin)
                    curMin = allLength;
                if (allLength > curMax)
                    curMax = allLength;

                lenDict.Add(new vertWithPathLength(vertex, allLength));
            }

            radius = curMin; diameter = curMax;

            List<vertWithPathLength> centers = lenDict.FindAll(x => x.Length == curMin);
            //List<vertWithPathLength> diams = lenDict.FindAll(x => x.Length == curMax);

            List<Vertex> retRads = new List<Vertex>();
            List<Vertex> retDiams = new List<Vertex>();

            foreach (var r in centers)
                retRads.Add(r.Vert);
            //foreach (var d in diams)
            //    retDiams.Add(d.Vert);

            Vertex[] ret = retRads.ToArray();
            //ret[0] = retRads.ToArray(); ret[1] = retDiams.ToArray();

            return ret;

        }
    }
}
