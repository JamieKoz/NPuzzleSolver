using System;
using System.Collections.Generic;

namespace NPuzzle
{
    public class AStarStrategy : SearchMethod
    {
        public AStarStrategy()
        {
            code = "ASTAR";
            longName = "A-Star Search";
            Frontier = new Frontier();
            Searched = new List<PuzzleState>();
        }

        public override bool AddToFrontier(PuzzleState aState)
        {
            //We only want to add the new state to the fringe if it doesn't exist
            // in the fringe or the searched list.
            if (Searched.Contains(aState) || Frontier.Contains(aState))
            {
                return false;
            }
            else
            {
                Frontier.Enqueue(aState);
                return true;
            }
        }

        public override Direction[] Solve(NPuzzle aPuzzle)
        {
            //keep searching the fringe until it's empty.
            //Items are "popped" from the fringe in order of lowest heuristic value.

            //Add the start state to the fringe
            AddToFrontier(aPuzzle.StartState);
            while (Frontier.Count > 0)
            {
                //get the next State
                PuzzleState thisState = PopFrontier();

                //is this the goal state?
                if (thisState.Equals(aPuzzle.GoalState))
                {
                    return thisState.GetPathToState();
                }

                //not the goal state, explore this node
                List<PuzzleState> newStates = thisState.Explore();

                for (int i = 0; i < newStates.Count; i++)
                {
                    PuzzleState newChild = newStates[i];
                    //if you can add these new states to the fringe
                    if (AddToFrontier(newChild))
                    {
                        //then, work out it's heuristic value
                        newChild.HeuristicValue = HeuristicValue(newStates[i], aPuzzle.GoalState);
                        newChild.EvaluationFunction = newChild.HeuristicValue;
                    }
                }


                Frontier.SortByHeuristicAsc();
            }

            //no more nodes and no path found?
            return null;
        }

        protected override PuzzleState PopFrontier()
        {
            //remove a state from the top of the fringe so that it can be searched.
            PuzzleState lState = Frontier.Pop();

            //add it to the list of searched states so that duplicates are recognised.
            Searched.Add(lState);

            return lState;
        }
        private int HeuristicValue(PuzzleState aState, PuzzleState goalState)
        {
            //find out how many elements in aState match the goalState
            int heuristic = 0;

            for (int i = 0; i < aState.Puzzle.GetLength(0); i++)
            {
                for (int j = 0; j < aState.Puzzle.GetLength(1); j++)
                {
                    var (X, Y) = FindCellContaining(goalState.Puzzle, aState.Puzzle[i, j]);

                    var dx = Math.Abs(i - X);
                    var dy = Math.Abs(j - Y);

                    var manhattenDistance = dx + dy;

                    heuristic += manhattenDistance;
                }
            }

            return heuristic + aState.Cost;
        }

        public (int X, int Y) FindCellContaining(int[,] puzzle, int value)
        {
            for (int i = 0; i < puzzle.GetLength(0); i++)
            {
                for (int j = 0; j < puzzle.GetLength(1); j++)
                {
                    if (puzzle[i, j] == value)
                    {
                        return (i, j);
                    }
                }
            }
            throw new Exception("somethings broken");
        }
    }
}