namespace YuGiDough {
    partial class deckSearch {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.displayList = new System.Windows.Forms.ListView();
            this.searchButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // displayList
            // 
            this.displayList.Location = new System.Drawing.Point(12, 12);
            this.displayList.Name = "displayList";
            this.displayList.Size = new System.Drawing.Size(909, 345);
            this.displayList.TabIndex = 0;
            this.displayList.TileSize = new System.Drawing.Size(125, 180);
            this.displayList.UseCompatibleStateImageBehavior = false;
            this.displayList.View = System.Windows.Forms.View.Tile;
            this.displayList.SelectedIndexChanged += new System.EventHandler(this.DisplayList_SelectedIndexChanged);
            // 
            // searchButton
            // 
            this.searchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchButton.Location = new System.Drawing.Point(332, 371);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(256, 54);
            this.searchButton.TabIndex = 1;
            this.searchButton.Text = "Pick Selected Card Up";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // deckSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 437);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.displayList);
            this.Name = "deckSearch";
            this.Text = "deckSearch";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView displayList;
        private System.Windows.Forms.Button searchButton;
    }
}