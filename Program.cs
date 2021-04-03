using System;
using System.Collections.Generic;
using System.IO;

namespace NPuzzle
{
    class Program
    {
        //the number of methods programmed into nPuzzler
        public static NPuzzle gPuzzle;
        public static List<SearchMethod> lMethods = new List<SearchMethod>();

        static void Main(string[] args)
        {
            //Create method objects
            InitMethods();

            //args contains:
            //  [0] - filename containing puzzle(s)
            //  [1] - method name

            if (args.Length < 2)
            {
                Console.WriteLine("Usage: nPuzzler <filename> <search-method>.");
                Environment.Exit(1);
            }

            //Get the puzzle from the file
            gPuzzle = ReadProblemFile(args[0]);

            string methodCode = args[1];
            SearchMethod thisMethod = null;

            //determine which method the user wants to use to solve the puzzles
            foreach (SearchMethod method in lMethods)
            {
                //do they want this one?
                if (method.code.ToUpperInvariant() == methodCode.ToUpperInvariant())
                {
                    //yes, use this method.
                    thisMethod = method;
                }
            }

            //Has the method been implemented?
            if (thisMethod == null)
            {
                //No, give an error
                Console.WriteLine("Search method identified by " + methodCode + " not implemented. Methods are case sensitive.");
                Environment.Exit(1);
            }

            //Solve the puzzle, using the method that the user chose
            Direction[] thisSolution = thisMethod.Solve(gPuzzle);

            //Print information about this solution
            Console.WriteLine(args[0] + "   " + methodCode + "   " + thisMethod.Searched.Count);
            if (thisSolution == null)
            {
                //No solution found :(
                Console.WriteLine("No solution found.");
            }
            else
            {
                //We found a solution, print all the steps to success!
                for (int j = 0; j < thisSolution.Length; j++)
                {
                    Console.Write(thisSolution[j].ToString() + ";");
                }
                Console.WriteLine();
            }
            //Reset the search method for next use.
            thisMethod.Reset();
            Environment.Exit(0);
        }

        private static void InitMethods()
        {
            lMethods.Add(new BFSStrategy());
            lMethods.Add(new GreedyBestFirstStrategy());
        }

        private static NPuzzle ReadProblemFile(string fileName) // this allow only one puzzle to be specified in a problem file
        {
            try
            {
                //create file reading objects
                StreamReader puzzle = new StreamReader(fileName);

                string puzzleDimension = puzzle.ReadLine();
                //split the string by letter "x"
                string[] bothDimensions = puzzleDimension.Split("x");

                //work out the "physical" size of the puzzle
                //here we only deal with NxN puzzles, so the puzzle size is taken to be the first number
                int puzzleSize = int.Parse(bothDimensions[0]);

                int[,] startPuzzleGrid = new int[puzzleSize, puzzleSize];
                int[,] goalPuzzleGrid = new int[puzzleSize, puzzleSize];

                //fill in the start state
                string startStateString = puzzle.ReadLine();
                startPuzzleGrid = ParseStateString(startStateString, startPuzzleGrid, puzzleSize);

                //fill in the end state
                string goalStateString = puzzle.ReadLine();
                goalPuzzleGrid = ParseStateString(goalStateString, goalPuzzleGrid, puzzleSize);

                //create the nPuzzle object...
                NPuzzle result = new NPuzzle(startPuzzleGrid, goalPuzzleGrid);

                puzzle.Dispose();
                return result;
            }
            catch (FileNotFoundException)
            {
                //The file didn't exist, show an error
                Console.WriteLine("Error: File \"" + fileName + "\" not found.");
                Console.WriteLine("Please check the path to the file.");
                Environment.Exit(1);
            }
            catch (IOException)
            {
                //There was an IO error, show and error message
                Console.WriteLine("Error in reading \"" + fileName + "\". Try closing it and programs that may be accessing it.");
                Console.WriteLine("If you're accessing this file over a network, try making a local copy.");
                Environment.Exit(1);
            }

            return null;
        }

        private static int[,] ParseStateString(string stateString, int[,] puzzleGrid, int width)
        {
            // Parse state string converts the text file's format for each puzzle into
            // multidimensional arrays.

            //split the string by spaces
            string[] tileLocations = stateString.Split(" ");

            // the top-left corner of the puzzle has a coordinate of [0,0]
            int x = 0;
            int y = 0;

            foreach (string tileLocation in tileLocations)
            {
                int tileNumber = int.Parse(tileLocation);

                //now, check the location of this tile
                if (x >= width)
                {
                    //reset x to 0 and go to next row (increase y by 1)
                    x = 0;
                    y++;
                }

                puzzleGrid[x, y] = tileNumber;
                x++;
            }

            return puzzleGrid;
        }
    }
}