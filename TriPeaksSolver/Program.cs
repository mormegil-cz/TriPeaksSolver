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
                var depthFirstSearch = new DepthFirstSearch(game);
                if (depthFirstSearch.Solve())
                    Console.WriteLine("Solved!");
                else
                    Console.WriteLine("Unsolved");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }
    }
}