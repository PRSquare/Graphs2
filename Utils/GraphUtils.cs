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
        private static uint _vert_name_numiration = 0;
        private static uint _edge_name_numiration = 0;

        public static void SetAutoVertexesPositions(Graph graph)
        {
            int count = graph.Vertexes.Count;
            double offset = (3.14 * 2) / count;
            for (int i = 0; i < count; ++i)
            {
                graph.Vertexes[i].X = 200 + Math.Cos(offset * i) * 100;
                graph.Vertexes[i].Y = 200 + Math.Sin(offset * i) * 100;
            }
        }

        public static string GetNewEdgeName()
        {
            string newName = "Edge_" + _edge_name_numiration.ToString(); ;
            _edge_name_numiration++;
            return newName;
        }

        public static string GetNewVerteName()
        {
            string newName = "Vertex_" + _vert_name_numiration.ToString(); ;
            _vert_name_numiration++;
            return newName;
        }

        public static void ResetAutoNamerNumeration()
        {
            _edge_name_numiration = 0;
            _vert_name_numiration = 0;
        }

        public static Graph ImportFromAdjacentyMatrix(string adjMat)
        {
            Vertex[] retGraph; // Переменная для записи графа
            Queue<String> lines = new Queue<String>(adjMat.Split('\n')); // Строки

            String line = lines.Dequeue(); // Убирание из очереди первой строки (строка с именами вершин)
            Queue<String> names = new Queue<String>(line.Split(' ')); // Список имён вершин
            names.Dequeue(); // Уничтожение первого (0)
            retGraph = new Vertex[names.Count]; // Установка кол-ва вершин в графе
            int curArrayPosition = 0; // Текущая позиция в массиве
            foreach (var name in names)
            {
                // Инициализация всех вершин
                retGraph[curArrayPosition] = new Vertex(name);
                curArrayPosition++;
            }

            curArrayPosition = 0;
            foreach (var l in lines)
            {
                // Построчное считывание строк матрицы
                Queue<String> adjs = new Queue<String>(l.Split(' '));
                string vertName = adjs.Dequeue(); // Имя вершины вначале строки
                int pos = 0;
                foreach (var adj in adjs)
                {
                    if (adj != "0")
                    {
                        // Если есть связь, то создаётся ребро
                        Edge ge = new Edge(GetNewEdgeName(), Int32.Parse(adj));
                        ge.RouteVert = retGraph[curArrayPosition];
                        ge.ConnectedVert = retGraph[pos];
                        ge.ConnectVertexes(); // Добавление ребра в вершину
                    }
                    ++pos;
                }
                curArrayPosition++;
            }

            Graph g = new Graph(new List<Vertex>(retGraph));
            SetAutoVertexesPositions(g);

            return g;
        }
    }
}
