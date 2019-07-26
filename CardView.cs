using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace YuGiDough {
    public partial class CardView : Form {
        // ---------------- ---------------- Class Variables ---------------- ---------------- //
        public string basePath;
        public Card currentCard;
        public Image currentImage;
        public string vimages, vcards;
        // ---------------- ---------------- Form Initialization ---------------- ---------------- //
        public CardView() {
            InitializeComponent();
            this.basePath = Card.basePath;
            vcards = basePath + "\\ydata\\";
            // Initialize card list
            listBox1.DisplayMember = "name";
            DirectoryInfo DI = new DirectoryInfo(vcards.TrimEnd('\\'));
            foreach (var file in DI.GetFiles("*.txt")) {
                currentCard = Card.loadFromFile(file.FullName);
                listBox1.Items.Add(currentCard);
            }
            // Other ini stuff
            labelLink.Hide();
            labelLevel.Hide();
            labelScale.Hide();
            DirectoryInfo DZ = new DirectoryInfo(basePath + "\\Deck");
            foreach(var file in DZ.GetFiles()) {
                allDecks.Items.Add(file.Name);
            }

            // Initialize monster search panel
            msoTypeBox.Items.AddRange(Card.monsterTypes);
            soResultCount.Text = listBox1.Items.Count + " Results";
            msoTypeBox.SelectedIndex = 0;
            
            //Images
            vimages = (basePath + "\\visuals\\");
            stsoTypeAny.Image = Image.FromFile(vimages + "SPELLTRAP.png");
            stsoTypeSpell.Image = Image.FromFile(vimages + "SPELL.jpg");
            stsoTypeTrap.Image = Image.FromFile(vimages + "TRAP.jpg");
            stsoIconAny.Image = Image.FromFile(vimages + "anyicon.png");
            stsoIconNormal.Image = Image.FromFile(vimages + "Normal.png");
            stsoIconQuickPlay.Image = Image.FromFile(vimages + "Quick-Play.png");
            stsoIconContinuous.Image = Image.FromFile(vimages + "Continuous.png");
            stsoIconEquip.Image = Image.FromFile(vimages + "Equip.png");
            stsoIconField.Image = Image.FromFile(vimages + "Field.png");
            stsoIconRitual.Image = Image.FromFile(vimages + "Ritual.png");
            stsoIconCounter.Image = Image.FromFile(vimages + "Counter.png");

            msoAttAny.Image = Image.FromFile(vimages + "anyattribute.png");
            msoAttEarth.Image = Image.FromFile(vimages + "EARTH.png");
            msoAttWind.Image = Image.FromFile(vimages + "WIND.png");
            msoAttWater.Image = Image.FromFile(vimages + "WATER.png");
            msoAttFire.Image = Image.FromFile(vimages + "FIRE.png");
            msoAttDark.Image = Image.FromFile(vimages + "DARK.png");
            msoAttLight.Image = Image.FromFile(vimages + "LIGHT.png");
            msoAttDivine.Image = Image.FromFile(vimages + "Divine-Beastie.png");

            pictureBox4.Image = Image.FromFile(vimages + "Coming Soon.png");

        } // End constructor
        //
        // ---------------- ---------------- Picture / Card List Control ---------------- ---------------- //
        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e) {
            currentCard = (Card)listBox1.SelectedItem;
            try {
                labelCardName.Text = currentCard.name;
            }
            catch (Exception) {
                listBox1.SelectedIndex = 0;
                currentCard = (Card)listBox1.SelectedItem;
                labelCardName.Text = currentCard.name;
            }
            pictureBox1.Image = Image.FromFile(currentCard.imgLink);
            textBox1.Text = currentCard.cardText;

            switch (currentCard.MST) {
                case "Monster":
                textBox2.Text = currentCard.monDat.attribute;
                //no plants
                pictureBox2.Image = Image.FromFile(vimages + currentCard.monDat.mType + ".png");
                textBox3.Text = currentCard.monDat.mType;
                pictureBox3.Image = Image.FromFile(vimages + currentCard.monDat.attribute + ".png");
                break;

                default:
                boxLink.Hide(); labelLink.Hide();
                boxLevel.Hide(); labelLevel.Hide();
                boxScale.Hide(); labelScale.Hide();
                textBox2.Text = currentCard.MST.ToUpper();
                pictureBox2.Image = Image.FromFile(vimages + currentCard.cardType[0] + ".png");
                textBox3.Text = currentCard.cardType[0];
                pictureBox3.Image = Image.FromFile(vimages + currentCard.MST.ToUpper() + ".png");
                break;
            } // End switch
            if (currentCard.monDat.pEffect != null) {
                textBox4.Text = currentCard.monDat.pEffect;
            }
            string typeString = "";
            foreach (string s in currentCard.cardType) {
                if (typeString.Equals("")) typeString = s;
                else typeString = typeString + " / " + s;
            }
            textBox5.Text = typeString;

            if (currentCard.monDat.link == 0) { labelLink.Hide(); boxLink.Hide(); }
            else {
                boxLink.Text = currentCard.monDat.link.ToString();
                labelLink.Show();
                boxLink.Show();
            }
            if (currentCard.monDat.scale == 0) { textBox4.Hide(); labelScale.Hide(); boxScale.Hide(); }
            else {
                boxScale.Text = currentCard.monDat.scale.ToString();
                boxScale.Show();
                labelScale.Show();
                textBox4.Show();
            }
            if (currentCard.monDat.level == 0) { boxLevel.Hide(); labelLevel.Hide(); }
            else {
                boxLevel.Text = currentCard.monDat.level.ToString();
                boxLevel.Show();
                labelLevel.Show();
            }
        } // End Card View select
        //
        // ---------------- ---------------- Search Button ---------------- ---------------- //
        private void Button1_Click_1(object sender, EventArgs e) {
            // Clear the current list, create a new DI, and create space for a bool variable.
            listBox1.Items.Clear();
            DirectoryInfo DI = new DirectoryInfo(vcards.TrimEnd('\\'));
            bool addToList;

            // Check each card in the file directory. Begin by setting the flag to true; it is easier to prove something is false than true.
            foreach (var file in DI.GetFiles("*.txt")) {
                addToList = true;

                // Seach by text
                currentCard = Card.loadFromFile(file.FullName);
                if (!(currentCard.StringToString().IndexOf(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0) && !searchTextBox.Text.Equals(string.Empty)) addToList = false;

                if (soCardType.SelectedIndex == 1 && !currentCard.MST.Equals("Monster")) addToList = false;
                if (soCardType.SelectedIndex == 2 && currentCard.MST.Equals("Monster")) addToList = false;

                // Monster check
                if (soCardType.SelectedIndex != 2) {
                    // Type check
                    if (msoTypeBox.SelectedIndex != 0) {
                        if (!currentCard.monDat.mType.Equals(msoTypeBox.SelectedItem.ToString())) addToList = false;
                    }

                    // Attribute Check
                    if (msoAttAny.Checked != true) {
                        if (msoAttEarth.Checked == true && !currentCard.monDat.attribute.Equals("EARTH")) addToList = false;
                        else if (msoAttDark.Checked == true && !currentCard.monDat.attribute.Equals("DARK")) addToList = false;
                        else if (msoAttLight.Checked == true && !currentCard.monDat.attribute.Equals("LIGHT")) addToList = false;
                        else if (msoAttWater.Checked == true && !currentCard.monDat.attribute.Equals("WATER")) addToList = false;
                        else if (msoAttFire.Checked == true && !currentCard.monDat.attribute.Equals("FIRE")) addToList = false;
                        else if (msoAttWind.Checked == true && !currentCard.monDat.attribute.Equals("WIND")) addToList = false;
                        else if (msoAttDivine.Checked == true && !currentCard.monDat.attribute.Equals("DIVINE")) addToList = false;
                    }

                    // Sub-Type check
                    bool flag;
                    foreach(ListViewItem itm in msoSubType.Items) {
                        flag = true;
                        if (itm.Checked == true) {
                            flag = false;
                            if (!currentCard.MST.Equals("Monster")) addToList = false;
                            foreach(string gst in currentCard.cardType) {
                                if (gst.Equals(itm.Text)) flag = true;
                            }
                        }
                        if (!flag) addToList = false;
                    }
                    
                    // Level/Rank/Link check
                    if (!msoBoxLevelRankLink.Text.Equals(string.Empty)) {
                        if (!currentCard.MST.Equals("Monster")) addToList = false;
                        else if (currentCard.monDat.level != 0) {
                            if (msoLRLLess.Checked != true && currentCard.monDat.level < int.Parse(msoBoxLevelRankLink.Text)) addToList = false;
                            if (msoLRLEqual.Checked != true && currentCard.monDat.level == int.Parse(msoBoxLevelRankLink.Text)) addToList = false;
                            if (msoLRLGreat.Checked != true && currentCard.monDat.level > int.Parse(msoBoxLevelRankLink.Text)) addToList = false;
                        }
                        else if (currentCard.monDat.rank != 0) {
                            if (msoLRLLess.Checked != true && currentCard.monDat.rank < int.Parse(msoBoxLevelRankLink.Text)) addToList = false;
                            if (msoLRLEqual.Checked != true && currentCard.monDat.rank == int.Parse(msoBoxLevelRankLink.Text)) addToList = false;
                            if (msoLRLGreat.Checked != true && currentCard.monDat.rank > int.Parse(msoBoxLevelRankLink.Text)) addToList = false;
                        }
                        else if (currentCard.monDat.link != 0) {
                            if (msoLRLLess.Checked != true && currentCard.monDat.link < int.Parse(msoBoxLevelRankLink.Text)) addToList = false;
                            if (msoLRLEqual.Checked != true && currentCard.monDat.link == int.Parse(msoBoxLevelRankLink.Text)) addToList = false;
                            if (msoLRLGreat.Checked != true && currentCard.monDat.link > int.Parse(msoBoxLevelRankLink.Text)) addToList = false;
                        }
                        else if (currentCard.cardText.IndexOf("is always treated as ", StringComparison.OrdinalIgnoreCase) >= 0) {
                            string temp = currentCard.cardText.Substring(currentCard.cardText.IndexOf("is always treated as") + "is always treated as".Length, 2).Trim(' ').Trim('.');
                            int olrl = int.Parse(temp);
                            if (msoLRLLess.Checked != true && olrl < int.Parse(msoBoxLevelRankLink.Text)) addToList = false;
                            if (msoLRLEqual.Checked != true && olrl == int.Parse(msoBoxLevelRankLink.Text)) addToList = false;
                            if (msoLRLGreat.Checked != true && olrl > int.Parse(msoBoxLevelRankLink.Text)) addToList = false;
                        }
                    }

                    // Scale check
                    if (!msoBoxScale.Text.Equals(string.Empty)) {
                        if (msoScaleLess.Checked != true && currentCard.monDat.scale < int.Parse(msoBoxScale.Text)) addToList = false;
                        if (msoScaleEqual.Checked != true && currentCard.monDat.scale == int.Parse(msoBoxScale.Text)) addToList = false;
                        if (msoScaleGreat.Checked != true && currentCard.monDat.scale > int.Parse(msoBoxScale.Text)) addToList = false;
                    }

                    // Atk/Def check
                    if (!msoBoxAtk.Text.Equals(string.Empty)) {
                        if (msoAtkLess.Checked != true && currentCard.monDat.atk < int.Parse(msoBoxAtk.Text)) addToList = false;
                        if (msoAtkEqual.Checked != true && currentCard.monDat.atk == int.Parse(msoBoxAtk.Text)) addToList = false;
                        if (msoAtkGreat.Checked != true && currentCard.monDat.atk > int.Parse(msoBoxAtk.Text)) addToList = false;
                    }
                    else if (!msoBoxDef.Text.Equals(string.Empty)) {
                        if (msoDefLess.Checked != true && currentCard.monDat.def < int.Parse(msoBoxDef.Text)) addToList = false;
                        if (msoDefEqual.Checked != true && currentCard.monDat.def == int.Parse(msoBoxDef.Text)) addToList = false;
                        if (msoDefGreat.Checked != true && currentCard.monDat.def > int.Parse(msoBoxDef.Text)) addToList = false;
                    }
                } // End of Monster check

                // Spell/Trap
                if (soCardType.SelectedIndex != 1) {
                    if (stsoTypeTrap.Checked == true && !currentCard.MST.Equals("Trap")) addToList = false;
                    else if (stsoTypeSpell.Checked == true && !currentCard.MST.Equals("Spell")) addToList = false;

                    if (stsoIconAny.Checked != true) {
                        if (currentCard.MST.Equals("Monster")) addToList = false;
                        else if (stsoIconContinuous.Checked == true && !currentCard.cardType[0].Equals("Continuous")) addToList = false;
                        else if (stsoIconCounter.Checked == true && !currentCard.cardType[0].Equals("Counter")) addToList = false;
                        else if (stsoIconEquip.Checked == true && !currentCard.cardType[0].Equals("Equip")) addToList = false;
                        else if (stsoIconField.Checked == true && !currentCard.cardType[0].Equals("Field")) addToList = false;
                        else if (stsoIconNormal.Checked == true && !currentCard.cardType[0].Equals("Normal")) addToList = false;
                        else if (stsoIconQuickPlay.Checked == true && !currentCard.cardType[0].Equals("Quick-Play")) addToList = false;
                        else if (stsoIconRitual.Checked == true && !currentCard.cardType[0].Equals("Ritual")) addToList = false;
                    }
                } // End of Spell / Trap check

                // If all checks are passed, add the card to the list! Yay!
                if (addToList) listBox1.Items.Add(currentCard);
            } // End of foreach file iteration
            soResultCount.Text = listBox1.Items.Count + " Results";
        } // End of Search Button
          // ---------------- //
        private void Button2_Click(object sender, EventArgs e) {
            soCardType.SelectedIndex = 0;

            stsoIconAny.Checked = true;
            stsoTypeAny.Checked = true;

            msoTypeBox.SelectedIndex = 0;
            msoAttAny.Checked = true;

            msoBoxAtk.Text = string.Empty;
            msoAtkLess.Checked = false;
            msoAtkEqual.Checked = false;
            msoAtkGreat.Checked = false;

            msoBoxDef.Text = string.Empty;
            msoDefLess.Checked = false;
            msoDefEqual.Checked = false;
            msoDefGreat.Checked = false;

            msoBoxLevelRankLink.Text = string.Empty;
            msoLRLLess.Checked = false;
            msoLRLEqual.Checked = false;
            msoLRLGreat.Checked = false;

            msoBoxScale.Text = string.Empty;
            msoScaleLess.Checked = false;
            msoScaleEqual.Checked = false;
            msoScaleGreat.Checked = false;

            foreach (ListViewItem itm in msoSubType.Items) { itm.Checked = false; }
        }

        private void SoCardType_SelectedIndexChanged(object sender, EventArgs e) {
            switch (soCardType.SelectedIndex) {
                case 1:
                soSpellTrap.Enabled = false;
                soMonster.Enabled = true;
                break;

                case 2:
                soSpellTrap.Enabled = true;
                soMonster.Enabled = false;
                break;

                default:
                soSpellTrap.Enabled = true;
                soMonster.Enabled = true;
                break;
            }
        }

        private void DeckBox_SelectedIndexChanged(object sender, EventArgs e) {
            currentCard = (Card)deckBox.SelectedItem;
            try {
                labelCardName.Text = currentCard.name;
            }
            catch (Exception) {
                listBox1.SelectedIndex = 0;
                currentCard = (Card)listBox1.SelectedItem;
                labelCardName.Text = currentCard.name;
            }
            pictureBox1.Image = Image.FromFile(currentCard.imgLink);
            textBox1.Text = currentCard.cardText;

            switch (currentCard.MST) {
                case "Monster":
                textBox2.Text = currentCard.monDat.attribute;
                //no plants
                pictureBox2.Image = Image.FromFile(vimages + currentCard.monDat.mType + ".png");
                textBox3.Text = currentCard.monDat.mType;
                pictureBox3.Image = Image.FromFile(vimages + currentCard.monDat.attribute + ".png");
                break;

                default:
                boxLink.Hide(); labelLink.Hide();
                boxLevel.Hide(); labelLevel.Hide();
                boxScale.Hide(); labelScale.Hide();
                textBox2.Text = currentCard.MST.ToUpper();
                pictureBox2.Image = Image.FromFile(vimages + currentCard.cardType[0] + ".png");
                textBox3.Text = currentCard.cardType[0];
                pictureBox3.Image = Image.FromFile(vimages + currentCard.MST.ToUpper() + ".png");
                break;
            } // End switch
            if (currentCard.monDat.pEffect != null) {
                textBox4.Text = currentCard.monDat.pEffect;
            }
            string typeString = "";
            foreach (string s in currentCard.cardType) {
                if (typeString.Equals("")) typeString = s;
                else typeString = typeString + " / " + s;
            }
            textBox5.Text = typeString;

            if (currentCard.monDat.link == 0) { labelLink.Hide(); boxLink.Hide(); }
            else {
                boxLink.Text = currentCard.monDat.link.ToString();
                labelLink.Show();
                boxLink.Show();
            }
            if (currentCard.monDat.scale == 0) { textBox4.Hide(); labelScale.Hide(); boxScale.Hide(); }
            else {
                boxScale.Text = currentCard.monDat.scale.ToString();
                boxScale.Show();
                labelScale.Show();
                textBox4.Show();
            }
            if (currentCard.monDat.level == 0) { boxLevel.Hide(); labelLevel.Hide(); }
            else {
                boxLevel.Text = currentCard.monDat.level.ToString();
                boxLevel.Show();
                labelLevel.Show();
            }
        } // End Card View select

        private void ButtonRemoveFromDeck_Click(object sender, EventArgs e) {
            try {
                int i = deckBox.SelectedIndex;
                deckBox.Items.RemoveAt(deckBox.SelectedIndex);
                if(i != 0) deckBox.SelectedIndex = i-1;
            }
            catch (Exception) { }
        }

        private void ButtonLoad_Click(object sender, EventArgs e) {
            // save to (basepath + \\Deck\\ + deckName.txt
            deckBox.Items.Clear();
            string[] deckList = File.ReadAllLines(basePath + "\\Deck\\" + textLoad.Text.Replace(".txt",string.Empty) + ".txt");
            foreach (string st in deckList) {
                if (!st.Equals(string.Empty)) {
                    st.Trim('\n');
                    Card temp = Card.loadFromFile(st);
                    deckBox.Items.Add(temp);
                }
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e) {
            Deck newDeck = new Deck();
            foreach(Card cd in deckBox.Items) {
                newDeck.addCard(cd);
            }
            newDeck.deckName = textSave.Text;
            // --------------- //
            if(File.Exists(basePath + "\\Deck\\" + newDeck.deckName + ".txt")) {

            }
            string deckLink = basePath + "\\Deck\\" + newDeck.deckName + ".txt";
            File.WriteAllLines(deckLink, newDeck.printCardList());
            allDecks.Items.Add(newDeck.deckName + ".txt");
            allDecks.Refresh();
        }

        private void AllDecks_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                textLoad.Text = allDecks.SelectedItem.ToString();
            }
            catch (Exception) { }
        }

        private void ButtonDeleteDeck_Click(object sender, EventArgs e) {
            if (allDecks.SelectedIndex >= 0) File.Delete(basePath + "\\Deck\\" + allDecks.Items[allDecks.SelectedIndex]);
        }

        private void ButtonClearDeck_Click(object sender, EventArgs e) { deckBox.Items.Clear(); }

        private void ButtonAddToDeck_Click(object sender, EventArgs e) { deckBox.Items.Add(currentCard); }
        // ---------------- ---------------- ---------------- ---------------- ---------------- ---------------- //
    } // End of Class
} // End of Namespace
