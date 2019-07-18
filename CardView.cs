using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace YuGiDough {
    public partial class CardView : Form {
        // ---------------- ---------------- Class Variables ---------------- ---------------- //
        public string basePath = "C:\\Users\\lucask\\ygodough\\ydata\\";
        public Card currentCard;
        public Image currentImage;
        // ---------------- ---------------- Form Initialization ---------------- ---------------- //
        public CardView() {
            InitializeComponent();
            // Initialize card list
            listBox1.DisplayMember = "name";
            DirectoryInfo DI = new DirectoryInfo(Card.basePath);
            foreach (var file in DI.GetFiles("*.txt")) {
                currentCard = Card.loadFromFile(file.FullName);
                listBox1.Items.Add(currentCard);
            }
            // Initialize monster search panel
            msoTypeBox.Items.AddRange(Card.monsterTypes);
            soResultCount.Text = listBox1.Items.Count + " Results";
            msoTypeBox.SelectedIndex = 0;
        } // End constructor
        //
        // ---------------- ---------------- Picture / Card List Control ---------------- ---------------- //
        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e) {
            currentCard = (Card)listBox1.SelectedItem;
            labelCardName.Text = currentCard.name;
            string cardname = currentCard.name;
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
            try {
                currentImage = Image.FromFile(basePath + cardname + ".jpg");
            }
            catch (Exception) {
                try {
                    cardname = cardname.Replace("-", "_");
                    cardname = cardname.Replace("#", string.Empty);
                    cardname = cardname.Replace("!", "_");
                    currentImage = Image.FromFile(basePath + cardname + ".jpg");
                }
                catch (Exception) {
                    try {
                        cardname = cardname.Replace(".", "_");
                        cardname = cardname.Replace("\'", "%27");
                        currentImage = Image.FromFile(basePath + cardname + ".jpg");
                    }
                    catch (Exception) {
                        string partialName = cardname;
                        DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(basePath);
                        FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*" + partialName + "*.*");

                        foreach (FileInfo foundFile in filesInDir) {
                            System.IO.File.Move(foundFile.FullName, foundFile.FullName.Replace("ydata", "Problem Cards"));
                        }
                    }
                }
            }
            pictureBox1.Image = currentImage;
            textBox1.Text = currentCard.cardText;

            switch (currentCard.MST) {
                case "Monster":
                textBox2.Text = currentCard.monDat.attribute;
                //no plants
                pictureBox2.Image = Image.FromFile("C:\\Users\\lucask\\ygodough\\visuals\\" + currentCard.monDat.mType + ".png");
                textBox3.Text = currentCard.monDat.mType;
                pictureBox3.Image = Image.FromFile("C:\\Users\\lucask\\ygodough\\visuals\\" + currentCard.monDat.attribute + ".png");
                break;

                default:
                boxLink.Hide(); labelLink.Hide();
                boxLevel.Hide(); labelLevel.Hide();
                boxScale.Hide(); labelScale.Hide();
                textBox2.Text = currentCard.MST.ToUpper();
                pictureBox2.Image = Image.FromFile("C:\\Users\\lucask\\ygodough\\visuals\\" + currentCard.cardType[0] + ".png");
                textBox3.Text = currentCard.cardType[0];
                pictureBox3.Image = Image.FromFile("C:\\Users\\lucask\\ygodough\\visuals\\" + currentCard.MST.ToUpper() + ".png");
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
            DirectoryInfo DI = new DirectoryInfo(basePath.TrimEnd('\\'));
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
                    if (!currentCard.MST.Equals("Monster")) addToList = false;
                    else if (!msoBoxAtk.Text.Equals(string.Empty)) {
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
        private void Panel1_Paint(object sender, PaintEventArgs e) {

        }

        private void SoMonster_Enter(object sender, EventArgs e) {

        }

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

        private void Label2_Click(object sender, EventArgs e) {

        }
        // ---------------- ---------------- ---------------- Search Options ---------------- ---------------- ---------------- //


        // ---------------- ---------------- ---------------- ---------------- ---------------- ---------------- //
    } // End of Class
} // End of Namespace
