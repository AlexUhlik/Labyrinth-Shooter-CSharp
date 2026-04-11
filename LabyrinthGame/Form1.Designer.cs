namespace LabyrinthGame
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.glControl1 = new OpenTK.GLControl();
            this.Player2Stats = new System.Windows.Forms.TableLayoutPanel();
            this.lblP2Ammo = new System.Windows.Forms.TextBox();
            this.Player1Stats = new System.Windows.Forms.TableLayoutPanel();
            this.lblP1Ammo = new System.Windows.Forms.TextBox();
            this.pbP1Armor = new LabyrinthGame.CustomProgressBar();
            this.pbP1Health = new LabyrinthGame.CustomProgressBar();
            this.pbP2Armor = new LabyrinthGame.CustomProgressBar();
            this.pbP2Health = new LabyrinthGame.CustomProgressBar();
            this.Player2Stats.SuspendLayout();
            this.Player1Stats.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(184, 3);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(430, 430);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            // 
            // Player2Stats
            // 
            this.Player2Stats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Player2Stats.ColumnCount = 1;
            this.Player2Stats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Player2Stats.Controls.Add(this.pbP2Armor, 0, 0);
            this.Player2Stats.Controls.Add(this.lblP2Ammo, 0, 2);
            this.Player2Stats.Controls.Add(this.pbP2Health, 0, 1);
            this.Player2Stats.Location = new System.Drawing.Point(631, 12);
            this.Player2Stats.Name = "Player2Stats";
            this.Player2Stats.RowCount = 3;
            this.Player2Stats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.Player2Stats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.Player2Stats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.Player2Stats.Size = new System.Drawing.Size(157, 72);
            this.Player2Stats.TabIndex = 2;
            // 
            // lblP2Ammo
            // 
            this.lblP2Ammo.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.792453F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblP2Ammo.Location = new System.Drawing.Point(3, 52);
            this.lblP2Ammo.Name = "lblP2Ammo";
            this.lblP2Ammo.ReadOnly = true;
            this.lblP2Ammo.Size = new System.Drawing.Size(68, 19);
            this.lblP2Ammo.TabIndex = 2;
            // 
            // Player1Stats
            // 
            this.Player1Stats.ColumnCount = 1;
            this.Player1Stats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Player1Stats.Controls.Add(this.pbP1Armor, 0, 0);
            this.Player1Stats.Controls.Add(this.lblP1Ammo, 0, 2);
            this.Player1Stats.Controls.Add(this.pbP1Health, 0, 1);
            this.Player1Stats.Location = new System.Drawing.Point(12, 12);
            this.Player1Stats.Name = "Player1Stats";
            this.Player1Stats.RowCount = 3;
            this.Player1Stats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.Player1Stats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.Player1Stats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.Player1Stats.Size = new System.Drawing.Size(157, 72);
            this.Player1Stats.TabIndex = 3;
            // 
            // lblP1Ammo
            // 
            this.lblP1Ammo.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.792453F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblP1Ammo.Location = new System.Drawing.Point(3, 52);
            this.lblP1Ammo.Name = "lblP1Ammo";
            this.lblP1Ammo.ReadOnly = true;
            this.lblP1Ammo.Size = new System.Drawing.Size(68, 19);
            this.lblP1Ammo.TabIndex = 2;
            // 
            // pbP1Armor
            // 
            this.pbP1Armor.BarColor = System.Drawing.Color.LightSkyBlue;
            this.pbP1Armor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbP1Armor.Location = new System.Drawing.Point(3, 3);
            this.pbP1Armor.MaxValue = 50;
            this.pbP1Armor.Name = "pbP1Armor";
            this.pbP1Armor.Size = new System.Drawing.Size(151, 15);
            this.pbP1Armor.TabIndex = 2;
            this.pbP1Armor.Text = "customProgressBar1";
            this.pbP1Armor.Value = 0;
            // 
            // pbP1Health
            // 
            this.pbP1Health.BarColor = System.Drawing.Color.GreenYellow;
            this.pbP1Health.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbP1Health.Location = new System.Drawing.Point(3, 24);
            this.pbP1Health.MaxValue = 100;
            this.pbP1Health.Name = "pbP1Health";
            this.pbP1Health.Size = new System.Drawing.Size(151, 22);
            this.pbP1Health.TabIndex = 3;
            this.pbP1Health.Text = "customProgressBar1";
            this.pbP1Health.Value = 0;
            // 
            // pbP2Armor
            // 
            this.pbP2Armor.BarColor = System.Drawing.Color.LightSkyBlue;
            this.pbP2Armor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbP2Armor.Location = new System.Drawing.Point(3, 3);
            this.pbP2Armor.MaxValue = 50;
            this.pbP2Armor.Name = "pbP2Armor";
            this.pbP2Armor.Size = new System.Drawing.Size(151, 15);
            this.pbP2Armor.TabIndex = 2;
            this.pbP2Armor.Text = "customProgressBar1";
            this.pbP2Armor.Value = 0;
            // 
            // pbP2Health
            // 
            this.pbP2Health.BarColor = System.Drawing.Color.GreenYellow;
            this.pbP2Health.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbP2Health.Location = new System.Drawing.Point(3, 24);
            this.pbP2Health.MaxValue = 100;
            this.pbP2Health.Name = "pbP2Health";
            this.pbP2Health.Size = new System.Drawing.Size(151, 22);
            this.pbP2Health.TabIndex = 3;
            this.pbP2Health.Text = "customProgressBar1";
            this.pbP2Health.Value = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Player1Stats);
            this.Controls.Add(this.Player2Stats);
            this.Controls.Add(this.glControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.Player2Stats.ResumeLayout(false);
            this.Player2Stats.PerformLayout();
            this.Player1Stats.ResumeLayout(false);
            this.Player1Stats.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.TextBox lblP2Ammo;
        private System.Windows.Forms.TableLayoutPanel Player2Stats;
        private CustomProgressBar pbP2Armor;
        private CustomProgressBar pbP2Health;
        private System.Windows.Forms.TableLayoutPanel Player1Stats;
        private CustomProgressBar pbP1Armor;
        private System.Windows.Forms.TextBox lblP1Ammo;
        private CustomProgressBar pbP1Health;
    }
}

