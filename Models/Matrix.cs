using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs2.Models
{
    public class Matrix
    {
        public int[][] Values;
        public int Height;
        public int Width;

        public Matrix(uint width, uint height) 
        {
            Values = new int[width][];
            for( int i = 0; i < height; ++i) 
                Values[i] = new int[height];
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
