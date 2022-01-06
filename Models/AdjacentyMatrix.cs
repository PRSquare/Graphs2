using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs2.Models
{
    class AdjacentyMatrix
    {
        public Matrix Mat;

        public AdjacentyMatrix(Graph graph)
        {
            uint size = (uint)graph.Vertexes.Count;
            Mat = new Matrix(size, size);
            Vertex[] vertexes = graph.Vertexes.ToArray();
            for (int i = 0; i < size; ++i)
            {
                for(int j = 0; j < size; ++j)
                {
                    Edge connected = vertexes[i].ConnectedEdges.Find(x => x.ConnectedVert == vertexes[j]);
                    if (connected != null)
                        Mat.Values[i][j] = connected.Weight;
                    else
                        Mat.Values[i][j] = 0;
                }
            }
        }
    }
}
