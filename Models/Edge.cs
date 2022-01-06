using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs2.Models
{
    public class Edge : ICloneable
    {
        public Vertex RouteVert;
        public Vertex ConnectedVert;
        public int Weight;
        public string Name;

        public bool IsDirected;

        public int EdgeNumber = 0;

        public Edge(string name = "Edge", int weight = 1) 
        {
            RouteVert = null;
            ConnectedVert = null;
            IsDirected = true;
            Name = name;
            Weight = weight;
        }

        public void ConnectVertexes() 
        {
            if (RouteVert == null || ConnectedVert == null)
                throw new Exception("not inic!!!"); // Create exception later!!!

            RouteVert.ConnectedEdges.Add(this);
        }

        public object Clone()
        {
            // ???
            Edge retEdge = new Edge(Name, Weight);
            retEdge.IsDirected = IsDirected;
            retEdge.ConnectedVert = ConnectedVert;
            return retEdge;
        }

        public override bool Equals(object obj)
        {
            Edge edge = (Edge)obj;
            return RouteVert.Equals(edge.RouteVert) && ConnectedVert.Equals(edge.ConnectedVert) &&
                Weight == edge.Weight && IsDirected == edge.IsDirected;
        }

        public static bool operator ==(Edge ed1, Edge ed2) 
        {
            return ed1.Equals(ed2);   
        }

        public static bool operator !=(Edge ed1, Edge ed2) 
        {
            return !ed1.Equals(ed2);
        }


        /// <summary>
        /// Auto generated GetHashCode() method
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            int hashCode = -1371070478;
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(RouteVert);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(ConnectedVert);
            hashCode = hashCode * -1521134295 + Weight.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + IsDirected.GetHashCode();
            return hashCode;
        }
    }
}
