using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Net;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;

namespace GaussMatrixDeterminant
{
    class Program
    {
        private static Random rnd = new Random();
        private static void Main()
        {
            try
            {
                int matrixSize = ReadMatrixSize();
                //List<List<double>> mat = ReadMatrix(matrixSize);

                int MaxAbsValue = 10;
                List<List<double>> mat = GenerateNewRandomMat(matrixSize, MaxAbsValue);
                WriteMatrix(mat);

                double determinant = GetDeterminantGauss(mat);                    
                Console.WriteLine($"This matrix determinant is equal to: {determinant}");              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void WriteMatrix(List<List<double>> mat)
        {
            Console.WriteLine($"Matrix ({mat.Count} x {mat.Count}):");
            for (int i = 0; i < mat.Count; i++)
                Console.WriteLine(String.Join(" ", mat[i]));
        }
        /// <summary>
        /// returns a matrix of size matrixSize x matrixSize with random generated elements between -limit and limit
        /// </summary>
        /// <param name="matrixSize"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        private static List<List<double>> GenerateNewRandomMat(int matrixSize, int limit)
        {
            List<List<double>> newMat = new List<List<double>>();

            for (int i = 0; i < matrixSize; i++)
            {
                List<double> row = new List<double>();
                for (int j = 0; j < matrixSize; j++)
                    row.Add(rnd.Next(-limit, limit + 1));
                newMat.Add(row);
            }

            return newMat;
        }

        private static int ReadMatrixSize()
        {
            const int maxSize = 1000;
            int matrixSize;

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

            return matrixSize;
        }

        private static List<List<double>> ReadMatrix(int matrixSize)
        {
            Console.WriteLine($"Write all the matrix elements (separated only by white spaces and new lines) ({matrixSize} x {matrixSize} matrix):");
            List<List<double>> newMat = new List<List<double>>();

            for (int i = 0; i < matrixSize; i++)
            {
                var line = Console.ReadLine().Split(' ');
                List<double> row = new List<double>();
                foreach (var el in line)
                    row.Add(int.Parse(el));
                newMat.Add(row);
            }

            return newMat;
        }
        /// <summary>
        /// Gauss method for computing the matrix determinant
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private static double GetDeterminantGauss(List<List<double>> matrix)
        {
            const double EPSILON = 1E-9;
            double det = 1;
            for (int i = 0; i < matrix.Count; ++i)
            {
                int k = i;
                for (int j = i + 1; j < matrix.Count; ++j)
                    if (Math.Abs(matrix[j][i]) > Math.Abs(matrix[k][i]))
                        k = j;
                if (Math.Abs(matrix[k][i]) < EPSILON)
                {
                    det = 0;
                    break;
                }
                (matrix[i], matrix[k]) = (matrix[k], matrix[i]);
                if (i != k)
                    det = -det;
                det *= matrix[i][i];
                for (int j = i+1; j < matrix.Count; ++j)
                        matrix[i][j] /= matrix[i][i];
                for (int j = 0; j < matrix.Count; ++j)
                    if (j != i && Math.Abs(matrix[j][i]) > EPSILON)
                        for (int z = i+1; z < matrix.Count; ++z)
                                matrix[j][z] -= matrix[i][z] * matrix[j][i];
            }
            return Math.Round(det);
        }
    }
}

