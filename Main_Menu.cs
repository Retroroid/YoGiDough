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
    public partial class Main_Menu : Form {
        public Main_Menu() {
            InitializeComponent();
        }

        private void MmbDatabase_Click(object sender, EventArgs e) {
            CardView cv = new CardView();
            DialogResult dr = cv.ShowDialog();
        }

        private void MmbField_Click(object sender, EventArgs e) {
            Field fd = new Field();
            DialogResult dr = fd.ShowDialog();
        }
    }
}
