namespace YuGiDough {
    partial class Main_Menu {
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
            this.mmbDatabase = new System.Windows.Forms.Button();
            this.mmbField = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mmbDatabase
            // 
            this.mmbDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mmbDatabase.Location = new System.Drawing.Point(12, 388);
            this.mmbDatabase.Name = "mmbDatabase";
            this.mmbDatabase.Size = new System.Drawing.Size(128, 64);
            this.mmbDatabase.TabIndex = 0;
            this.mmbDatabase.Text = "Database Tool";
            this.mmbDatabase.UseVisualStyleBackColor = true;
            this.mmbDatabase.Click += new System.EventHandler(this.MmbDatabase_Click);
            // 
            // mmbField
            // 
            this.mmbField.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mmbField.Location = new System.Drawing.Point(394, 388);
            this.mmbField.Name = "mmbField";
            this.mmbField.Size = new System.Drawing.Size(128, 64);
            this.mmbField.TabIndex = 1;
            this.mmbField.Text = "Playmat";
            this.mmbField.UseVisualStyleBackColor = true;
            this.mmbField.Click += new System.EventHandler(this.MmbField_Click);
            // 
            // Main_Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 464);
            this.Controls.Add(this.mmbField);
            this.Controls.Add(this.mmbDatabase);
            this.Name = "Main_Menu";
            this.Text = "Main_Menu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button mmbDatabase;
        private System.Windows.Forms.Button mmbField;
    }
}