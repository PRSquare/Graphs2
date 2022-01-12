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
        public Edge _edge;

        public string Name
        {
            get => _edge.Name;
            set
            {
                _edge.Name = value;
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

        private PointViewModel _arrowPos1;
        public PointViewModel ArrowPos1 { get => _arrowPos1; set { _arrowPos1 = value; OnPropertyChanged(nameof(ArrowPos1)); } }

        private PointViewModel _arrowPos2;
        public PointViewModel ArrowPos2 { get => _arrowPos2; set { _arrowPos2 = value; OnPropertyChanged(nameof(ArrowPos2)); } }


        public PointViewModel MidPosOffset;

        public Action<EdgeViewModel> OnSelection;

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

        public bool IsDirected { 
            get => _isDirected; 
            set { _isDirected = value; _edge.IsDirected = IsDirected; OnPropertyChanged(nameof(IsDirected)); } 
        }

        public int Weight
        {
            get => _edge.Weight;
            set
            {
                _edge.Weight = value;
                OnPropertyChanged(nameof(Weight));
            } 
        }

        public EdgeViewModel(Edge edge)
        {
            _edge = edge;
            Name = _edge.Name;
            Weight = _edge.Weight;
            IsDirected = _edge.IsDirected;

            StartPos = new PointViewModel();
            EndPos = new PointViewModel();
            MidPos = new PointViewModel();
            MidPosOffset = new PointViewModel();
            TextPos = new PointViewModel();
            ArrowPos1 = new PointViewModel();
            ArrowPos2 = new PointViewModel();

            UpdateCords();
            
            OnSelection = null;

            defaultColor = new SolidColorBrush(Colors.Black);
            whenSelectedColor = new SolidColorBrush(Colors.Red);
        }

        public void UpdateCords()
        {
            StartPos.X = _edge.RouteVert.X + 5;
            StartPos.Y = _edge.RouteVert.Y + 5;

            EndPos.X = _edge.ConnectedVert.X + 5;
            EndPos.Y = _edge.ConnectedVert.Y + 5;
            
            if(_edge.RouteVert == _edge.ConnectedVert)
            {
                StartPos.X -= 5;

                EndPos.X += 5;

                MidPos.X = (StartPos.X + EndPos.X) / 2.0 + MidPosOffset.X;
                MidPos.Y = StartPos.Y + 20 + MidPosOffset.Y + 10*_edge.EdgeNumber;
            }
            else
            {
                MidPos.X = (StartPos.X + EndPos.X) / 2.0 + MidPosOffset.X + 10 * _edge.EdgeNumber;
                MidPos.Y = (StartPos.Y + EndPos.Y) / 2.0 + MidPosOffset.Y + 10 * _edge.EdgeNumber;
            }
            // Central point of Bezier curve
            TextPos.X = 0.25 * StartPos.X + 0.5 * MidPos.X + 0.25 * EndPos.X;
            TextPos.Y = 0.25 * StartPos.Y + 0.5 * MidPos.Y + 0.25 * EndPos.Y;

            double xoffset = 0.9; double yoffset = 0.9;

            ArrowPos1.X = (1 - xoffset) * (1 - xoffset) * StartPos.X + (1 - xoffset) * xoffset * 2 * MidPos.X + xoffset * xoffset * EndPos.X - 5;
            ArrowPos1.Y = (1 - yoffset) * (1 - yoffset) * StartPos.Y + (1 - yoffset) * yoffset * 2 * MidPos.Y + yoffset * yoffset * EndPos.Y - 5;

            ArrowPos2.X = 0.81 * StartPos.X + 0.81 * 2 * MidPos.X + 0.81 * EndPos.X;
            ArrowPos2.Y = 0.81 * StartPos.Y + 0.81 * 2 * MidPos.Y + 0.81 * EndPos.Y;
        }

        public override void ChangePosition(double x, double y)
        {
            // Calculating point to positionate Bezier curve's mid point in coordinates 
            MidPosOffset.X = (2 * x - 0.5 * (StartPos.X + EndPos.X)) - (StartPos.X + EndPos.X) / 2.0;
            MidPosOffset.Y = (2 * y - 0.5 * (StartPos.Y + EndPos.Y)) - (StartPos.Y + EndPos.Y) / 2.0;
        }

        public override void EnableSelection()
        {
            if (_isSelected == false)
                _isSelected = true;
            if (!(OnSelection is null))
                OnSelection.Invoke(this);

            ContextMenuText = whenSelectedText;
            ObjectColor = whenSelectedColor;
        }
    }
}
