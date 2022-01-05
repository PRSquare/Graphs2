using Graphs2.Commands;
using Graphs2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Graphs2.ViewModels
{
    /// <summary>
    /// ViewModel class for edges
    /// </summary>
    public class EdgeViewModel : BaseObject
    {
        Edge _edge;

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                _edge.Name = Name;
                OnPropertyChanged(nameof(Name));
            }
        }

        private PointViewModel _startPos;
        public PointViewModel StartPos { get => _startPos; set { _startPos = value; OnPropertyChanged(nameof(StartPos)); } }

        private PointViewModel _endPos;
        public PointViewModel EndPos { get => _endPos; set { _endPos = value; OnPropertyChanged(nameof(EndPos)); } }

        private PointViewModel _midPos;
        public PointViewModel MidPos { get => _midPos; set { _midPos = value; OnPropertyChanged(nameof(MidPos)); } }

        private PointViewModel _textPos;
        public PointViewModel TextPos { get => _textPos; set { _textPos = value; OnPropertyChanged(nameof(TextPos)); } }

        public PointViewModel MidPosOffset;

        private Action<EdgeViewModel> _onDelete;
        public Action<EdgeViewModel> OnDelete 
        {
            get => _onDelete;
            set
            {
                _onDelete = value;
                DeleteFromGraph = new DeleteEdgeActionOnCommand(OnDelete, this);
            }
        }

        private bool _isDirected;

        public bool IsDirected { get => _isDirected; set { _isDirected = value; OnPropertyChanged(nameof(IsDirected)); } }


        public EdgeViewModel(Edge edge)
        {
            _edge = edge;
            Name = _edge.Name;
            IsDirected = _edge.IsDirected;

            StartPos = new PointViewModel();
            EndPos = new PointViewModel();
            MidPos = new PointViewModel();
            MidPosOffset = new PointViewModel();
            TextPos = new PointViewModel();

            UpdateCords();

            defaultColor = new SolidColorBrush(Colors.Black);
            whenSelectedColor = new SolidColorBrush(Colors.Red);
        }


        public void UpdateCords()
        {
            StartPos.X = _edge.RouteVert.X + 5;
            StartPos.Y = _edge.RouteVert.Y + 5;

            EndPos.X = _edge.ConnectedVert.X + 5;
            EndPos.Y = _edge.ConnectedVert.Y + 5;

            MidPos.X = (StartPos.X + EndPos.X) / 2.0 + MidPosOffset.X;
            MidPos.Y = (StartPos.Y + EndPos.Y) / 2.0 + MidPosOffset.Y;

            // Central point of Bezier curve
            TextPos.X = 0.25 * StartPos.X + 0.5 * MidPos.X + 0.25 * EndPos.X;
            TextPos.Y = 0.25 * StartPos.Y + 0.5 * MidPos.Y + 0.25 * EndPos.Y;
        }

        public override void ChangePosition(double x, double y)
        {
            // Calculating point to positionate Bezier curve's mid point in coordinates 
            MidPosOffset.X = (2 * x - 0.5 * (StartPos.X + EndPos.X)) - (StartPos.X + EndPos.X) / 2.0;
            MidPosOffset.Y = (2 * y - 0.5 * (StartPos.Y + EndPos.Y)) - (StartPos.Y + EndPos.Y) / 2.0;
        }
    }
}
