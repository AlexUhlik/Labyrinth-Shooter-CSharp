namespace LabyrinthGame
{
    partial class TestWindow
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.GameContainer = new System.Windows.Forms.Panel();
            this.pnlPause = new System.Windows.Forms.Panel();
            this.btnContinue = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.pnlGameOver = new System.Windows.Forms.Panel();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblFinalScore = new System.Windows.Forms.Label();
            this.lblWinner = new System.Windows.Forms.Label();
            this.glControl1 = new OpenTK.GLControl();
            this.Player1Stats = new System.Windows.Forms.Panel();
            this.stats1 = new System.Windows.Forms.TableLayoutPanel();
            this.P1Score = new System.Windows.Forms.Label();
            this.P1Ammo = new System.Windows.Forms.Label();
            this.pbP1Armor = new LabyrinthGame.CustomProgressBar();
            this.pbP1Health = new LabyrinthGame.CustomProgressBar();
            this.Player2Stats = new System.Windows.Forms.Panel();
            this.stats2 = new System.Windows.Forms.TableLayoutPanel();
            this.pbP2Armor = new LabyrinthGame.CustomProgressBar();
            this.pbP2Health = new LabyrinthGame.CustomProgressBar();
            this.P2Score = new System.Windows.Forms.Label();
            this.P2Ammo = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.GameContainer.SuspendLayout();
            this.pnlPause.SuspendLayout();
            this.pnlGameOver.SuspendLayout();
            this.Player1Stats.SuspendLayout();
            this.stats1.SuspendLayout();
            this.Player2Stats.SuspendLayout();
            this.stats2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.GameContainer, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Player1Stats, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Player2Stats, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // GameContainer
            // 
            this.GameContainer.BackColor = System.Drawing.Color.Black;
            this.GameContainer.Controls.Add(this.pnlPause);
            this.GameContainer.Controls.Add(this.pnlGameOver);
            this.GameContainer.Controls.Add(this.glControl1);
            this.GameContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GameContainer.Location = new System.Drawing.Point(160, 0);
            this.GameContainer.Margin = new System.Windows.Forms.Padding(0);
            this.GameContainer.Name = "GameContainer";
            this.GameContainer.Size = new System.Drawing.Size(480, 450);
            this.GameContainer.TabIndex = 1;
            this.GameContainer.Layout += new System.Windows.Forms.LayoutEventHandler(this.GameContainer_Layout);
            // 
            // pnlPause
            // 
            this.pnlPause.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pnlPause.Controls.Add(this.btnContinue);
            this.pnlPause.Controls.Add(this.btnFinish);
            this.pnlPause.Location = new System.Drawing.Point(157, 18);
            this.pnlPause.Name = "pnlPause";
            this.pnlPause.Size = new System.Drawing.Size(165, 89);
            this.pnlPause.TabIndex = 4;
            this.pnlPause.Visible = false;
            // 
            // btnContinue
            // 
            this.btnContinue.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.btnContinue.Font = new System.Drawing.Font("ROG Fonts STRIX SCAR", 8.150944F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContinue.Location = new System.Drawing.Point(24, 0);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(107, 40);
            this.btnContinue.TabIndex = 4;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = false;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click_1);
            // 
            // btnFinish
            // 
            this.btnFinish.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.btnFinish.Font = new System.Drawing.Font("ROG Fonts STRIX SCAR", 8.150944F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinish.Location = new System.Drawing.Point(24, 46);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(107, 40);
            this.btnFinish.TabIndex = 3;
            this.btnFinish.Text = "Finish";
            this.btnFinish.UseVisualStyleBackColor = false;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // pnlGameOver
            // 
            this.pnlGameOver.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pnlGameOver.Controls.Add(this.btnRestart);
            this.pnlGameOver.Controls.Add(this.btnExit);
            this.pnlGameOver.Controls.Add(this.lblFinalScore);
            this.pnlGameOver.Controls.Add(this.lblWinner);
            this.pnlGameOver.Location = new System.Drawing.Point(113, 125);
            this.pnlGameOver.Name = "pnlGameOver";
            this.pnlGameOver.Size = new System.Drawing.Size(249, 234);
            this.pnlGameOver.TabIndex = 1;
            this.pnlGameOver.Visible = false;
            // 
            // btnRestart
            // 
            this.btnRestart.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.btnRestart.Font = new System.Drawing.Font("ROG Fonts STRIX SCAR", 8.150944F);
            this.btnRestart.Location = new System.Drawing.Point(156, 166);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(75, 40);
            this.btnRestart.TabIndex = 3;
            this.btnRestart.Text = "Restart";
            this.btnRestart.UseVisualStyleBackColor = false;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.btnExit.Font = new System.Drawing.Font("ROG Fonts STRIX SCAR", 8.150944F);
            this.btnExit.Location = new System.Drawing.Point(16, 166);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 40);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblFinalScore
            // 
            this.lblFinalScore.AutoSize = true;
            this.lblFinalScore.Font = new System.Drawing.Font("Consolas", 18.33962F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFinalScore.ForeColor = System.Drawing.SystemColors.Control;
            this.lblFinalScore.Location = new System.Drawing.Point(45, 96);
            this.lblFinalScore.Name = "lblFinalScore";
            this.lblFinalScore.Size = new System.Drawing.Size(164, 32);
            this.lblFinalScore.TabIndex = 1;
            this.lblFinalScore.Text = "FinalScore";
            // 
            // lblWinner
            // 
            this.lblWinner.AutoSize = true;
            this.lblWinner.Font = new System.Drawing.Font("Impact", 23.77358F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblWinner.ForeColor = System.Drawing.SystemColors.Control;
            this.lblWinner.Location = new System.Drawing.Point(65, 39);
            this.lblWinner.Name = "lblWinner";
            this.lblWinner.Size = new System.Drawing.Size(124, 43);
            this.lblWinner.TabIndex = 0;
            this.lblWinner.Text = "Winner";
            // 
            // glControl1
            // 
            this.glControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(166, 125);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(150, 150);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            // 
            // Player1Stats
            // 
            this.Player1Stats.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Player1Stats.Controls.Add(this.stats1);
            this.Player1Stats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Player1Stats.Location = new System.Drawing.Point(0, 0);
            this.Player1Stats.Margin = new System.Windows.Forms.Padding(0);
            this.Player1Stats.Name = "Player1Stats";
            this.Player1Stats.Padding = new System.Windows.Forms.Padding(15, 15, 15, 0);
            this.Player1Stats.Size = new System.Drawing.Size(160, 450);
            this.Player1Stats.TabIndex = 1;
            // 
            // stats1
            // 
            this.stats1.ColumnCount = 1;
            this.stats1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.stats1.Controls.Add(this.P1Score, 0, 3);
            this.stats1.Controls.Add(this.P1Ammo, 0, 2);
            this.stats1.Controls.Add(this.pbP1Armor, 0, 0);
            this.stats1.Controls.Add(this.pbP1Health, 0, 1);
            this.stats1.Dock = System.Windows.Forms.DockStyle.Top;
            this.stats1.Location = new System.Drawing.Point(15, 15);
            this.stats1.Margin = new System.Windows.Forms.Padding(25, 25, 3, 3);
            this.stats1.Name = "stats1";
            this.stats1.RowCount = 4;
            this.stats1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.stats1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.stats1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.5F));
            this.stats1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.5F));
            this.stats1.Size = new System.Drawing.Size(130, 94);
            this.stats1.TabIndex = 3;
            // 
            // P1Score
            // 
            this.P1Score.AutoSize = true;
            this.P1Score.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P1Score.Font = new System.Drawing.Font("ROG Fonts STRIX SCAR", 12.90566F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.P1Score.ForeColor = System.Drawing.SystemColors.Control;
            this.P1Score.Location = new System.Drawing.Point(3, 71);
            this.P1Score.Name = "P1Score";
            this.P1Score.Size = new System.Drawing.Size(124, 23);
            this.P1Score.TabIndex = 7;
            this.P1Score.Text = "Score";
            this.P1Score.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // P1Ammo
            // 
            this.P1Ammo.AutoSize = true;
            this.P1Ammo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P1Ammo.Font = new System.Drawing.Font("ROG Fonts STRIX SCAR", 12.90566F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.P1Ammo.ForeColor = System.Drawing.SystemColors.Control;
            this.P1Ammo.Location = new System.Drawing.Point(3, 50);
            this.P1Ammo.Name = "P1Ammo";
            this.P1Ammo.Size = new System.Drawing.Size(124, 21);
            this.P1Ammo.TabIndex = 6;
            this.P1Ammo.Text = "Ammo";
            this.P1Ammo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbP1Armor
            // 
            this.pbP1Armor.BarColor = System.Drawing.Color.LightSkyBlue;
            this.pbP1Armor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbP1Armor.Location = new System.Drawing.Point(3, 3);
            this.pbP1Armor.MaxValue = 50;
            this.pbP1Armor.Name = "pbP1Armor";
            this.pbP1Armor.Size = new System.Drawing.Size(124, 12);
            this.pbP1Armor.TabIndex = 2;
            this.pbP1Armor.Text = "customProgressBar1";
            this.pbP1Armor.Value = 0;
            // 
            // pbP1Health
            // 
            this.pbP1Health.BarColor = System.Drawing.Color.DarkSeaGreen;
            this.pbP1Health.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbP1Health.Location = new System.Drawing.Point(3, 21);
            this.pbP1Health.MaxValue = 100;
            this.pbP1Health.Name = "pbP1Health";
            this.pbP1Health.Size = new System.Drawing.Size(124, 26);
            this.pbP1Health.TabIndex = 3;
            this.pbP1Health.Text = "customProgressBar1";
            this.pbP1Health.Value = 0;
            // 
            // Player2Stats
            // 
            this.Player2Stats.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Player2Stats.Controls.Add(this.stats2);
            this.Player2Stats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Player2Stats.Location = new System.Drawing.Point(640, 0);
            this.Player2Stats.Margin = new System.Windows.Forms.Padding(0);
            this.Player2Stats.Name = "Player2Stats";
            this.Player2Stats.Padding = new System.Windows.Forms.Padding(15, 15, 15, 0);
            this.Player2Stats.Size = new System.Drawing.Size(160, 450);
            this.Player2Stats.TabIndex = 0;
            // 
            // stats2
            // 
            this.stats2.ColumnCount = 1;
            this.stats2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.stats2.Controls.Add(this.pbP2Armor, 0, 0);
            this.stats2.Controls.Add(this.pbP2Health, 0, 1);
            this.stats2.Controls.Add(this.P2Score, 0, 3);
            this.stats2.Controls.Add(this.P2Ammo, 0, 2);
            this.stats2.Dock = System.Windows.Forms.DockStyle.Top;
            this.stats2.Location = new System.Drawing.Point(15, 15);
            this.stats2.Name = "stats2";
            this.stats2.RowCount = 4;
            this.stats2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.stats2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.stats2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.5F));
            this.stats2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.5F));
            this.stats2.Size = new System.Drawing.Size(130, 94);
            this.stats2.TabIndex = 3;
            // 
            // pbP2Armor
            // 
            this.pbP2Armor.BarColor = System.Drawing.Color.LightSkyBlue;
            this.pbP2Armor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbP2Armor.Location = new System.Drawing.Point(3, 3);
            this.pbP2Armor.MaxValue = 50;
            this.pbP2Armor.Name = "pbP2Armor";
            this.pbP2Armor.Size = new System.Drawing.Size(124, 12);
            this.pbP2Armor.TabIndex = 2;
            this.pbP2Armor.Text = "customProgressBar1";
            this.pbP2Armor.Value = 0;
            // 
            // pbP2Health
            // 
            this.pbP2Health.BarColor = System.Drawing.Color.DarkSeaGreen;
            this.pbP2Health.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbP2Health.Location = new System.Drawing.Point(3, 21);
            this.pbP2Health.MaxValue = 100;
            this.pbP2Health.Name = "pbP2Health";
            this.pbP2Health.Size = new System.Drawing.Size(124, 26);
            this.pbP2Health.TabIndex = 3;
            this.pbP2Health.Text = "customProgressBar1";
            this.pbP2Health.Value = 0;
            // 
            // P2Score
            // 
            this.P2Score.AutoSize = true;
            this.P2Score.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P2Score.Font = new System.Drawing.Font("ROG Fonts STRIX SCAR", 12.90566F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.P2Score.ForeColor = System.Drawing.SystemColors.Control;
            this.P2Score.Location = new System.Drawing.Point(3, 71);
            this.P2Score.Name = "P2Score";
            this.P2Score.Size = new System.Drawing.Size(124, 23);
            this.P2Score.TabIndex = 4;
            this.P2Score.Text = "Score";
            this.P2Score.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // P2Ammo
            // 
            this.P2Ammo.AutoSize = true;
            this.P2Ammo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P2Ammo.Font = new System.Drawing.Font("ROG Fonts STRIX SCAR", 12.90566F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.P2Ammo.ForeColor = System.Drawing.SystemColors.Control;
            this.P2Ammo.Location = new System.Drawing.Point(3, 50);
            this.P2Ammo.Name = "P2Ammo";
            this.P2Ammo.Size = new System.Drawing.Size(124, 21);
            this.P2Ammo.TabIndex = 5;
            this.P2Ammo.Text = "Ammo";
            this.P2Ammo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TestWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TestWindow";
            this.Text = "TestWindow";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TestWindow_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TestWindow_KeyUp);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.GameContainer.ResumeLayout(false);
            this.pnlPause.ResumeLayout(false);
            this.pnlGameOver.ResumeLayout(false);
            this.pnlGameOver.PerformLayout();
            this.Player1Stats.ResumeLayout(false);
            this.stats1.ResumeLayout(false);
            this.stats1.PerformLayout();
            this.Player2Stats.ResumeLayout(false);
            this.stats2.ResumeLayout(false);
            this.stats2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel GameContainer;
        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.Panel Player1Stats;
        private System.Windows.Forms.Panel Player2Stats;
        private System.Windows.Forms.TableLayoutPanel stats1;
        private CustomProgressBar pbP1Armor;
        private CustomProgressBar pbP1Health;
        private System.Windows.Forms.TableLayoutPanel stats2;
        private CustomProgressBar pbP2Armor;
        private CustomProgressBar pbP2Health;
        private System.Windows.Forms.Label P2Score;
        private System.Windows.Forms.Label P2Ammo;
        private System.Windows.Forms.Label P1Ammo;
        private System.Windows.Forms.Label P1Score;
        private System.Windows.Forms.Panel pnlGameOver;
        private System.Windows.Forms.Label lblWinner;
        private System.Windows.Forms.Label lblFinalScore;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel pnlPause;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Button btnRestart;
    }
}