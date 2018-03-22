using System;

namespace TriPeaksSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = TriPeaksGame.LoadFromFile("game.txt");
            foreach (var state in game.ComputeFollowingStates(game.CreateInitialState()))
            {
                Console.WriteLine(state);
            }
        }
    }
}