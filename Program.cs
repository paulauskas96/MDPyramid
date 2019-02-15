using System;
using System.Collections.Generic;
using System.IO;

namespace MDPyramid
{
    class Program
    {
        static int[][] Pyramid;
        static int[] Path;

        static void Main(string[] args)
        {
            Pyramid = ReadInput();
            
            Console.WriteLine("Current pyramid:");
            foreach (var row in Pyramid)
                Console.WriteLine(string.Join(" ", row));

            var sum = Calculate();

            Console.WriteLine("Max sum: " + sum);
            Console.WriteLine("Path: " + string.Join(" -> ", Path));
            Console.ReadLine();
        }

        private static int Calculate()
        {
            Path = new int[Pyramid.Length];
            Path[0] = Pyramid[0][0];

            bool deadEnd = false;

            int sum = GetLargestSum(0, 0, 0, ref deadEnd);

            if (deadEnd)
            {
                Console.WriteLine("There is no way down the pyramid.");
                return 0;
            }

            return sum;
        }

        private static int GetLargestSum(int sum, int i, int j, ref bool deadEnd)
        {
            int leftSum = int.MinValue;
            int rightSum = int.MinValue;

            int value = Pyramid[i][j];

            //Reached the bottom
            if (Pyramid.Length == i + 1)
            {
                deadEnd = false;
                return sum + value;
            }

            int mod = Pyramid[i][j] % 2;

            bool canGoLeft = Pyramid[i + 1][j] % 2 != mod;
            bool canGoRight = Pyramid[i + 1][j + 1] % 2 != mod;

            //Calculates next node sum recursively if it is allowed going there
            if (canGoLeft)
            {
                leftSum = GetLargestSum(sum + value, i + 1, j, ref deadEnd);
                if (deadEnd)
                    canGoLeft = false;
            }

            if (canGoRight)
            {
                rightSum = GetLargestSum(sum + value, i + 1, j + 1, ref deadEnd);
                if (deadEnd)
                    canGoRight = false;
            }

            if (!canGoLeft && !canGoRight)
            {
                deadEnd = true;
                return 0;
            }

            deadEnd = false;

            //Checking which of the next nodes provides greater sum
            if (leftSum >= rightSum)
            {
                Path[i + 1] = Pyramid[i + 1][j];
                return leftSum;
            }
            else
            {
                Path[i + 1] = Pyramid[i + 1][j + 1];
                return rightSum;
            }
        }

        static int[][] ReadInput()
        {
            while (true)
            {
                string[] rows;

                Console.WriteLine("Enter file absolute path:");

                try
                {
                    var path = Console.ReadLine();
                    rows = File.ReadAllLines(path);
                }


                catch (Exception)
                {
                    Console.WriteLine("Could not find file from path specified. Please retry.");
                    continue;
                }

                var success = true;
                var pyramid = new List<int[]>();

                foreach (var row in rows)
                {
                    var rowNodes = new List<int>();
                    var rowNodeTexts = row.Split(' ');

                    if (rowNodeTexts.Length != pyramid.Count + 1)
                    {
                        Console.WriteLine("Input does not match pyramid format.");
                        success = false;
                        break;
                    }

                    try
                    {
                        foreach (var nodeText in rowNodeTexts)
                            rowNodes.Add(Convert.ToInt32(nodeText));
                    }

                    catch (FormatException)
                    {
                        Console.WriteLine("There are non-integer nodes.");
                        success = false;
                        break;
                    }

                    pyramid.Add(rowNodes.ToArray());
                }

                if (!success)
                    continue;

                return pyramid.ToArray();
            }
        }
    }
}
