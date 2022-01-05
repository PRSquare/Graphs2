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


        public ICommand VertexSelectionChange { get; set; }

        public VertexViewModel(Vertex vert)
        {

            _vert = vert;
            Name = _vert.Name;
            X = _vert.X;
            Y = _vert.Y;
            ObjectColor = new SolidColorBrush(Colors.Yellow);
            
            VertexSelectionChange = new ActionOnCommand(ChangeSelection);

            DefaultColor = new SolidColorBrush(Colors.Yellow);
            WhenSelectedColor = new SolidColorBrush(Colors.Red);
        }
    }
}
