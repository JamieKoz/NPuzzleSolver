
using System.Collections.Generic;
using System.Linq;

namespace NPuzzle
{
    public class Frontier
    {
        //We can use a linked list here because it's more efficient than an array
        //and because we only want to add/remove items from the start or end.
        private List<PuzzleState> Items = new List<PuzzleState>();

        public int Count => Items.Count;

        /// <summary>
        /// Removes items from the top of the frontier.
        /// </summary>
        /// <returns></returns>
        public PuzzleState Pop()
        {
            var item = Items[0];
            Items.RemoveAt(0);
            return item;
        }

        /// <summary>
        /// Add a item to the top of the frontier
        /// </summary>
        /// <param name="state"></param>
        public void Push(PuzzleState state)
        {
            Items.Prepend(state);
        }

        public void Push(PuzzleState[] states)
        {
            foreach (PuzzleState state in states)
            {
                Push(state);
            }
        }

        /// <summary>
        ///  Add a item to the bottom the frontier
        /// </summary>
        /// <param name="aState"></param>
        public void Enqueue(PuzzleState state)
        {
            Items.Append(state);
        }

        /// <summary>
        /// Add multiple items to the bottom of the frontier.
        /// Index zero will be enqueued first followed by the rest
        /// of the elements in the array
        /// </summary>
        /// <param name="states"></param>
        public void Enqueue(PuzzleState[] states)
        {
            foreach (PuzzleState state in states)
            {
                Enqueue(state);
            }
        }

        public void Clear()
        {
            Items.Clear();
        }

        public bool Contains(PuzzleState state)
        {
            return Items.Contains(state);
        }

        public void SortByCostAsc()
        {
            //TODO: Implement SortByCostAsc()
        }

        public void SortByCostDesc()
        {
            //TODO: Implement SortByCostDesc()
        }

        public void SortByHeuristicAsc()
        {
            //TODO: Implement SortByHeuristicAsc()
        }

        public void SortByHeuristicDesc()
        {
            //TODO: Implement SortByHeuristicDesc()
        }
    }

}