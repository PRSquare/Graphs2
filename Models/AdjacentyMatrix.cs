using Graphs2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs2.Models
{
    public class AdjacentyMatrix
    {
        private Matrix<Edge> Mat;
        public Vertex[] Verts;
        public bool IsInaccurate = false;
        public readonly Graph _graph;

        public AdjacentyMatrix(Graph graph)
        {
            _graph = graph;
            int size = graph.Vertexes.Count;

            Mat = new Matrix<Edge>(size, size);
            Vertex[] vertexes = graph.Vertexes.ToArray();
            Verts = vertexes;
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
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

        public AdjacentyMatrix(Matrix<Edge> mat)
        {
            if (mat.Width != mat.Height)
                throw new Exception("Incorrect matrix");
            Mat = mat;
        }

        public Edge[] GetRow(int i) => Mat.GetRow(i);
        public string GetRowAsString(int i) => Mat.GetRowAsString(i);
        public string GetRowAsStringWithoutWeighs(int i)
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

            if (weight != 0)
            {
                if (!(Mat.Values[i][j] is null))
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
                    Mat.Values[i][j] = newEdge;
                }
            }
            else
            {
                Vertex[] vertArray = _graph.Vertexes.ToArray();
                if (vertArray[i].ConnectedEdges.Count > 0)
                {
                    Edge edge = vertArray[i].ConnectedEdges[0];
                    _graph.RemoveEdge(edge);
                }
                Mat.Values[i][j] = null;
            }
        }

        public Edge At(int i, int j) => Mat.Values[i][j];
        public int Size => Mat.Width;

        public override string ToString()
        {
            string[] buff = new string[Size + 1];

            //buff[0] = 
            string nameBuff = "0";
            Edge[] firstRow = Mat.GetRow(0);
            for (int i = 0; i < firstRow.Length; ++i)
                nameBuff += $" {Verts[i].Name}";
            nameBuff += "\n";
            for (int i = 0; i < Size; ++i)
            {
                nameBuff += $"{Verts[i].Name} ";
                string valsbuffstr = Mat.GetRowAsString(i);
                valsbuffstr = valsbuffstr.Substring(0, valsbuffstr.Length - 1);
                nameBuff += valsbuffstr;
                nameBuff += "\n";
            }


            return nameBuff;
        }

        public static AdjacentyMatrix And(AdjacentyMatrix mat1, AdjacentyMatrix mat2)
        {
            if (mat1.Size != mat2.Size)
                throw new Exception($"Uncomparable matrixes!\nFirst matrix {mat1.Size}x{mat1.Size}. SecondMatrix{mat2.Size}x{mat2.Size}");

            int Size = mat1.Size;

            AdjacentyMatrix retMat = new AdjacentyMatrix(Graph.CreateConnected(mat1._graph));

            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    Edge val1 = mat1.At(i, j);
                    Edge val2 = mat2.At(i, j);
                    retMat.SetValue((val1 is null == val2 is null) ? 1 : 0, i, j);
                }
            }

            return retMat;
        }

        public static AdjacentyMatrix Not(AdjacentyMatrix mat)
        {
            int Size = mat.Size;
            List<Vertex> vertList = new List<Vertex>();
            foreach (var vert in mat.Verts)
                vertList.Add(new Vertex(vert.Name));

            AdjacentyMatrix retMat = new AdjacentyMatrix(new Graph(vertList));

            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    Edge val = mat.At(i, j);
                    retMat.SetValue(val is null ? 1 : 0, i, j);
                    var a = retMat;
                }
            }
            return retMat;
        }

        public bool IsNull()
        {
            for (int i = 0; i < Size; ++i)
                for (int j = 0; j < Size; ++j)
                    if (!(At(i, j) is null))
                        return false;
            return true;
        }
    }
}
