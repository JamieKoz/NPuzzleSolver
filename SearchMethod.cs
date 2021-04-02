
using System.Collections.Generic;

namespace NPuzzle
{
    public abstract class SearchMethod
    {
        /// <summary>
        /// The code used to identify the method at the command line
        /// </summary>
        public string code;

        /// <summary>
        /// The actual name of the method.
        /// </summary>
        public string longName;

        public abstract Direction[] Solve(NPuzzle aPuzzle);

        /// <summary>
        /// / The Frontier needs to be a Queue and a Stack.
        /// </summary>
        public Frontier Frontier;

        /// <summary>
        /// The searched list simply needs to be able to store nodes for the purpose of checking
        /// Fast addition and removal is crucial here.
        /// HashSet provides constant time for add, contains and size.
        /// </summary>
        public List<PuzzleState> Searched;

        abstract public bool AddToFrontier(PuzzleState aState);

        abstract protected PuzzleState PopFrontier();

        public void Reset()
        {
            this.Frontier.Clear();
            this.Searched.Clear();
        }
    }
}
