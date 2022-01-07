using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs2.Models
{
    public class Matrix<T>
    {
        public T[][] Values;
        public int Height;
        public int Width;

        public Matrix(int width, int height) 
        {
            if (width < 0 || height < 0)
                throw new Exception("Negative width ore height is impossible");
            Values = new T[width][];
            for( int i = 0; i < height; ++i) 
                Values[i] = new T[height];
            Height = (int)height;
            Width = (int)width;
        }

        /*
        public int GetValue(int i, int j) 
        {
            if (i > Values.Length)
                if (j > Values[i].Length)
                    throw new IndexOutOfRangeException();

            return Values[i][j];
        }
        */
    }
}
