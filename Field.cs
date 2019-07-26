using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace YuGiDough {
    public partial class Field : Form {
        public Card copyCard;
        public Image viewedImage;
        DirectoryInfo DI;
        public string direc;
        public ImageList IL;
        public Image cardBack;
        public PictureBox memoryBank;
        public ListViewItem handBank;
        public Size[] baseSize;
        public bool cardFromDeck;
        public Random rng;
        public static Card searchCard;
        public static int indexer;

        public Deck deckRed;
        public Deck GYRed;
        public Deck EDRed;
        public Deck banRed;

        public Deck deckBlue;
        public Deck GYBlue;
        public Deck EDBlue;
        public Deck banBlue;

        public Field() {
            Random rng = new Random(Guid.NewGuid().GetHashCode());
            cardFromDeck = false;
            this.DI = new DirectoryInfo(Card.basePath + "\\ydata");
            direc = DI.FullName + "\\";
            IL = new ImageList();
            IL.ImageSize = new Size(83, 120);
            baseSize = new Size[3];
            baseSize[0] = new Size(81, 120);
            cardBack = new Bitmap(Image.FromFile(Card.basePath + "\\visuals\\cardBack.png"), baseSize[0]);
            InitializeComponent();
            handDepositRed.Tag = handRed;
            handDepositBlue.Tag = handBlue;
            zoneDeckRed.Image = cardBack;
            zoneDeckBlue.Image = cardBack;


            deckRed = new Deck(new Random(Guid.NewGuid().GetHashCode())); deckBlue = new Deck(new Random(Guid.NewGuid().GetHashCode()));
            GYRed = new Deck(new Random(Guid.NewGuid().GetHashCode())); GYBlue = new Deck(new Random(Guid.NewGuid().GetHashCode()));
            EDRed = new Deck(new Random(Guid.NewGuid().GetHashCode())); EDBlue = new Deck(new Random(Guid.NewGuid().GetHashCode()));
            banRed = new Deck(new Random(Guid.NewGuid().GetHashCode())); banBlue = new Deck(new Random(Guid.NewGuid().GetHashCode()));
            ZoneGYRed.Tag = GYRed; ZoneGYBlue.Tag = GYBlue;
            zoneExtraRed.Tag = EDRed; zoneExtraBlue.Tag = EDBlue;
            zoneBanishedRed.Tag = banRed; zoneBanishedBlue.Tag = banBlue;
            memoryBank = new PictureBox();
            memoryBank.Tag = null;
            memoryBank.Image = null;

            copyCard = Card.loadFromFile(direc + "Substitoad" + ".txt");
            placeCard(copyCard, zoneMonsterRed4);
            copyCard = Card.loadFromFile(direc + "Summon_Sorceress" + ".txt");
            placeCard(copyCard, zoneLinkWest);
            copyCard = Card.loadFromFile(direc + "Machine_Angel_Ritual.txt");
            placeCard(copyCard, zoneSTBlue1);
            copyCard = Card.loadFromFile(direc + "Supreme_King_Z_ARC.txt");
            placeCard(copyCard, zoneLinkEast);
            viewCard(copyCard);
            zoneDeckRed.Tag = Deck.loadDeckFromFile("Nekrozma");
            ((Deck)zoneDeckRed.Tag).rng = new Random(Guid.NewGuid().GetHashCode());
            zoneDeckBlue.Tag = Deck.loadDeckFromFile("Freg");
            ((Deck)zoneDeckBlue.Tag).rng = new Random(Guid.NewGuid().GetHashCode());
            countDeckRed.Text = ((Deck)zoneDeckRed.Tag).cardCount().ToString();
            countDeckBlue.Text = ((Deck)zoneDeckBlue.Tag).cardCount().ToString();
        }
        
        public void viewCard(Card toView) {
            setNullDetails();
            // ---------------- Retrieve Image ---------------- //
            string cardname = toView.name;
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
                viewedImage = Image.FromFile(direc + cardname + ".jpg");
            }
            catch (Exception) {
                try {
                    cardname = cardname.Replace("-", "_");
                    cardname = cardname.Replace("#", string.Empty);
                    cardname = cardname.Replace("!", "_");
                    viewedImage = Image.FromFile(direc + cardname + ".jpg");
                }
                catch (Exception) {
                    try {
                        cardname = cardname.Replace(".", "_");
                        cardname = cardname.Replace("\'", "%27");
                        viewedImage = Image.FromFile(direc + cardname + ".jpg");
                    }
                    catch (Exception) { }
                }
            }
            toView.imgLink = direc + cardname + ".jpg";
            cardDetailDisplay.Image = viewedImage;
            // ---------------- Image Retrieved ---------------- //
            cardDetailName.Text = toView.name;
            cardDetailType.Text = string.Empty;

            switch (toView.MST) {
                case "Monster":
                cardDetailType.Text = toView.monDat.mType;
                cardDetailLabelLevel.Enabled = true;
                cardDetailLevel.Enabled = true;
                cardDetailLevel.Text = toView.monDat.level.ToString();
                foreach (string st in toView.cardType) {
                    cardDetailType.Text += (" / " + st);
                    if (st.Equals("Link")) {
                        cardDetailLabelLink.Enabled = true;
                        cardDetailLink.Enabled = true;
                        cardDetailLink.Text = toView.monDat.link.ToString();
                        cardDetailLabelLevel.Enabled = false;
                        cardDetailLevel.Enabled = false;
                    }
                    else if (st.Equals("Pendulum")) {
                        cardDetailLabelScale.Enabled = true;
                        cardDetailScale.Enabled = true;
                        cardDetailScale.Text = toView.monDat.scale.ToString();
                        cardDetailTextPendulum.Enabled = true;
                        cardDetailTextPendulum.Text = toView.monDat.pEffect;
                    }
                    else if (st.Equals("Xyz")) { cardDetailLevel.Text = toView.monDat.rank.ToString(); }
                    string[] toText = new string[3];
                    toText[0] = toView.monDat.attribute;
                    toText[1] = toView.monDat.mType;
                    foreach (string stype in toView.cardType) {
                        toText[1] += " / " + stype;
                    }
                    toText[2] = toView.cardText;
                    cardDetailText.Lines = toText;
                }
                break;

                default:
                cardDetailText.Text = toView.cardText;
                cardDetailType.Text = toView.cardType[0] + " " + toView.MST;
                break;
            }
        }
        private void setNullDetails() {
            cardDetailName.Text = "Card Name";
            cardDetailType.Text = string.Empty;

            cardDetailLabelLevel.Enabled = false;
            cardDetailLabelScale.Enabled = false;
            cardDetailLabelLink.Enabled = false;
            cardDetailLevel.Enabled = false;
            cardDetailScale.Enabled = false;
            cardDetailLink.Enabled = false;
            cardDetailLevel.Text = string.Empty;
            cardDetailScale.Text = string.Empty;
            cardDetailLink.Text = string.Empty;

            cardDetailText.Text = "Card Details";
            cardDetailTextPendulum.Text = string.Empty;
            cardDetailTextPendulum.Enabled = false;
        }
        // ---------------- ---------------- // Card Mobility // ---------------- ---------------- //
        // ---------------- ---------------- ---------------- ---------------- ---------------- //
        // ---------------- Moving Cards Around ---------------- //
        private void addToDeck(object sender, EventArgs e) {
            if (memoryBank.Tag == null) return;
            ToolStripMenuItem snd = (ToolStripMenuItem)sender;
            ContextMenuStrip kk = (ContextMenuStrip)snd.Owner;
            PictureBox pb = (PictureBox)kk.SourceControl;
            Deck dk = (Deck)pb.Tag;
            Card cd = (Card)memoryBank.Tag;
            dk.addCard(cd);
            nullBank();
            decksUpdateImages();
        }
        public void placeCard(PictureBox placeToPlace) {
            // This one is for moving cards between zones
            placeToPlace.Tag = (Card)memoryBank.Tag;
            placeToPlace.Image = memoryBank.Image;
            memoryBank.Cursor = Cursors.Arrow;
            nullBank();
        }
        public void placeCard(Card toPlace, PictureBox placeToPlace) {
            // This one is for placing cards initially.
            try {
                placeToPlace.Image = new Bitmap(Image.FromFile(toPlace.imgLink), new Size(81, 120));
                placeToPlace.Tag = toPlace;
                memoryBank.Cursor = Cursors.Arrow;
                nullBank();
            }
            catch (Exception) { }
        }
        private void Lclick(object sender, EventArgs e) {
            // Moving cards around the field
            try {
                PictureBox pb = (PictureBox)sender;
                if (pb.Tag != null && (memoryBank == null || memoryBank.Tag == null)) {
                    memoryBank = pb;
                    pb.Cursor = Cursors.NoMove2D;
                    viewCard((Card)pb.Tag);
                }
                else if (pb.Tag == null && memoryBank.Tag != null) {
                    placeCard(pb);
                    cardFromDeck = false;
                    nullBank();
                }
                else if (pb.Tag != null && memoryBank.Tag != null && !cardFromDeck) {
                    memoryBank.Cursor = Cursors.Arrow;
                    memoryBank = new PictureBox();
                    pb.Cursor = Cursors.Arrow;
                }
            }
            catch (Exception f) { MessageBox.Show(f.Message); }
        }
        private void addToHandFromMemory(object sender, EventArgs e) {
            PictureBox pb = (PictureBox)sender;
            ListView hand = (ListView)pb.Tag;
            Card toAdd = (Card)memoryBank.Tag;
            addToHand(toAdd, hand);
            nullBank();
        }
        private void addToHand(Card cardToAdd, ListView hand) {
            if (memoryBank.Tag != null) {
                if (!IL.Images.ContainsKey(cardToAdd.dataBaseID.ToString())) IL.Images.Add(cardToAdd.dataBaseID.ToString(), Image.FromFile(cardToAdd.imgLink));
                ListViewItem newItem = new ListViewItem();
                newItem.ImageKey = cardToAdd.dataBaseID.ToString();
                newItem.Tag = cardToAdd;
                hand.LargeImageList = IL;
                hand.Items.Add(newItem);
                hand.Refresh();
            }
        }
        private void flipCard(object sender, EventArgs e) {
            ToolStripMenuItem snd = (ToolStripMenuItem) sender;
            ContextMenuStrip kk = (ContextMenuStrip) snd.Owner;
            PictureBox pb = (PictureBox)kk.SourceControl;
            if (!pb.Tag.Equals(null)) {
                Card ct = (Card)pb.Tag;
                if (ct.flipped) {
                    pb.Image = new Bitmap(Image.FromFile(ct.imgLink), baseSize[0]);
                    ct.flipped = false;
                    if (ct.rotated) pb.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                else {
                    pb.Image = new Bitmap(cardBack);
                    ct.flipped = true;
                    if (ct.rotated) pb.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                pb.Refresh();
            }
        }
        private void shiftCard(object sender, EventArgs e) {
            ToolStripMenuItem snd = (ToolStripMenuItem)sender;
            ContextMenuStrip kk = (ContextMenuStrip)snd.Owner;
            PictureBox pb = (PictureBox)kk.SourceControl;
            Card ct = (Card)pb.Tag;
            if (ct.rotated) {
                pb.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                ct.rotated = false;
            }
            else {
                pb.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                ct.rotated = true;
            }
            pb.Refresh();
        }
        private void Hand_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (memoryBank.Tag == null) {
                    ListView snd = (ListView)sender;
                    copyCard = (Card)(snd.Items[snd.SelectedIndices[0]].Tag);
                    viewedImage = IL.Images[copyCard.dataBaseID.ToString()];
                    snd.Items.RemoveAt(snd.SelectedIndices[0]);
                    viewCard(copyCard);
                    memoryBank = new PictureBox();
                    memoryBank.Tag = copyCard;
                    memoryBank.Image = viewedImage; 
                }
            }
            catch (Exception) { }
        }
        private void nullBank() {
            memoryBank.Tag = null;
            memoryBank.Image = null;
            memoryBank = new PictureBox();
        }
        private void updateCounts() {
            countDeckRed.Text = ((Deck)zoneDeckRed.Tag).cardCount();
            countDeckBlue.Text = ((Deck)zoneDeckBlue.Tag).cardCount();
            countGYRed.Text = ((Deck)ZoneGYRed.Tag).cardCount();
            countGYBlue.Text = ((Deck)ZoneGYBlue.Tag).cardCount();
            countBanRed.Text = ((Deck)zoneBanishedRed.Tag).cardCount();
            countBanBlue.Text = ((Deck)zoneBanishedBlue.Tag).cardCount();
            countEDRed.Text = ((Deck)zoneExtraRed.Tag).cardCount();
            countEDBlue.Text = ((Deck)zoneExtraBlue.Tag).cardCount();
        }
        private void Field_Load(object sender, EventArgs e) {

        }
        private void doDraw(object sender, EventArgs e) {
            if (memoryBank.Tag == null) {
                ToolStripMenuItem snd = (ToolStripMenuItem)sender;
                ContextMenuStrip kk = (ContextMenuStrip)snd.Owner;
                PictureBox pb = (PictureBox)kk.SourceControl;
                Deck dk = (Deck)pb.Tag;
                Card cd = dk.drawCard();
                memoryBank.Tag = cd;
                memoryBank.Image = new Bitmap(Image.FromFile(cd.imgLink), baseSize[0]);
                cardFromDeck = true;
                viewCard(cd);
                decksUpdateImages();
            }
        }
        private void doSearch(object sender, EventArgs e) {
            ToolStripMenuItem snd = (ToolStripMenuItem)sender;
            ContextMenuStrip kk = (ContextMenuStrip)snd.Owner;
            PictureBox pb = (PictureBox)kk.SourceControl;
            Deck dk = (Deck)pb.Tag;
            deckSearch searcher = new deckSearch(dk);
            DialogResult dr = searcher.ShowDialog();
            // 120 x 177
            if (searchCard != null) {
                dk.shuffle();
                memoryBank.Tag = searchCard;
                memoryBank.Image = new Bitmap(Image.FromFile(searchCard.imgLink), baseSize[0]);
                cardFromDeck = true;
                viewCard(searchCard);
                searchCard = null;
                decksUpdateImages();
            }
        }
        private void doShuffle(object sender, EventArgs e) {
            ToolStripMenuItem snd = (ToolStripMenuItem)sender;
            ContextMenuStrip kk = (ContextMenuStrip)snd.Owner;
            PictureBox pb = (PictureBox)kk.SourceControl;
            Deck dk = (Deck)pb.Tag;
            dk.shuffle();
        }
        private void deckLclick(object sender, EventArgs e) {
            if (memoryBank.Tag == null) return;
            PictureBox pb = (PictureBox)sender;
            Deck dk = (Deck)pb.Tag;
            Card cd = (Card)memoryBank.Tag;
            dk.addCard(cd);
            nullBank();
            decksUpdateImages();
        }
        private void decksUpdateImages() {
            updateCounts();
            if (((Deck)ZoneGYRed.Tag).cardList.Count > 0)
                ZoneGYRed.Image = new Bitmap(Image.FromFile(((Deck)ZoneGYRed.Tag).getFaceUpCard().imgLink), baseSize[0]);
            else ZoneGYRed.Image = null;
            if (((Deck)ZoneGYBlue.Tag).cardList.Count > 0)
                ZoneGYBlue.Image = new Bitmap(Image.FromFile(((Deck)ZoneGYBlue.Tag).getFaceUpCard().imgLink), baseSize[0]);
            else ZoneGYBlue.Image = null;
            if (((Deck)zoneBanishedRed.Tag).cardList.Count > 0)
                zoneBanishedRed.Image = new Bitmap(Image.FromFile(((Deck)zoneBanishedRed.Tag).getFaceUpCard().imgLink), baseSize[0]);
            else zoneBanishedRed.Image = null;
            if (((Deck)zoneBanishedBlue.Tag).cardList.Count > 0)
                zoneBanishedBlue.Image = new Bitmap(Image.FromFile(((Deck)zoneBanishedBlue.Tag).getFaceUpCard().imgLink), baseSize[0]);
            else zoneBanishedBlue.Image = null;

            if (((Deck)zoneExtraRed.Tag).cardList.Count > 0)
                zoneExtraRed.Image = cardBack;
            else zoneExtraRed.Image = null;
            if (((Deck)zoneExtraBlue.Tag).cardList.Count > 0)
                zoneExtraBlue.Image = cardBack;
            else zoneExtraBlue.Image = null;
        }
    } // End of class
} // End of namespace
