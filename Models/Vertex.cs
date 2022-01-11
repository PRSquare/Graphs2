using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs2.Models
{
    /// <summary>
    /// Класс вершины графа
    /// </summary>
    public class Vertex : ICloneable
    {
        private double _x;
        public double X 
        { 
            get => _x; 
            set{ _x = value; } 
        }

        private double _y;
        public double Y { get => _y; set => _y = value; }

        public string Name;

        public bool Visited = false;

        public List<Edge> ConnectedEdges;

        public Vertex(string name)
        {
            X = 0; Y = 0;
            ConnectedEdges = new List<Edge>();
            Name = name;
        }

        public Vertex(Vertex vert)
        {
            X = vert.X; Y = vert.Y;
            Name = vert.Name;
            ConnectedEdges = vert.ConnectedEdges;
            Visited = vert.Visited;
        }

        public object Clone()
        {
            Vertex retVert = new Vertex(Name);
            retVert.X = X;
            retVert.Y = Y;

            foreach( var ed in ConnectedEdges)
            {
                Edge newEdge = new Edge(ed.Name, ed.Weight);
                newEdge.RouteVert = this;
                newEdge.RouteVert = ed.ConnectedVert;
                retVert.AddConnectedEdge(newEdge);
            }

            // ConnectedEdges

            return retVert;
        }

        public object CloneWithoutEdges()
        {
            Vertex retVert = new Vertex(Name);
            retVert.X = X;
            retVert.Y = Y;

            return retVert;
        }


        /// <summary>
        /// Adds new connection
        /// </summary>
        /// <param name="edge">Edge to add</param>
        /// <returns>If edge already exists returns existing edge, else returns added edge</returns>
        public Edge AddConnectedEdge( Edge edge)
        {
            Edge existing = ConnectedEdges.Find(x => x == edge);
            if(existing == null)
            {
                ConnectedEdges.Add(edge);
                return edge;
            }
            return existing;
        }

        /// <summary>
        /// Removes edge from connected edges
        /// </summary>
        /// <param name="edge">Edge to remove</param>
        /// <returns>True if edge was removed, Else if not (edge doesn't exist)</returns>
        public bool RemoveConnectedEdge( Edge edge)
        {
            return ConnectedEdges.Remove(edge);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return this is null;
            Vertex vertex = (Vertex)obj;
            return Name == vertex.Name && ConnectedEdges == vertex.ConnectedEdges;
        }

        public static bool operator ==(Vertex vert1, Vertex vert2)
        {
            if (vert1 is null)
                return vert2 is null;
            return vert1.Equals(vert2);
        }
        public static bool operator !=(Vertex vert1, Vertex vert2)
        {
            return !vert1.Equals(vert2);
        }

        /// <summary>
        /// Auto generated GetHashCode() method
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            int hashCode = -1706486309;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Edge>>.Default.GetHashCode(ConnectedEdges);
            return hashCode;
        }
    }
}
