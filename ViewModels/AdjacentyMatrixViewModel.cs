using Graphs2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Graphs2.ViewModels
{
    class CellValue : BaseViewModel
    {
        private Edge _edge;
        public Edge Ed
        {
            get => _edge;
            set
            {
                _edge = value;
                OnPropertyChanged(nameof(Ed));
            }
        }

        public int Weight
        {
            get
            {
                if (Ed is null)
                    return 0;
                else
                    return Ed.Weight;
            }
            set
            {
                OnSetValue?.Invoke(value, _i, _j);
                OnPropertyChanged(nameof(Weight));
            }
        }

        private int _i; private int _j;

        public Action<int, int, int> OnSetValue;

        public CellValue(int i, int j)
        {
            _i = i; _j = j;
        }
    }

    class Row : BaseViewModel
    {
        private ObservableCollection<CellValue> _cells;
        public ObservableCollection<CellValue> Cells
        {
            get => _cells;
            set
            {
                _cells = value;
                OnPropertyChanged(nameof(Cells));
            }
        }
        public Row()
        {
            Cells = new ObservableCollection<CellValue>();
        }
    }

    class AdjacentyMatrixViewModel : BaseViewModel
    {
        public AdjacentyMatrix AdjMat;

        private ObservableCollection<Row> _rows;
        public ObservableCollection<Row> Rows
        {
            get => _rows;
            set
            {
                _rows = value;
                OnPropertyChanged(nameof(Rows));
            }
        }

        public AdjacentyMatrixViewModel(AdjacentyMatrix adjmat)
        {
            AdjMat = adjmat;
            Rows = new ObservableCollection<Row>();
            for( int i = 0; i < AdjMat.Size; ++i)
            {
                Row row = new Row();
                for (int j = 0; j < AdjMat.Size; ++j)
                {
                    CellValue cell = new CellValue(i, j);
                    cell.Ed = AdjMat.At(i, j);
                    cell.OnSetValue = OnSetCellValue;
                    row.Cells.Add(cell);
                }
                Rows.Add(row);
            }
        }

        public void OnSetCellValue(int value, int i, int j)
        {
            AdjMat.SetValue(value, i, j);
        }
    }
}
