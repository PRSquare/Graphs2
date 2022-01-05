using Graphs2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs2.Utils
{
    public class GraphUtils
    {
        public static void SetAutoVertexesPositions( Graph graph ) 
        {
            int count = graph.Vertexes.Count;
            double offset = (3.14 * 2) / count;
            for ( int i = 0; i < count; ++i)
            {
                graph.Vertexes[i].X = 200 + Math.Cos(offset * i) * 100;
                graph.Vertexes[i].Y = 200 + Math.Sin(offset * i) * 100;
            }
        }
    }
}
