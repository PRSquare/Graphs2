using Graphs2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs2.Utils
{
    public class GraphImport : GraphExport
    {
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
                        Edge ge = new Edge(GraphUtils.GetNewEdgeName(), Int32.Parse(adj));
                        ge.RouteVert = retGraph[curArrayPosition];
                        ge.ConnectedVert = retGraph[pos];
                        ge.ConnectVertexes(); // Добавление ребра в вершину
                    }
                    ++pos;
                }
                curArrayPosition++;
            }

            Graph g = new Graph(new List<Vertex>(retGraph));
            GraphUtils.SetAutoVertexesPositions(g);

            return g;
        }


        // Метод сканировани Edges
        private static List<Vertex> scanEdges(List<Vertex> vertList, string EVList)
        {
            List<Vertex> retVerts = vertList;

            string lastWord = "";
            string curWord = "";

            string edgeName = "Edge";
            int edgeWeight = 1;

            bool scanning = false;
            foreach (char a in EVList)
            {
                if (scanning == true)
                {
                    if (curWord != "")
                    {
                        edgeWeight = Int32.Parse(curWord); // Считвание веса вершины
                        scanning = false;
                    }
                }
                switch (a)
                {
                    case '(':
                        // На следующем проходе запишнтся имя ребра
                        scanning = true;

                        edgeName = curWord;
                        lastWord = curWord;
                        curWord = "";
                        break;
                    case ')':
                        // Создание ребра
                        scanning = false;
                        // Create vert

                        Vertex rootVert = retVerts.Find(x => x.Name == lastWord); // Поиск уже существующей начальной вершины
                        if (rootVert == null)
                        {
                            // Создание, если не существует
                            retVerts.Add(new Vertex(lastWord));
                            rootVert = retVerts.Last();
                        }
                        Vertex conVert = retVerts.Find(x => x.Name == curWord); //  Поиск уже существующей конечной вершины
                        if (conVert == null)
                        {
                            // Создание, если не существует
                            retVerts.Add(new Vertex(curWord));
                            conVert = retVerts.Last();
                        }
                        Edge ed = new Edge(edgeName, edgeWeight); // Создание ребра
                        ed.RouteVert = rootVert;
                        ed.ConnectedVert = conVert;
                        ed.ConnectVertexes(); // Присоединение ребра к вершине


                        lastWord = curWord;
                        curWord = "";
                        break;
                    case '{':

                        lastWord = curWord;
                        curWord = "";
                        break;
                    case '}':
                        return retVerts;

                        lastWord = curWord;
                        curWord = "";
                        break;

                    case '\t':
                    case ',':
                        break;
                    case ' ':
                    case '\n':
                        lastWord = curWord;
                        curWord = "";
                        break;
                    default:
                        // Запись символа к последнему слову
                        curWord += a;
                        break;
                }
            }
            return retVerts;
        }

        // Класс координат вершины
        private class Cords
        {
            private double x;
            private double y;
            private short state = 0;
            public void addValue(double value) // Метод присваивает значение не инициализированной координате
            {
                switch (state)
                {
                    case 0:
                        x = value;
                        ++state;
                        return;
                    case 1:
                        y = value;
                        ++state;
                        return;
                    default:
                        throw new Exception("Both vars are already set!");
                }
            }
            public double X() { return x; }
            public double Y() { return y; }

        }

        // Метод сканирования Vertex
        private static Vertex scanVert(string EVList)
        {
            Vertex retVert = null;
            string lastWord = "";
            string curWord = "";

            string vertName = "Vertex";

            bool scanning = false;

            Cords vertCords = new Cords();

            foreach (var a in EVList)
            {
                switch (a)
                {
                    case '(':

                        scanning = true;

                        vertName = curWord; // Запись имени вершины
                        lastWord = curWord;
                        curWord = "";
                        break;
                    case ')':
                        // Создание вершины
                        scanning = false;
                        // Create vert

                        vertCords.addValue(double.Parse(curWord)); // Запись координаты

                        retVert = new Vertex(vertName);
                        retVert.X = vertCords.X();
                        retVert.Y = vertCords.Y(); // Установка координат


                        lastWord = curWord;
                        curWord = "";
                        break;
                    case '{':

                        lastWord = curWord;
                        curWord = "";
                        break;
                    case '}':
                        return retVert; // Возвращение вершины

                    case ',':
                        lastWord = curWord;
                        vertCords.addValue(double.Parse(curWord)); // Запись координаты
                        curWord = "";
                        break;
                    case '\t':
                    case ' ':
                    case '\n':
                        break;
                    default:
                        curWord += a;
                        break;
                }
            }
            return retVert;
        }

        // Метод создания графа из списка рёбер/вершин
        public static Graph CreateGraphGromEdgVertList(string EVList)
        {
            List<Vertex> retVerts = new List<Vertex>();

            short cirBrState = 0;
            short figBrState = 0;
            char lastSymbol = '\0';
            string lastWord = "";
            string curWord = "";

            bool isComment = false;

            for (int i = 0; i < EVList.Length; ++i)
            {
                if (isComment)
                {
                    // Если комментарий - пропускаем
                    if (EVList[i] != '\n')
                        continue;
                    isComment = false;

                }
                switch (EVList[i])
                {
                    case '{':
                        figBrState++;

                        lastWord = curWord;
                        if (lastWord == "Vertex") // Если Vertex начинаем считывать 
                            retVerts.Add(scanVert(EVList.Substring(i)));
                        if (lastWord == "Edges") // Если Edges начинаем считывать
                            retVerts = scanEdges(retVerts, EVList.Substring(i));
                        curWord = "";
                        break;
                    case '}':
                        figBrState--;

                        lastWord = curWord;
                        curWord = "";
                        break;

                    case '\t':
                    case ',':
                    case ' ':
                    case '\n':
                        break;
                    case '%':
                        isComment = true;
                        break;
                    default:
                        curWord += EVList[i];
                        break;
                }

                lastSymbol = EVList[i];
            }
            return new Graph(retVerts);
        }
    }
}
