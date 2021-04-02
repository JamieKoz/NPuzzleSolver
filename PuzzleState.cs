using System;
using System.Collections.Generic;
using NPuzzle.Exceptions;

namespace NPuzzle
{
    public class PuzzleState : IEquatable<PuzzleState>
    {
        public int[,] Puzzle;
        public PuzzleState Parent;
        public List<PuzzleState> Children;
        public int Cost;
        public int HeuristicValue;
        public int EvaluationFunction { get; set; }
        public Direction PathFromParent;

        public PuzzleState(PuzzleState parent, Direction fromParent, int[,] puzzle)
        {
            Parent = parent;
            PathFromParent = fromParent;
            Puzzle = puzzle;
            Cost = Parent.Cost + 1;
            EvaluationFunction = 0;
            HeuristicValue = 0;
        }

        public PuzzleState(int[,] puzzle)
        {
            Parent = null;
            PathFromParent = Direction.None;
            Cost = 0;
            Puzzle = puzzle;
            EvaluationFunction = 0;
            HeuristicValue = 0;
        }

        public Direction[] GetPossibleActions()
        {

            int[] blankLocation = { 0, 0 }; //dummy value to avoid errors.

            try
            {
                //find where the blank cell is and store the directions.
                blankLocation = FindBlankCell();
            }
            catch (InvalidPuzzleException)
            {
                Console.WriteLine("There was an error in processing! Aborting...");
                Environment.Exit(1);
            }

            Direction[] result = new Direction[CountMovements(blankLocation)];

            int thisIndex = 0;

            // X check
            if (blankLocation[0] == 0)
            {
                //the blank cell is already as far left as it will go, it can move right
                result[thisIndex++] = Direction.Right;
            }
            else if (blankLocation[0] == (Puzzle.Length - 1))
            {
                result[thisIndex++] = Direction.Left;
            }
            else
            {
                result[thisIndex++] = Direction.Left;
                result[thisIndex++] = Direction.Right;
            }

            // Y Check
            if (blankLocation[1] == 0)
            {
                //the blank cell is already as far up as it will go, it can move down
                result[thisIndex++] = Direction.Down;
            }
            else if (blankLocation[1] == (Puzzle.Length - 1))
            {
                result[thisIndex++] = Direction.Up;
            }
            else
            {
                result[thisIndex++] = Direction.Up;
                result[thisIndex++] = Direction.Down;
            }
            return result;
        }

        private int[] FindBlankCell()
        {
            for (int i = 0; i < Puzzle.GetLength(0); i++)
            {
                for (int j = 0; j < Puzzle.GetLength(1); j++)
                {
                    if (Puzzle[i, j] == 0)
                    {
                        int[] result = { i, j };
                        return result;
                    }
                }
            }
            //No blank cell found?
            throw new InvalidPuzzleException();
        }

        private int CountMovements(int[] blankLocation)
        {
            int result = 2;
            try
            {
                blankLocation = FindBlankCell();

                for (int i = 0; i <= 1; i++)
                {
                    if (blankLocation[i] == 0 || blankLocation[i] == (Puzzle.Length - 1))
                    {
                        //do nothing
                    }
                    else
                    {
                        result++;
                    }
                }
            }
            catch (InvalidPuzzleException)
            {
                //do something
            }
            return result;
        }

        private int[,] CloneArray(int[,] cloneMe)
        {
            int[,] result = new int[cloneMe.GetLength(0), cloneMe.GetLength(1)];

            for (int i = 0; i < cloneMe.GetLength(0); i++)
            {
                for (int j = 0; j < cloneMe.GetLength(1); j++)
                {
                    result[i, j] = cloneMe[i, j];
                }
            }
            return result;
        }

        public PuzzleState Move(Direction direction)
        {
            //Moving up moves the empty cell up (and the cell above it down)
            //first, create the new one (the one to return)
            PuzzleState result = new PuzzleState(this, direction, CloneArray(this.Puzzle));

            //now, execute the changes: move the blank cell aDirection
            //find the blankCell
            int[] blankCell = { 0, 0 };
            try
            {
                blankCell = FindBlankCell();
            }
            catch (InvalidPuzzleException)
            {
                Console.WriteLine("There was an error in processing! Aborting...");
                Environment.Exit(1);
            }
            try
            {
                //move the blank cell in the new child puzzle

                if (direction == Direction.Up)
                {
                    result.Puzzle[blankCell[0], blankCell[1]] = result.Puzzle[blankCell[0], blankCell[1] - 1];
                    result.Puzzle[blankCell[0], blankCell[1] - 1] = 0;
                }
                else if (direction == Direction.Down)
                {
                    result.Puzzle[blankCell[0], blankCell[1]] = result.Puzzle[blankCell[0], blankCell[1] + 1];
                    result.Puzzle[blankCell[0], blankCell[1] + 1] = 0;
                }
                else if (direction == Direction.Left)
                {
                    result.Puzzle[blankCell[0], blankCell[1]] = result.Puzzle[blankCell[0] - 1, blankCell[1]];
                    result.Puzzle[blankCell[0] - 1, blankCell[1]] = 0;
                }
                else    //aDirection == Right;
                {
                    result.Puzzle[blankCell[0], blankCell[1]] = result.Puzzle[blankCell[0] + 1, blankCell[1]];
                    result.Puzzle[blankCell[0] + 1, blankCell[1]] = 0;
                }
                return result;
            }
            catch (IndexOutOfRangeException)
            {
                throw new CantMoveThatWayException();
            }
        }

        public List<PuzzleState> Explore()
        {
            //populate children
            Direction[] possibleMoves = GetPossibleActions();
            Children = new List<PuzzleState>();
            for (int i = 0; i < possibleMoves.Length; i++)
            {
                try
                {
                    Children.Add(Move(possibleMoves[i]));
                }
                catch (CantMoveThatWayException)
                {
                    Console.WriteLine("There was an error in processing! Aborting...");
                    Environment.Exit(1);
                }
            }
            return Children;
        }

        public Direction[] GetPathToState()
        {
            Direction[] result;

            if (Parent == null) //If this is the root node, there is no path!
            {
                result = new Direction[0];
                return result;
            }
            else                //Other wise, path to here is the path to parent
                                // plus parent to here
            {
                Direction[] pathToParent = Parent.GetPathToState();
                result = new Direction[pathToParent.Length + 1];
                for (int i = 0; i < pathToParent.Length; i++)
                {
                    result[i] = pathToParent[i];
                }
                result[result.Length - 1] = this.PathFromParent;
                return result;
            }
        }

        public bool Equals(PuzzleState other)
        {
            //evaluate if these states are the same (does this.Puzzle == aState.Puzzle)?
            for (int i = 0; i < Puzzle.GetLength(0); i++)
            {
                for (int j = 0; j < Puzzle.GetLength(1); j++)
                {
                    if (this.Puzzle[i, j] != other.Puzzle[i, j])
                        return false;       //stop checking as soon as we find an
                                            // element that doesn't match
                }
            }
            return true;	//All elements matched? Return true
        }
    }
}