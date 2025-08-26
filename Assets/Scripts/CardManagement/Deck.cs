namespace Scripts.Deck
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class Deck : MonoBehaviour
    {
        public List<PlayCard> AllCards => cardsInDeck.Concat(cardsInDiscard).ToList();
        public bool DeckEmpty => cardsInDeck.Count == 0;

        [SerializeField] public readonly Queue<PlayCard> cardsInDeck = new();
        public readonly List<PlayCard> cardsInDiscard = new();

        readonly string cardDataPath = "Cards";

        public void Initialize()
        {
            GatherCards();
            ShuffleCards();
        }

        protected void GatherCards()
        {
            List<Card> cards = Resources.LoadAll<Card>(cardDataPath).ToList();
            foreach (var card in cards)
            {
                PlayCard playCard = new();
                playCard.Initialize(card);

                cardsInDeck.Enqueue(playCard);
            }
            Debug.Log($"deck size is {cardsInDeck.Count}");
        }

        // MADE REFERENCING KNUTH SHUFFLE ALGORITHM
        // https://rosettacode.org/wiki/Knuth_shuffle
        public void ShuffleCards()
        {
            List<PlayCard> shuffledCards = AllCards.ToList();

            // Create a shuffled list of cards
            for (int i = AllCards.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);

                // swap cards
                (shuffledCards[j], shuffledCards[i]) = (shuffledCards[i], shuffledCards[j]);
            }

            // Queue the cards into deck
            cardsInDeck.Clear();
            foreach (var card in shuffledCards)
            {
                cardsInDeck.Enqueue(card);
            }

            // Clear the discard (it was shuffled back in)
            cardsInDiscard.Clear();
        }

        public PlayCard Draw()
        {
            if (cardsInDeck.Count > 0)
            {
                return cardsInDeck.Dequeue();
            }
            else
            {
                return null;
            }
        }

        public void AddToDeck(PlayCard card)
        {
            cardsInDeck.Enqueue(card);
        }
        
        public void Discard(PlayCard card)
        {
            if (card == null) throw new ArgumentNullException();
            Debug.Log($"Adding card to discrd: {card.Card.name}");
            cardsInDiscard.Add(card);
        }
    }
}
