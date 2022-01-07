using Graphs2.Models;
using Graphs2.Utils;
using Graphs2.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Graphs2
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Graph MyGraph;
        public GraphViewModel GraphVM;

        public App() 
        {
            
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            List<Vertex> vertList = new List<Vertex>();
            //List<Edge> edgeList = new List<Edge>();

            Vertex vert1 = new Vertex("vert1");
            Vertex vert2 = new Vertex("vert2");
            Vertex vert3 = new Vertex("vert3");

            Edge ed1 = new Edge("e", 5);
            Edge ed2 = new Edge("w", 2);
            Edge ed3 = new Edge();
            Edge edCircle = new Edge();
            Edge sameEdge = new Edge();

            ed1.RouteVert = vert1; ed1.ConnectedVert = vert2;
            ed2.RouteVert = vert2; ed2.ConnectedVert = vert3;
            ed3.RouteVert = vert3; ed3.ConnectedVert = vert1;
            edCircle.RouteVert = vert1; edCircle.ConnectedVert = vert1;

            sameEdge.RouteVert = vert2; sameEdge.ConnectedVert = vert1;

            ed1.ConnectVertexes();
            ed2.ConnectVertexes();
            ed3.ConnectVertexes();
            edCircle.ConnectVertexes();

            sameEdge.ConnectVertexes();

            vertList.Add(vert1); vertList.Add(vert2); vertList.Add(vert3); 
            //edgeList.Add(ed1); edgeList.Add(ed2); edgeList.Add(ed3); edgeList.Add(edCircle);

            MyGraph = new Graph(vertList);

            AdjacentyMatrix mymat = new AdjacentyMatrix(MyGraph);

            GraphUtils.SetAutoVertexesPositions(MyGraph);

            Dictionary<Vertex, int> test1 = MyGraph.DijkstrasAlgorithm(vert1);

            string buffer = "";
            foreach ( var d in test1)
                buffer += d.Key.Name + " " + d.Value.ToString() + "\n";

            //MessageBox.Show($"{buffer}");

            MainWindow mainWindow = new MainWindow();

            MainViewModel MVM = new MainViewModel(MyGraph, mymat);
            MVM.GraphCanvas = mainWindow.GraphCanvas;

            mainWindow.DataContext = MVM;

            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
