using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Net;
using System.Numerics;

namespace MatrixDeterminant
{
    class Program
    {
        private static Random rnd = new Random();
        private static void Main()    
        {
            try
            {
                int matrixSize=0;
                ReadMatrixSize(ref matrixSize);
                List<List<int>> mat = new List<List<int>>();
                ReadMatrix(mat,matrixSize);
                //mat = GenerateNewRandomMat(matrixSize,10);
                //WriteMatrix(mat);

                BigInteger determinant = GetDeterminantRecursive(mat);
                Console.WriteLine($"This matrix determinant is equal to: {determinant}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void WriteMatrix(List<List<int>> mat)
        {
            Console.WriteLine($"Matrix ({mat.Count} x {mat.Count}):");
            for (int i = 0; i < mat.Count; i++)
                Console.WriteLine(String.Join(" ", mat[i]));
        }

        private static List<List<int>> GenerateNewRandomMat(int matrixSize,int limit)
        {
            List<List<int>> newMat = new List<List<int>>();
            for(int i=0;i<matrixSize;i++)
            {
                List<int> row = new List<int>();
                for (int j = 0; j < matrixSize; j++)
                    row.Add(rnd.Next(-limit, limit));
                newMat.Add(row);
            }
            return newMat;
        }

        private static void ReadMatrixSize(ref int matrixSize)
        {
            const int maxSize = 12; //for matrix size 12 it takes between 3 and 5 minutes to compute the determinant
            Console.Write($"Give the matrix size (0<d<{maxSize + 1}): ");
            bool ok;
            do
            {
                ok = true;
                matrixSize = int.Parse(Console.ReadLine());
                if (matrixSize < 0 || matrixSize > maxSize)
                {
                    ok = false;
                    Console.Write($"{matrixSize} value is not suitable for the matrix size, chose other value (0<d<{maxSize + 1}): ");
                }
            } while (!ok);
        }

        private static void ReadMatrix(List<List<int>> matrix,int matrixSize)
        {
            Console.WriteLine($"Write all the matrix elements (separated only by white spaces and new lines) ({matrixSize} x {matrixSize} matrix):");
            for (int i = 0; i < matrixSize; i++)
            {
                var line = Console.ReadLine().Split(' ');
                List<int> row = new List<int>(); 
                foreach (var el in line)
                    row.Add(int.Parse(el));
                matrix.Add(row);
            }
        }
        /// <summary>
        /// recursive method for compute the matrix determinant
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private static BigInteger GetDeterminantRecursive(List<List<int>> matrix)
        {
            if (matrix.Count == 2)
                return BigInteger.Parse((matrix[0][0] * matrix[1][1] - matrix[0][1] * matrix[1][0]).ToString());
            else
            {
                BigInteger determinant = BigInteger.Zero;
                for (int j = 0; j < matrix.Count; j++)
                        determinant += BigInteger.Parse(Math.Pow(-1,j+2).ToString()) * matrix[0][j] * GetDeterminantRecursive(GetMinor(matrix, 0, j));
                        // determinant = determinant + (-1)^(i+j) * a[1][j] * determinant(minor(a,1,j))
                return determinant;
            }
        }

        /// <summary>
        /// receive a matrix and returns the minor of this matrix Mij = GetMinor(matrix,i,j); 
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="ignoreRow"></param>
        /// <param name="ignoreColumn"></param>
        /// <returns></returns>
        private static List<List<int>> GetMinor(List<List<int>> matrix, int ignoreRow, int ignoreColumn)
        {
            List<List<int>> Minor = new List<List<int>>();
            for (int i = 0; i < matrix.Count; i++)
            {
                List<int> row = new List<int>();
                for (int j = 0; j < matrix.Count; j++)
                    if (i != ignoreRow && j != ignoreColumn)
                        row.Add(matrix[i][j]);

                if(i!=ignoreRow)
                    Minor.Add(row);
            }
            
            return Minor;
        }
    }
}

