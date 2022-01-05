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

        public Matrix(uint width, uint height) 
        {
            Values = new int[width][];
            for( int i = 0; i < height; ++i) 
                Values[i] = new int[height];
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
