namespace NPuzzle
{
    public class NPuzzle
    {
        public PuzzleState StartState { get; set; }
        public PuzzleState GoalState { get; set; }

        public NPuzzle(PuzzleState startState, PuzzleState goalState)
        {
            //initialise the start and end states.
            StartState = startState;
            GoalState = goalState;
        }

        public NPuzzle(int[,] startState, int[,] goalState)
        {
            //initialise the start and end states using only the puzzle arrays
            StartState = new PuzzleState(startState);
            GoalState = new PuzzleState(goalState);
        }
    }
}
