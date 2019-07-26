using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuGiDough {
    public class Card {
        // ---------------- ---------------- Variables ---------------- ---------------- //
        public int dataBaseID;
        public bool rotated, flipped;
        public string name, cardText, MST, imgLink, cdlink;
        public List<string> cardType;
        public List<set> Sets;
        public monsterData monDat;
        public static string basePath, datPath;
        public static string[] sTypes = { "Normal", "Continuous", "Counter", "Equip", "Field", "Quick-Play", "Ritual" };
        public static string[] attributes = { "Any", "Earth", "Wind", "Fire", "Water", "Dark", "Light", "Divine" };
        public static string[] monsterTypes = { "Any", "Aqua", "Beast", "Beast-Warrior", "Cyberse", "Dinosaur", "Divine-Beast", "Dragon", "Fairy", "Fiend",
            "Fish", "Insect", "Machine", "Plant", "Psychic", "Pyro", "Reptile", "Rock",
            "Sea Serpent", "Spellcaster", "Thunder", "Warrior", "Winged-Beast", "Zombie" };
        // ---------------- ---------------- Structures ---------------- ---------------- //
        public struct set {
            public string setID, setName, setRelease;
            public set(string setRelease, string setID, string setName) {
                this.setID = setID;
                this.setName = setName;
                this.setRelease = setRelease;
            }
        }
        public struct monsterData {
            public string attribute, mType, pEffect;
            public int atk, def, level, rank, link, scale, XyzMat;
            public void setNulls() { pEffect = string.Empty; attribute = string.Empty; mType = string.Empty; atk = 0; def = 0; level = 0; rank = 0; link = 0; scale = 0; }
        }
        // ---------------- ---------------- Constructors ---------------- ---------------- //
        public Card() {
            this.monDat.setNulls();
            Sets = new List<set>();
            cardType = new List<string>();

        }
        // ---------------- ---------------- Methods ---------------- ---------------- //
        public static Card loadFromFile(string cardPath) {
            // Create a string array from the file, and create the card for the data.
            string[] fileData = System.IO.File.ReadAllLines(cardPath);
            Card temp = new Card();
            temp.cdlink = cardPath;
            // Add name and ID to card data.
            temp.dataBaseID = int.Parse(fileData[0]);
            temp.name = fileData[1].Replace("&quot;","\"");
            // Find the location of the card text, and where it begins.
            int i = 2;
            while (!fileData[i].Contains("Card Text")) { i++; }
            temp.cardText = fileData[i + 1];
            // Add all the sets
            for (int j = i + 3; j < fileData.Length - 3; j += 3) { temp.Sets.Add(new set(fileData[j], fileData[j + 1], fileData[j + 2])); }
            // Deal with the card type
            if (fileData[2].Equals("Icon")) {
                string[] tempArr = fileData[3].Split(' ');
                temp.MST = tempArr[1];
                temp.cardType.Add(tempArr[0]);
            }
            else if (fileData[2].Equals("Attribute")) {
                temp.MST = "Monster";
                temp.doMonster(fileData, i);
            }
            else throw new Exception("Error while parsing card type.");

            // Find the card image!
            Image temperr = null;
            string cardname = temp.name;
            cardname = cardname.Split('(')[0].TrimEnd(' ');
            cardname = cardname.Replace("amp;", string.Empty);
            cardname = cardname.Replace(":", "_");
            cardname = cardname.Replace(" ", "_");
            cardname = cardname.Replace("\\", "_");
            cardname = cardname.Replace("&quot;", "_");
            cardname = cardname.Replace("ãƒ»", "・");
            cardname = cardname.Replace("&Uuml;", "Ü");
            cardname = cardname.Replace("&uacute;", "ú");
            cardname = cardname.Replace("?", "%3F");
            cardname = cardname.Replace("/", "_");
            cardname = cardname.Replace("&Omega;", "Ω");
            cardname = cardname.Replace("&beta;", "β");
            cardname = cardname.Replace("&alpha;", "β");
            cardname = cardname.Replace("&ntilde;", "ñ");
            cardname = cardname.Replace("\"", "_");
            try {
                temperr = Image.FromFile(basePath + "\\ydata\\" + cardname + ".jpg");
            }
            catch (Exception) {
                try {
                    cardname = cardname.Replace("-", "_");
                    cardname = cardname.Replace("#", string.Empty);
                    cardname = cardname.Replace("!", "_");
                    temperr = Image.FromFile(basePath + "\\ydata\\" + cardname + ".jpg");
                }
                catch (Exception) {
                    try {
                        cardname = cardname.Replace(".", "_");
                        cardname = cardname.Replace("\'", "%27");
                        temperr = Image.FromFile(basePath + "\\ydata\\" + cardname + ".jpg");
                    }
                    catch (Exception) { }
                }
            }
            temp.rotated = false;
            temp.flipped = false;
            temp.imgLink = basePath + "\\ydata\\" + cardname + ".jpg";
            // Return the card
            return temp;
        }
        // ---------------- ---------------- //
        public override string ToString() { return this.name; }
        public string StringToString() {
            return this.monDat.attribute + this.monDat.mType + this.monDat.pEffect + this.name + this.cardText + this.cardType.ToString();
    }
        // ---------------- ---------------- Helper Methods ---------------- ---------------- //
        private void doMonster(string[] importData, int endCardData) {
            List<string> cardData = new List<string>();
            for (int i = 2; i < endCardData; i++) { cardData.Add(importData[i]); }
            string[] cDat = cardData.ToArray();

            for (int i = 0; i < cDat.Length; i++) {
                switch (cDat[i]) {
                    case "Attribute":
                    this.monDat.attribute = cDat[i + 1]; i++; break;
                    case "Level":
                    this.monDat.level = int.Parse(cDat[i + 1]); i++; break;
                    case "Rank":
                    this.monDat.rank = int.Parse(cDat[i + 1]); i++; break;
                    case "Link":
                    this.monDat.link = int.Parse(cDat[i + 1]); i++; break;
                    case "Pendulum Scale":
                    this.monDat.scale = int.Parse(cDat[i + 1]); i++; break;
                    case "Monster Type":
                    this.monDat.mType = cDat[i + 1]; i++; break;
                    case "Pendulum Effect":
                    this.monDat.pEffect = cDat[i + 1]; break;
                    case "ATK":
                    if (cDat[i + 1].Equals("-") || cDat[i + 1].Equals("?")) this.monDat.def = 0;
                    else this.monDat.atk = int.Parse(cDat[i + 1]);
                    i++; break;
                    case "DEF":
                    if (cDat[i + 1].Equals("-") || cDat[i + 1].Equals("?")) this.monDat.def = 0;
                    else this.monDat.def = int.Parse(cDat[i + 1]);
                    i++; break;
                    case "Card Type":
                    while (!cDat[i].Equals("ATK")) {
                        if (!cDat[i].Contains(" ") && !cDat[i].Contains("/")) { this.cardType.Add(cDat[i]); }
                        i++;
                    }
                    i--; break;
                }
            }
        }
        // ---------------- ---------------- End of Class---------------- ---------------- //
    }
}
