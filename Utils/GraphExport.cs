using Graphs2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs2.Utils
{
    public class GraphExport
    {
        public static void SaveGraphCodeFile(string path, Graph graph)
        {
            // Установка англ. формата, чтобы числа с плав. точкой сохранялись как 16.1, а не как 16,1
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            StreamWriter sw = new StreamWriter(path);
            foreach (var vert in graph.Vertexes)
            {
                // Запись всеъ вершин
                sw.WriteLine("Vertex{" + vert.Name + "(" + vert.X + ", " + vert.Y + ")}");
            }
            sw.WriteLine("Edges\n{"); // Начало записи рёбер
            bool first = true; // Первая ли запись
            foreach (var edge in graph.Edges)
            {
                if (first) // Если первая запись, то не ставим запятую в конце предыдущей строки
                    first = false;
                else
                    sw.Write(",\n");
                // Запись ребра
                sw.Write("\t" + edge.Name + "(" + edge.Weight.ToString() + ", " + edge.RouteVert.Name + ", " + edge.ConnectedVert.Name + ")");
                /*if (!edge.IsDirected)
                {
                    // Если ребро не направленое, то запись еще одного такого же, где начало и конец меняются местами   
                    sw.Write(",\n");
                    sw.Write("\t" + edge.Name + "(" + edge.Weight.ToString() + ", " + edge.ConnectedVert.Name + ", " + edge.RouteVert.Name + ")");
                }*/
            }

            sw.WriteLine("\n}");
            sw.Flush(); // Запись всего содержимого буфера в файл
            sw.Close(); // Закрытие файла
        }
    }
}
