using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YuGiDough {
    public partial class deckSearch : Form {
        public Deck dts;
        public ImageList IL;
        public Size bsize;
        public deckSearch(Deck toSearch) {
            InitializeComponent();
            IL = new ImageList();
            bsize = new Size(120, 177);
            IL.ImageSize = bsize;
            this.dts = toSearch;
            foreach (Card cd in this.dts.cardList) {
                if (!IL.Images.ContainsKey(cd.dataBaseID.ToString())) IL.Images.Add(
                    cd.dataBaseID.ToString(), new Bitmap(Image.FromFile(cd.imgLink), bsize));
                ListViewItem lvi = new ListViewItem();
                lvi.Tag = cd;
                lvi.ImageKey = cd.dataBaseID.ToString();
                displayList.Items.Add(lvi);
            }
            displayList.LargeImageList = IL;
        }

        private void SearchButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void DisplayList_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                Field.searchCard = this.dts.removeByIndex(displayList.SelectedIndices[0]);
            }
            catch (Exception) { }
        }
    }
}
