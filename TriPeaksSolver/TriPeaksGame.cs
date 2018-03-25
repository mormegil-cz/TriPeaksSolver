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
            { 3, 5, 7, 9, 10, 12, 13, 15, 16, 18, 19, 20, 21, 22, 23, 24, 25, 26 };

        private static readonly sbyte[] secondNeighbor =
            { 4, 6, 8, 10, 11, 13, 14, 16, 17, 19, 20, 21, 22, 23, 24, 25, 26, 27 };

        private static readonly String cardNames = "?A23456789TJQK";

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
            return (byte) cardNames.IndexOf(card);
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
                if ((state.CardsOut & (1U << i)) == 0
                    && (state.CardsOut & (1U << firstNeighbor[i])) != 0
                    && (state.CardsOut & (1U << secondNeighbor[i])) != 0)
                {
                    int diff = (13 + state.CurrentCard - peaks[i]) % 13;
                    if (diff == 1 || diff == 12)
                    {
                        yield return new GameState(state.CardsOut | (1U << i), state.DeckPosition, peaks[i]);
                    }
                }
            }

            for (int i = 18; i <= 27; ++i)
            {
                if ((state.CardsOut & (1U << i)) == 0)
                {
                    int diff = (13 + state.CurrentCard - peaks[i]) % 13;
                    if (diff == 1 || diff == 12)
                    {
                        yield return new GameState(state.CardsOut | (1U << i), state.DeckPosition, peaks[i]);
                    }
                }
            }

            if (state.DeckPosition < deck.Length)
            {
                yield return new GameState(state.CardsOut, (sbyte) (state.DeckPosition + 1), deck[state.DeckPosition]);
            }
        }

        public string PrintState(GameState state)
        {
            var result = new StringBuilder();
            for (int i = 0; i < 3; ++i)
            {
                result.Append("   ");
                result.Append(PrintCard(state.CardsOut, i));
                result.Append(' ');
            }
            result.AppendLine();
            for (int i = 0; i < 3; ++i)
            {
                result.Append("  ");
                for (int j = 0; j < 2; ++j)
                {
                    result.Append(PrintCard(state.CardsOut, i * 2 + j + 3));
                    result.Append(' ');
                }
            }
            result.AppendLine();
            for (int i = 0; i < 9; ++i)
            {
                result.Append(' ');
                result.Append(PrintCard(state.CardsOut, 9 + i));
            }
            result.AppendLine();
            for (int i = 0; i < 10; ++i)
            {
                result.Append(PrintCard(state.CardsOut, 18 + i));
                result.Append(' ');
            }
            result.AppendLine();
            result.AppendLine();
            result.Append(" [");
            result.Append(cardNames[state.CurrentCard]);
            result.Append("] ");
            for (int i = state.DeckPosition; i < deck.Length; ++i)
            {
                result.Append(cardNames[deck[i]]);
                result.Append(' ');
            }
            result.AppendLine();
            return result.ToString();
        }

        private char PrintCard(uint cardsOut, int index)
        {
            return (cardsOut & (1U << index)) == 0 ? cardNames[peaks[index]] : '_';
        }

        public bool IsUnsolvable(GameState state)
        {
            // TODO
            return false;
        }
    }
}