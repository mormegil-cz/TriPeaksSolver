using System;
using System.Text;

namespace TriPeaksSolver
{
    public struct GameState
    {
        public readonly uint CardsOut;
        public readonly sbyte DeckPosition;
        public readonly byte CurrentCard;

        public GameState(uint cardsOut, sbyte deckPosition, byte currentCard)
        {
            CardsOut = cardsOut;
            DeckPosition = deckPosition;
            CurrentCard = currentCard;
        }

        public bool IsSolved => CardsOut == (1U << 28) - 1;

        public bool Equals(GameState other)
        {
            return other.CardsOut == CardsOut && other.DeckPosition == DeckPosition && other.CurrentCard == CurrentCard;
        }

        public override bool Equals(object obj)
        {
            return (obj is GameState other) && Equals(other);
        }

        public override int GetHashCode()
        {
            return CardsOut.GetHashCode() ^ ((DeckPosition << 8) | CurrentCard);
        }
    }
}