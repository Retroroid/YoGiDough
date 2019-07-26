using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuGiDough {
    public class Deck {
        public Stack<Card> cardList;
        public string deckName, deckLink;
        public Random rng;
        public Deck() { this.cardList = new Stack<Card>(); }
        public Deck(Random rnge) { this.cardList = new Stack<Card>(); this.rng = rnge; }
        public void addCard(Card cd) { this.cardList.Push(cd); }
        //-------------------------------------------------------------------------------------------
        public String toString() { return this.cardList.Count + " cards."; }
        //-------------------------------------------------------------------------------------------
        public string[] printCardList() {
            StringBuilder sb = new StringBuilder();
            foreach (Card cd in this.cardList) {
                sb.AppendLine(cd.cdlink);
            }
            return sb.ToString().Split('\n');
        }
        //-------------------------------------------------------------------------------------------
        public void shuffle() {
            List<Card> tempDeck = this.cardList.ToList();
            this.cardList = new Stack<Card>();
            int index = 0;
            while(tempDeck.Count > 0) {
                index = this.rng.Next(0,tempDeck.Count);
                Card tempCard = tempDeck[index];
                tempDeck.RemoveAt(index);
                this.cardList.Push(tempCard);
            }
        }
        //-------------------------------------------------------------------------------------------
        public static Deck loadDeckFromFile(string decname) {
            Deck newDeck = new Deck();
            newDeck.deckName = decname;
            newDeck.deckLink = Card.basePath + "\\Deck\\" + decname + ".txt";
            string[] deckList = File.ReadAllLines(newDeck.deckLink);
            foreach (string st in deckList) {
                if (!st.Equals(string.Empty)) {
                    st.Trim('\n');
                    Card temp = Card.loadFromFile(st);
                    newDeck.addCard(temp);
                }
            }
            return newDeck;
        }
        //-------------------------------------------------------------------------------------------
        public Card drawCard() {
            return this.cardList.Pop();
        }
        public Card getFaceUpCard() {
            return this.cardList.Peek();
        }
        public string cardCount() {
            return this.cardList.Count.ToString();
        }
        public Card removeByIndex(int index) {
            if (index < 0 && index >= this.cardList.Count) return null;
            List<Card> temparray = this.cardList.ToList();
            Card tc = temparray[index];
            temparray.RemoveAt(index);
            this.cardList.Clear();
            foreach (Card cd in temparray) this.cardList.Push(cd);
            return tc;
        }
    } // End of class
} // End of namespace

