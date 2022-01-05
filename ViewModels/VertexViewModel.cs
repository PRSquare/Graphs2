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

        public VertexViewModel(Vertex vert)
        {

            _vert = vert;
            Name = _vert.Name;
            X = _vert.X;
            Y = _vert.Y;
            ObjectColor = new SolidColorBrush(Colors.Yellow);

            defaultColor = new SolidColorBrush(Colors.Yellow);
            whenSelectedColor = new SolidColorBrush(Colors.Red);
        }

        public override void ChangePosition(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
