using Graphs2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Graphs2.ViewModels
{
    public class EdgeViewModel : BaseViewModel
    {
        Edge _edge;

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private PointViewModel _startPos;
        public PointViewModel StartPos { get => _startPos; set { _startPos = value; OnPropertyChanged(nameof(StartPos)); } }

        private PointViewModel _endPos;
        public PointViewModel EndPos { get => _endPos; set { _endPos = value; OnPropertyChanged(nameof(EndPos)); } }

        private PointViewModel _midPos;
        public PointViewModel MidPos { get => _midPos; set { _midPos = value;  OnPropertyChanged(nameof(MidPos)); } }

        private SolidColorBrush _edgeColor;

        private bool _isDirected;
        public bool IsDirected { get => _isDirected; set { _isDirected = value; OnPropertyChanged(nameof(IsDirected)); } }


        public SolidColorBrush EdgeColor
        {
            get => _edgeColor;
            set
            {
                _edgeColor = value;
                OnPropertyChanged(nameof(EdgeColor));
            }
        }

        public EdgeViewModel(Edge edge)
        {
            _edge = edge;
            Name = _edge.Name;
            IsDirected = _edge.IsDirected;

            StartPos = new PointViewModel();
            EndPos = new PointViewModel();
            MidPos = new PointViewModel();

            UpdateCords();

            EdgeColor = new SolidColorBrush(Colors.Black);
        }

        public void UpdateCords() 
        {
            StartPos.X = _edge.RouteVert.X + 5;
            StartPos.Y = _edge.RouteVert.Y + 5;

            EndPos.X = _edge.ConnectedVert.X + 5;
            EndPos.Y = _edge.ConnectedVert.Y + 5;

            MidPos.X = (StartPos.X + EndPos.X) / 2.0;
            MidPos.Y = (StartPos.Y + EndPos.Y) / 2.0;
        }
    }
}
