using Graphs2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs2.Utils
{
    public class GraphAlgorythms
    {
        // ================================================= Labs ================================================================

        // ==== 2 ====

        /// <summary>
        /// Breadth Fist Search
        /// </summary>
        /// <param name="start">Start vertex</param>
        /// <param name="target">Vertex to find</param>
        /// <returns>Count of edges to vert or -1 if there is no path</returns>
        public static List<Vertex> BreadthFristSearch(Graph graph, Vertex start, Vertex target)
        {
            if (!graph.Vertexes.Exists(x => x == start))
                throw new Exception("Start point doesn't exist");
            if (!graph.Vertexes.Exists(x => x == target))
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
                            graph._clearVertexsesAfterSearch(); // Clearing vertexes visited state

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

            graph._clearVertexsesAfterSearch(); // Clearing vertexes visited state
            return null; // If vertex was not found returns -1
        }


        // ==== 3 ====

        private static Vertex heuristicConnectionsCount(List<Vertex> verts) // Heuristic function to find more preferable node
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


        public static List<Vertex> BestFirstSearch(Graph graph, Vertex start, Vertex target)
        {
            if (!graph.Vertexes.Exists(x => x == start))
                throw new Exception("Start point doesn't exist");
            if (!graph.Vertexes.Exists(x => x == target))
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
                while (curVerts.Count > 0)
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
                            graph._clearVertexsesAfterSearch(); // Clearing vertexes visited state

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

            graph._clearVertexsesAfterSearch();

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



        public static Dictionary<Vertex, int> DijkstrasAlgorithm(Graph graph, Vertex start)
        {
            if (!graph.Vertexes.Exists(x => x == start))
                throw new Exception("Start point doesn't exist");

            foreach (var edge in graph.Edges)
                if (edge.Weight < 0)
                    throw new Exception($"{edge.Name} has negative weight");

            List<vertWithPathLength> dv = new List<vertWithPathLength>(); // Creating list of vertexes with markers (length)
            foreach (var vert in graph.Vertexes)
            {
                if (vert == start)
                    dv.Add(new vertWithPathLength(vert, 0)); // If start vert length = 0
                else
                    dv.Add(new vertWithPathLength(vert, Int32.MaxValue)); // Else it is set to infinity (maxint)
            }



            while (graph.Vertexes.Exists(x => !x.Visited)) // While not all vertexes were visited
            {
                List<vertWithPathLength> leftedList = dv.FindAll(x => !x.Visited); // List of not visited vertexes
                vertWithPathLength cur = leftedList.Min(); // Take vertex with minimal length
                cur.Vert.Visited = true; // It is visited now
                foreach (Edge ed in cur.Vert.ConnectedEdges) // Finding path length to all connected vertexes 
                {
                    Vertex analyseVert = ed.ConnectedVert;

                    if (analyseVert.Visited == true) // Do not check visited vertexes
                        continue;

                    var v = dv.Find(x => x.Vert == analyseVert); // Take vertex from list of vertexes with markers (List<dijksrtasVert> dv)
                    if (v.Length > ed.Weight + cur.Length) // If there is shorter path, rewriting length
                        v.Length = ed.Weight + cur.Length;
                }
            }


            graph._clearVertexsesAfterSearch(); // Clearing vertexes visited state

            // Translating from List<dijksrtasVert> to Dictionary<Vertex, int>

            Dictionary<Vertex, int> retlist = new Dictionary<Vertex, int>();
            foreach (var a in dv)
                if (a.Length != Int32.MaxValue)
                    retlist.Add(a.Vert, (int)a.Length);

            return retlist;
        }

        // ===== 5 ======

        private static double heuristicPathLength(Vertex vert1, Vertex vert2) =>
            Math.Sqrt((vert1.X - vert2.X) * (vert1.X - vert2.X) + (vert1.Y - vert2.Y) * (vert1.Y - vert2.Y));

        class vertWithPLAndHeuristic : vertWithPathLength, IComparable
        {
            public double H;
            public vertWithPLAndHeuristic(Vertex vert, double pl, double h) : base(vert, pl)
            {
                H = h;
            }
            public int ConempareTo(object obj)
            {
                return H.CompareTo((obj as vertWithPLAndHeuristic).H) + Length.CompareTo((obj as vertWithPLAndHeuristic).Length);
            }
        }

        public static Dictionary<Vertex, double> AStar(Graph graph, Vertex start, Vertex target)
        {
            if (!graph.Vertexes.Exists(x => x == start))
                throw new Exception("Start point doesn't exist");
            if (!graph.Vertexes.Exists(x => x == target))
                throw new Exception("Target point doesn't exist");

            List<vertWithPLAndHeuristic> unvisited = new List<vertWithPLAndHeuristic>();
            Dictionary<vertWithPLAndHeuristic, vertWithPLAndHeuristic> VertWithParent = new Dictionary<vertWithPLAndHeuristic, vertWithPLAndHeuristic>();

            unvisited.Add(new vertWithPLAndHeuristic(start, 0, 0));

            while (unvisited.Count > 0)
            {
                vertWithPLAndHeuristic curVert = unvisited.Min();
                unvisited.Remove(curVert);
                curVert.Vert.Visited = true;

                List<Edge> connectedEdges = new List<Edge>(curVert.Vert.ConnectedEdges);

                while (connectedEdges.Count > 0)
                {
                    var e = connectedEdges.Min(); // Picking edge with the less weight
                    connectedEdges.Remove(e);

                    Vertex obsVert = e.ConnectedVert; // Taking vertex on the end of this edge

                    double Length = curVert.Length + e.Weight; 
                    double h = heuristicPathLength(obsVert, target);

                    if (!obsVert.Visited)
                    {
                        vertWithPLAndHeuristic v = unvisited.Find(x => x.Vert == obsVert);
                        vertWithPLAndHeuristic newVert = new vertWithPLAndHeuristic(obsVert, Length, h);
                        vertWithPLAndHeuristic existingVert = v is null ? newVert : v;
                        if (v is null)
                        {
                            unvisited.Add(existingVert);
                            VertWithParent.Add(existingVert, curVert);
                        }
                        if (newVert.CompareTo(existingVert) < 0)
                        {
                            existingVert.H = h; existingVert.Length = Length;
                            VertWithParent[existingVert] = curVert;
                        }

                        if (obsVert == target)
                        {
                            Dictionary<Vertex, double> retDict = new Dictionary<Vertex, double>();
                            retDict.Add(obsVert, existingVert.H);
                            vertWithPLAndHeuristic rootVert = VertWithParent[existingVert];
                            var a = VertWithParent;
                            while (rootVert.Vert != start)
                            {
                                retDict.Add(rootVert.Vert, rootVert.H);
                                rootVert = VertWithParent[rootVert];
                            }
                            retDict.Add(rootVert.Vert, rootVert.H);

                            graph._clearVertexsesAfterSearch();

                            return retDict;
                        }
                    }
                }

            }

            graph._clearVertexsesAfterSearch();

            return null;
            //return null;
        }

        public static Vertex[] RadDimFinder(Graph graph, out int radius, out int diameter)
        {
            List<vertWithPathLength> lenDict = new List<vertWithPathLength>();
            int tempDist = -1;
            int curMax = 0;
            int curMin = Int32.MaxValue;
            foreach (var vertex in graph.Vertexes)
            {
                Dictionary<Vertex, int> curLenDist = DijkstrasAlgorithm(graph, vertex);
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

        public static bool IsIsomorphWith(Graph graph1, Graph graph2)
        {
            AdjacentyMatrix first = new AdjacentyMatrix(graph1);
            AdjacentyMatrix second = new AdjacentyMatrix(graph2);

            if (first.Size != second.Size)
                return false;

            int size = first.Size;

            List<string> firstStrings = new List<string>();
            List<string> secondStrings = new List<string>();


            for (int i = 0; i < size; ++i)
            {
                firstStrings.Add(first.GetRowAsStringWithoutWeighs(i));
                secondStrings.Add(second.GetRowAsStringWithoutWeighs(i));
            }

            firstStrings.Sort(); secondStrings.Sort();
            bool iso = true;
            for (int i = 0; i < size; ++i)
            {
                iso = iso && firstStrings[i] == secondStrings[i];
                if (!iso)
                    break;
            }


            return iso;
        }

        private static bool _isCon(Graph graph, List<Vertex> Vs)
        {
            int distCount = -1;
            foreach (var vertex in Vs)
            {
                Dictionary<Vertex, int> curLenDist = DijkstrasAlgorithm(graph, vertex);
                if (distCount == -1)
                    distCount = curLenDist.Count;

                if (curLenDist.Count != distCount)
                    return false;
            }
            return true;
        }

        private static List<Vertex> _dfsWithoutCleaning(Vertex vert)
        {
            List<Vertex> retList = new List<Vertex>();
            retList.Add(vert);
            vert.Visited = true;
            foreach (var ed in vert.ConnectedEdges)
            {
                if (!ed.ConnectedVert.Visited)
                    retList.AddRange(_dfsWithoutCleaning(ed.ConnectedVert));
            }
            return retList;
        }

        private static List<List<Vertex>> _componentCount(Graph graph)
        {
            List<List<Vertex>> comps = new List<List<Vertex>>();
            foreach (var vert in graph.Vertexes)
            {
                if (!vert.Visited)
                    comps.Add(_dfsWithoutCleaning(vert));
            }
            graph._clearVertexsesAfterSearch();
            return comps;
        }

        private static List<List<Vertex>> _stronglyConnectedFind(Graph graph)
        {
            List<Vertex> compTestVerts = new List<Vertex>();

            AdjacentyMatrix test = new AdjacentyMatrix(graph);

            test.ToString();

            foreach ( var vert1 in graph.Vertexes)
            {
                foreach (var vert2 in graph.Vertexes)
                {
                    if(!(BreadthFristSearch(graph, vert1, vert2) is null) && !(BreadthFristSearch(graph, vert2, vert1) is null))
                    {
                        Vertex curVert1 = compTestVerts.Find(x => x.Name == vert1.Name);
                        if (curVert1 is null)
                        {
                            Vertex newVert1 = vert1.CloneWithoutEdges() as Vertex;
                            curVert1 = newVert1;
                            compTestVerts.Add(newVert1);
                        }

                        Vertex curVert2 = compTestVerts.Find(x => x.Name == vert2.Name);
                        if (curVert2 is null)
                        {
                            Vertex newVert2 = vert2.CloneWithoutEdges() as Vertex;
                            curVert2 = newVert2;
                            compTestVerts.Add(newVert2);
                        }

                        Edge ed1 = new Edge(); Edge ed2 = new Edge();
                        ed1.IsDirected = false; ed2.IsDirected = false;
                        ed1.RouteVert = curVert1; ed1.ConnectedVert = curVert2;
                        ed2.RouteVert = curVert2; ed2.ConnectedVert = curVert1;
                        ed1.ConnectVertexes(); ed2.ConnectVertexes();
                    }
                }
            }
            Graph newG = new Graph(compTestVerts);

            return _componentCount(newG);
        }

        class vertWithNumberAndLow
        {
            public int N; 
            public int Low = -1;
            public List<vertWithNumberAndLow> Ch = new List<vertWithNumberAndLow>();
            
            public Vertex _vert;

            public bool Visited
            {
                get => _vert.Visited;
                set { _vert.Visited = value; }
            }

            public List<Edge> ConnectedEdges
            {
                get => _vert.ConnectedEdges;
            }

            public vertWithNumberAndLow(Vertex vert, int n)
            {
                _vert = vert;
                N = n;
            }

            public bool Equals(Vertex obj)
            {
                if (obj is null)
                    return this is null;
                return _vert.Name.Equals(obj.Name);
            }

            public bool Equals(vertWithNumberAndLow obj)
            {
                if (obj is null)
                    return this is null;
                return _vert.Name.Equals(obj._vert.Name);
            }

        }

        private static List<vertWithNumberAndLow> _dfsNumeration(List<vertWithNumberAndLow> vertlist, Vertex vert, out vertWithNumberAndLow addedVert)
        {
            List<vertWithNumberAndLow> retList = vertlist;
            int n = 1;
            foreach (var v in vertlist)
                if (v.N >= n)
                    n = v.N + 1;

            vertWithNumberAndLow newVert = new vertWithNumberAndLow(vert, n);
            newVert.Visited = true;
            addedVert = newVert;
            retList.Add(newVert);
            
            foreach (var ed in newVert.ConnectedEdges)
            {
                var conVert = ed.ConnectedVert;
                if (!conVert.Visited)
                {
                    vertWithNumberAndLow added;
                    //retList.AddRange(_dfsNumeration(retList, conVert, out added));
                    _dfsNumeration(retList, conVert, out added);
                    newVert.Ch.Add(added);
                }
            }
            int minLow = Int32.MaxValue;
            int minN = Int32.MaxValue;

            var rltest = retList;

            foreach (var v in newVert.Ch)
                if (v.Low < minLow)
                    minLow = v.Low;
            List<vertWithNumberAndLow> allConnected = new List<vertWithNumberAndLow>();
            foreach( var ed in newVert.ConnectedEdges)
                allConnected.Add(retList.Find(x => x.Equals(ed.ConnectedVert)));
            
            foreach (var v in allConnected)
                if (v.N < minN)
                    minN = v.N;

            newVert.Low = Math.Min(minLow, minN);

            if(n == 1 && newVert.Ch.Count < 2)
                retList.Remove(newVert);

            return retList;

        }

        private static List<Vertex> _articulationPointsSearch( Graph graph)
        {
            List<Vertex> retList = new List<Vertex>();
            Vertex unvisited = graph.Vertexes.Find(x => x.Visited == false);
            while(!(unvisited is null))
            {
                List<vertWithNumberAndLow> answerList = new List<vertWithNumberAndLow>();
                vertWithNumberAndLow firstVert;
                answerList = _dfsNumeration(answerList, unvisited, out firstVert);
                if(!(answerList is null))
                {
                    foreach (var v in answerList)
                    {
                        int targN = v.Low;
                        vertWithNumberAndLow addVert = answerList.Find(x => x.N == targN);
                        if(!(addVert is null))
                            if (!retList.Contains(addVert._vert))
                                retList.Add(addVert._vert);

                    }
                }
                unvisited = graph.Vertexes.Find(x => x.Visited == false);
            }

            graph._clearVertexsesAfterSearch();

            return retList;
        }

        private static List<Edge> _bridgesSearch( List<Vertex> articulationPoints)
        {
            List<Edge> bridges = new List<Edge>();
            foreach (var vert in articulationPoints)
            {
                foreach (var ed in vert.ConnectedEdges)
                {
                    if (ed.ConnectedVert.ConnectedEdges.Count == 0)
                    {
                        bridges.Add(ed);
                        continue;
                    }
                    if ( ed.ConnectedVert.ConnectedEdges.Count == 1 && ed.ConnectedVert.ConnectedEdges.Exists(x => x.ConnectedVert == vert))
                    {
                        bridges.Add(ed);
                        bridges.Add(ed.ConnectedVert.ConnectedEdges[0]);
                        continue;
                    }
                    if (articulationPoints.Contains(ed.ConnectedVert))
                    {
                        bridges.Add(ed);
                        continue;
                    }
                }
            }
            return bridges;
        }

        public static bool IsConnected(Graph graph, out bool IsStrongConnection, out List<List<Vertex>> compCount, out List<Vertex> articulationPoints, out List<Edge> bridges)
        {
            bool isOriented = false;
            IsStrongConnection = true;

            articulationPoints = null;
            bridges = null;

            Graph g = graph.Clone() as Graph;

            compCount = null;

            foreach (var ed in g.Edges)
            {
                if (ed.IsDirected == true)
                {
                    isOriented = true;
                    break;
                }
            }
            if (isOriented)
            {
                compCount = _stronglyConnectedFind(g);
                bool IsStrCon = _isCon(g, g.Vertexes);
                IsStrongConnection = IsStrCon;

                if (IsStrCon)
                {
                    return true;
                }
                else
                {
                    List<Vertex> verts = new List<Vertex>(g.Vertexes);

                    Graph allConnectedGraph = new Graph(verts);
                    int startEdCount = allConnectedGraph.Edges.Count;
                    for (int i = 0; i < startEdCount; ++i)
                    {
                        Edge addedEdge = allConnectedGraph.Edges[i].GetOposite();
                        addedEdge.ConnectVertexes();
                        allConnectedGraph.AddEdge(addedEdge);
                    }

                    var test = new AdjacentyMatrix(allConnectedGraph);
                    string buff = test.ToString();

                    return _isCon(g, allConnectedGraph.Vertexes);
                }
            }

            articulationPoints = _articulationPointsSearch(g);
            bridges = _bridgesSearch(articulationPoints);

            compCount = _componentCount(g);
            return _isCon(g, g.Vertexes);
        }
    }
}
