using System;
using System.Text;

namespace TriPeaksSolver
{
    public struct GameState
    {
        public readonly ulong CardsOut;
        public readonly int DeckPosition;
        public readonly byte CurrentCard;

        public GameState(ulong cardsOut, int deckPosition, byte currentCard)
        {
            CardsOut = cardsOut;
            DeckPosition = deckPosition;
            CurrentCard = currentCard;
        }

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
            return CardsOut.GetHashCode() ^ DeckPosition.GetHashCode() ^ CurrentCard;
        }
    }
}