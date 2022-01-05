using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Graphs2.ViewModels
{
    public class PointViewModel : BaseViewModel
    {
        public Point P { get => new Point(X, Y); }

        private double _x;
        public double X 
        {
            get => _x;
            set 
            {
                _x = value;
                OnPropertyChanged(nameof(X));
                OnPropertyChanged(nameof(P));
            }
        }
        private double _y;
        public double Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
                OnPropertyChanged(nameof(P));
            }
        }

        public PointViewModel(double x = 0, double y = 0 )
        {
            X = x; Y = y;
        }
    }
}
