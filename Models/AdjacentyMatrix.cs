using Graphs2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs2.Models
{
    class AdjacentyMatrix
    {
        private Matrix<Edge> Mat;
        public bool IsInaccurate = false;
        Graph _graph;

        public AdjacentyMatrix(Graph graph)
        {
            _graph = graph;
            int size = graph.Vertexes.Count;
            Mat = new Matrix<Edge>(size, size);
            Vertex[] vertexes = graph.Vertexes.ToArray();
            for (int i = 0; i < size; ++i)
            {
                for(int j = 0; j < size; ++j)
                {
                    List<Edge> connected = vertexes[i].ConnectedEdges.FindAll(x => x.ConnectedVert == vertexes[j]);
                    if (connected?.Count > 1)
                        IsInaccurate = true;

                    if (connected?.Count != 0)
                        Mat.Values[i][j] = connected.First();
                    else
                        Mat.Values[i][j] = null;
                }
            }
        }

        public Edge[] GetRow(int i) => Mat.GetRow(i);
        public string GetRowAsString(int i) => Mat.GetRowAsString(i);
        public string GetRowAsStringWithoutWeighs( int i)
        {
            string buff = Mat.GetRowAsString(i);
            string[] str = buff.Split(' ');
            for (int j = 0; j < str.Length; ++j)
                if (str[j] != "0")
                    str[j] = "1";
            return String.Join(" ", str);
        }


        public void SetValue(int weight, int i, int j)
        {
            if (i >= Mat.Width || j >= Mat.Height || i < 0 || j < 0)
                throw new IndexOutOfRangeException();

            if(weight != 0)
            {
                if( !(Mat.Values[i][j] is null))
                {
                    Mat.Values[i][j].Weight = weight;
                }
                else
                {
                    Edge newEdge = new Edge(GraphUtils.GetNewEdgeName(), weight);
                    Vertex[] vertArray = _graph.Vertexes.ToArray();
                    newEdge.RouteVert = vertArray[i];
                    newEdge.ConnectedVert = vertArray[j];
                    if (_graph.AddEdge(newEdge))
                        newEdge.ConnectVertexes();
                }
            }
            else
            {
                Vertex[] vertArray = _graph.Vertexes.ToArray();
                Edge edge = vertArray[i].ConnectedEdges[0];
                _graph.RemoveEdge(edge);
            }
        }

        public Edge At(int i, int j) => Mat.Values[i][j];
        public int Size => Mat.Width;

        public override string ToString()
        {
            return Mat.ToString();
        }
    }
}
