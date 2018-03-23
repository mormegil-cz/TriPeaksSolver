using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace TriPeaksSolver
{
    public class TriPeaksGame
    {
        /*
         *    0     1     2
         *   3 4   5 6   7 8
         *  9 A B C D E F G H
         * I J K L M N O P Q R
         */

        private static readonly sbyte[] firstNeighbor =
            {3, 5, 7, 9, 10, 12, 13, 15, 16, 18, 19, 20, 21, 22, 23, 24, 25, 26};

        private static readonly sbyte[] secondNeighbor =
            {4, 6, 8, 10, 11, 13, 14, 16, 17, 19, 20, 21, 22, 23, 24, 25, 26, 27};

        private readonly byte[] peaks;
        private readonly byte[] deck;

        public static TriPeaksGame LoadFromFile(String filename)
        {
            using (var reader = new StreamReader(filename))
            {
                return LoadFromFile(reader);
            }
        }

        public static TriPeaksGame LoadFromFile(TextReader reader)
        {
            var peaksStr = reader.ReadLine();
            var deckSizeStr = reader.ReadLine();
            var deckSize = Int32.Parse(deckSizeStr, CultureInfo.InvariantCulture);
            var deckStr = reader.ReadLine();
            return new TriPeaksGame(ParseCards(peaksStr), ParseCards(deckStr));
        }

        private static byte[] ParseCards(string line)
        {
            var cardsStr = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            byte[] result = new byte[cardsStr.Length];
            for (var i = 0; i < cardsStr.Length; ++i)
            {
                result[i] = ParseCard(cardsStr[i]);
            }

            return result;
        }

        private static byte ParseCard(string cardStr)
        {
            char card = cardStr[0];
            if (card >= '2' && card <= '9')
            {
                return (byte) ((int) card - (int) '0');
            }
            else
            {
                switch (Char.ToUpperInvariant(card))
                {
                    case 'T':
                        return 10;
                    case 'J':
                        return 11;
                    case 'Q':
                        return 12;
                    case 'K':
                        return 13;
                    case 'A':
                        return 1;
                    default:
                        throw new FormatException();
                }
            }
        }

        public TriPeaksGame(byte[] peaks, byte[] deck)
        {
            this.peaks = peaks;
            this.deck = deck;
        }

        public GameState CreateInitialState()
        {
            return new GameState(0, 1, deck[0]);
        }

        public IEnumerable<GameState> ComputeFollowingStates(GameState state)
        {
            for (var i = 0; i < firstNeighbor.Length; ++i)
            {
                if ((state.CardsOut & (1UL << i)) == 0
                    && (state.CardsOut & (1UL << firstNeighbor[i])) != 0
                    && (state.CardsOut & (1UL << secondNeighbor[i])) != 0)
                {
                    if ((13 + state.CurrentCard - peaks[i]) % 13 == 1)
                    {
                        yield return new GameState(state.CardsOut | (1UL << i), state.DeckPosition, peaks[i]);
                    }
                }
            }

            for (int i = 18; i <= 27; ++i)
            {
                if ((state.CardsOut & (1UL << i)) == 0)
                {
                    if ((13 + state.CurrentCard - peaks[i]) % 13 == 1)
                    {
                        yield return new GameState(state.CardsOut | (1UL << i), state.DeckPosition, peaks[i]);
                    }
                }
            }

            if (state.DeckPosition < deck.Length)
            {
                yield return new GameState(state.CardsOut, state.DeckPosition + 1, deck[state.DeckPosition]);
            }
        }

        public string PrintState(GameState state)
        {
            var result = new StringBuilder();
            result.Append("   ");
            for (int i = 0; i < 2; ++i)
            {
                result.Append(PrintCard(state.CardsOut, i));
                result.Append("     ");
            }

            result.AppendLine();
            result.Append("  ");
            for (int i = 0; i < 2; ++i)
            {
                result.Append(PrintCard(state.CardsOut, i));
                result.Append("   ");
            }

/*
 *    0     1     2
 *   3 4   5 6   7 8
 *  9 A B C D E F G H
 * I J K L M N O P Q R
 */
        }
    }
}