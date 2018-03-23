using System;

namespace TriPeaksSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var game = args.Length == 0
                    ? TriPeaksGame.LoadFromFile(Console.In)
                    : TriPeaksGame.LoadFromFile(args[0]);

                foreach (var state in game.ComputeFollowingStates(game.CreateInitialState()))
                {
                    Console.WriteLine(state);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }
    }
}