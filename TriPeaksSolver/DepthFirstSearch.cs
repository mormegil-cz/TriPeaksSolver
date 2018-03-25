using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;

namespace TriPeaksSolver
{
    public class DepthFirstSearch
    {
        private readonly TriPeaksGame game;
        private readonly HashSet<GameState> visitedStates = new HashSet<GameState>();

        public DepthFirstSearch(TriPeaksGame game)
        {
            this.game = game;
        }

        public bool Solve()
        {
            var result = RecursiveSolve(game.CreateInitialState());
            Console.WriteLine("{0} states visited", visitedStates.Count);
            return result;
        }

        private bool RecursiveSolve(GameState state)
        {
            if (visitedStates.Contains(state)) return false;
            visitedStates.Add(state);

            if (state.IsSolved)
            {
                Console.WriteLine("************** SOLVED ****************");
                Console.WriteLine(game.PrintState(state));
                return true;
            }
            if (game.IsUnsolvable(state))
            {
                return false;
            }

            foreach (var nextState in game.ComputeFollowingStates(state))
            {
                // Console.WriteLine(game.PrintState(nextState));
                if (RecursiveSolve(nextState))
                {
                    Console.WriteLine(game.PrintState(state));
                    return true;
                }
            }

            // Thread.CurrentThread.Abort();
            return false;
        }
    }
}