using Graphs2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs2.ViewModels
{
    class AdjacentyMatrixViewModel : BaseViewModel
    {
        public AdjacentyMatrix AdjMat;

        private ObservableCollection<ObservableCollection<int>> _rows;
        public ObservableCollection<ObservableCollection<int>> Rows
        {
            get => _rows;
            set
            {
                _rows = value;
                OnPropertyChanged(nameof(Rows));
            }
        }

        private ObservableCollection<int> _line;
        public ObservableCollection<int> Line
        {
            get => _line;
            set
            {
                _line = value;
                OnPropertyChanged(nameof(Line));
            }
        }

        public AdjacentyMatrixViewModel(AdjacentyMatrix adjmat)
        {
            AdjMat = adjmat;
            Rows = new ObservableCollection<ObservableCollection<int>>();
            for( int i = 0; i < AdjMat.Mat.Width; ++i)
            {
                Line = new ObservableCollection<int>();
                for( int j = 0; j < AdjMat.Mat.Height; ++i)
                {
                    Line.Add(AdjMat.Mat.Values[i][j]);
                }
                Rows.Add(Line);
            }
        }
    }
}
