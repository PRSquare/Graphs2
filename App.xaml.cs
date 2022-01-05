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

        protected override void OnStartup(StartupEventArgs e)
        {
            List<Vertex> vertList = new List<Vertex>();
            List<Edge> edgeList = new List<Edge>();

            Vertex vert1 = new Vertex("vert1");
            Vertex vert2 = new Vertex("vert2");
            Vertex vert3 = new Vertex("vert3");

            Edge ed1 = new Edge();
            Edge ed2 = new Edge();
            Edge ed3 = new Edge();

            ed1.RouteVert = vert1; ed1.ConnectedVert = vert2;
            ed2.RouteVert = vert2; ed2.ConnectedVert = vert3;
            ed3.RouteVert = vert3; ed3.ConnectedVert = vert1;

            ed1.ConnectVertexes();
            ed2.ConnectVertexes();
            ed3.ConnectVertexes();

            vertList.Add(vert1); vertList.Add(vert2); vertList.Add(vert3);
            edgeList.Add(ed1); edgeList.Add(ed2); edgeList.Add(ed3);

            MyGraph = new Graph(vertList, edgeList);

            GraphUtils.SetAutoVertexesPositions(MyGraph);

            MainWindow mainWindow = new MainWindow();

            MainViewModel MVM = new MainViewModel(MyGraph);
            MVM.GraphCanvas = mainWindow.GraphCanvas;

            mainWindow.DataContext = MVM;

            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
