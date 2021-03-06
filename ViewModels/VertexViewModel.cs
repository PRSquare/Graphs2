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
    public class VertexViewModel : BaseObject
    {
        public Vertex _vert;

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                _vert.Name = Name;
                OnPropertyChanged(nameof(Name));
            }
        }

        private double _x;
        public double X
        {
            get => _x;
            set
            {
                _x = value;
                _vert.X = X;
                OnPropertyChanged(nameof(X));
            }
        }

        private double _y;
        public double Y
        {
            get => _y;
            set
            {
                _y = value;
                _vert.Y = Y;
                OnPropertyChanged(nameof(Y));
            }
        }

        private PointViewModel _textPos;
        public PointViewModel TextPos { get => _textPos; set { _textPos = value; OnPropertyChanged(nameof(TextPos)); } }

        private Action<VertexViewModel> _onDelete;
        public Action<VertexViewModel> OnDelete
        {
            get => _onDelete;
            set
            {
                _onDelete = value;
                DeleteFromGraph = new DeleteVertexActionOnCommand(OnDelete, this);
            }
        }

        public Action<VertexViewModel> OnSelection;

        public VertexViewModel(Vertex vert)
        {
            _vert = vert;
            Name = _vert.Name;
            X = _vert.X;
            Y = _vert.Y;
            TextPos = new PointViewModel(X, Y + 10);
            ObjectColor = new SolidColorBrush(Colors.Yellow);

            OnSelection = null;

            defaultColor = new SolidColorBrush(Colors.Yellow);
            whenSelectedColor = new SolidColorBrush(Colors.Red);
        }

        public override void ChangePosition(double x, double y)
        {
            X = x;
            Y = y;

            TextPos = new PointViewModel(X, Y + 10);
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
