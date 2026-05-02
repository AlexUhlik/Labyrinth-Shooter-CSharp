namespace LabyrinthGame
{
    partial class StartForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblPlayer1Info = new System.Windows.Forms.Label();
            this.lblPlayer2Info = new System.Windows.Forms.Label();
            this.picControlsP2 = new System.Windows.Forms.PictureBox();
            this.picControlsP1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picControlsP2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picControlsP1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("ROG Fonts STRIX SCAR", 48.22641F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTitle.Location = new System.Drawing.Point(12, 71);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(822, 86);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Labyrinth Game";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.btnStart.Font = new System.Drawing.Font("ROG Fonts STRIX SCAR", 16.30189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(263, 197);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(270, 44);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.btnExit.Font = new System.Drawing.Font("ROG Fonts STRIX SCAR", 16.30189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(263, 247);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(270, 44);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblPlayer1Info
            // 
            this.lblPlayer1Info.AutoSize = true;
            this.lblPlayer1Info.Font = new System.Drawing.Font("ROG Fonts STRIX SCAR", 16.30189F);
            this.lblPlayer1Info.ForeColor = System.Drawing.SystemColors.Control;
            this.lblPlayer1Info.Location = new System.Drawing.Point(89, 197);
            this.lblPlayer1Info.Name = "lblPlayer1Info";
            this.lblPlayer1Info.Size = new System.Drawing.Size(159, 30);
            this.lblPlayer1Info.TabIndex = 4;
            this.lblPlayer1Info.Text = "Player 1";
            // 
            // lblPlayer2Info
            // 
            this.lblPlayer2Info.AutoSize = true;
            this.lblPlayer2Info.Font = new System.Drawing.Font("ROG Fonts STRIX SCAR", 16.30189F);
            this.lblPlayer2Info.ForeColor = System.Drawing.SystemColors.Control;
            this.lblPlayer2Info.Location = new System.Drawing.Point(585, 205);
            this.lblPlayer2Info.Name = "lblPlayer2Info";
            this.lblPlayer2Info.Size = new System.Drawing.Size(168, 30);
            this.lblPlayer2Info.TabIndex = 6;
            this.lblPlayer2Info.Text = "Player 2";
            // 
            // picControlsP2
            // 
            this.picControlsP2.BackColor = System.Drawing.Color.Transparent;
            this.picControlsP2.Image = global::LabyrinthGame.Properties.Resources.KeysNew2_3;
            this.picControlsP2.Location = new System.Drawing.Point(524, 233);
            this.picControlsP2.Name = "picControlsP2";
            this.picControlsP2.Size = new System.Drawing.Size(264, 177);
            this.picControlsP2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picControlsP2.TabIndex = 5;
            this.picControlsP2.TabStop = false;
            // 
            // picControlsP1
            // 
            this.picControlsP1.BackColor = System.Drawing.Color.Transparent;
            this.picControlsP1.Image = global::LabyrinthGame.Properties.Resources.KeysNew;
            this.picControlsP1.Location = new System.Drawing.Point(12, 233);
            this.picControlsP1.Name = "picControlsP1";
            this.picControlsP1.Size = new System.Drawing.Size(264, 177);
            this.picControlsP1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picControlsP1.TabIndex = 3;
            this.picControlsP1.TabStop = false;
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblPlayer2Info);
            this.Controls.Add(this.picControlsP2);
            this.Controls.Add(this.lblPlayer1Info);
            this.Controls.Add(this.picControlsP1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblTitle);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Name = "StartForm";
            this.Text = "StartForm";
            ((System.ComponentModel.ISupportInitialize)(this.picControlsP2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picControlsP1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.PictureBox picControlsP1;
        private System.Windows.Forms.Label lblPlayer1Info;
        private System.Windows.Forms.PictureBox picControlsP2;
        private System.Windows.Forms.Label lblPlayer2Info;
    }
}