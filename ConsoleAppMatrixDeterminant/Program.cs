using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Dynamic;


namespace permutari
{
    class Program
    {
        public static int matrixSize;
        public static List<List<int>> storedPermutations = new List<List<int>>();
        private static void Main()
        {
            try
            {
                ReadMatrixSize();
                int[,] mat = new int[matrixSize, matrixSize];
                ReadMatrix(mat);

                int[] permutation = new int[matrixSize];
                GetAllPermutations(permutation, 0);
                
                long determinant = GetDeterminant(mat);
                Console.WriteLine($"This matrix determinant is equal to: {determinant}");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ReadMatrixSize()
        {
            const int maxSize = 10;
            Console.Write($"Give matrix size (0<d<{maxSize + 1}): ");
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

        private static void ReadMatrix(int[,] matrix)
        {
            Console.WriteLine($"Write all the matrix elements (separated only by white spaces and new lines) ({matrixSize} x {matrixSize} matrix):");
            for (int i = 0; i < matrixSize; i++)
            {
                var line = Console.ReadLine().Split(' ');
                for (int j = 0; j < matrixSize; j++)
                    matrix[i, j] = int.Parse(line[j]);
            }
        }

        private static long GetDeterminant(int[,] matrix)
        {
            long determinant = 0;
            for (int i = 0; i < storedPermutations.Count; i++)
                determinant += MultipliedPermSum(matrix, i);
            return determinant;
        }

        private static long MultipliedPermSum(int[,] matrix, int permRow)
        {
            long ret = 1;
            for (int j = 0; j < matrixSize; j++)
                ret *= matrix[storedPermutations[permRow][j], j];  //matrix[permElement][permElementIndex]
            return ret * GetSignForThisPerm(permRow);
        }

        private static int GetSignForThisPerm(int permRow)
        {
            int counter = 0;
            for (int j = 0; j < matrixSize - 1; j++)
                for (int k = j + 1; k < matrixSize; k++)
                    if (storedPermutations[permRow][j] > storedPermutations[permRow][k])
                        counter++;
            if (counter % 2 == 0)
                return 1;
            else return -1;
        }

        private static void GetAllPermutations(int[] permutation, int k)
        {
            for (int i = 0; i < matrixSize; i++)
            {
                permutation[k] = i;
                if (isNotAlreadyPartOfThisPerm(permutation, k))
                {
                    if (k == matrixSize - 1)
                        StorePermutation(storedPermutations, permutation);
                    else
                        GetAllPermutations(permutation, k + 1);
                }
            }
        }

        private static bool isNotAlreadyPartOfThisPerm(int[] permutation, int k)
        {
            for (int i = 0; i < k; i++)
                if (permutation[i] == permutation[k])
                    return false;
            return true;
        }

        private static void StorePermutation(List<List<int>> storedPermutations, int[] permutation)
        {
            storedPermutations.Add(permutation.ToList());
        }
    }
}

