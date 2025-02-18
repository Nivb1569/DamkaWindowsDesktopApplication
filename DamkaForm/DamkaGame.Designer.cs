namespace DamkaForm
{
    partial class DamkaGame
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
            this.boardPanel = new System.Windows.Forms.Panel();
            this.labelPlayer2 = new System.Windows.Forms.Label();
            this.Player2Score = new System.Windows.Forms.Label();
            this.Player1Score = new System.Windows.Forms.Label();
            this.labelPlayer1 = new System.Windows.Forms.Label();
            this.lablesPanel = new System.Windows.Forms.Panel();
            this.lablesPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // boardPanel
            // 
            this.boardPanel.BackColor = System.Drawing.Color.Transparent;
            this.boardPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.boardPanel.Location = new System.Drawing.Point(41, 83);
            this.boardPanel.Name = "boardPanel";
            this.boardPanel.Size = new System.Drawing.Size(521, 362);
            this.boardPanel.TabIndex = 4;
            // 
            // labelPlayer2
            // 
            this.labelPlayer2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelPlayer2.AutoSize = true;
            this.labelPlayer2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelPlayer2.Location = new System.Drawing.Point(257, 10);
            this.labelPlayer2.Name = "labelPlayer2";
            this.labelPlayer2.Size = new System.Drawing.Size(57, 13);
            this.labelPlayer2.TabIndex = 2;
            this.labelPlayer2.Text = "Player 2:";
            this.labelPlayer2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelPlayer2.Click += new System.EventHandler(this.labelPlayer2_Click);
            // 
            // Player2Score
            // 
            this.Player2Score.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Player2Score.AutoSize = true;
            this.Player2Score.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Player2Score.Location = new System.Drawing.Point(321, 10);
            this.Player2Score.Name = "Player2Score";
            this.Player2Score.Size = new System.Drawing.Size(14, 13);
            this.Player2Score.TabIndex = 3;
            this.Player2Score.Text = "0";
            this.Player2Score.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Player1Score
            // 
            this.Player1Score.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Player1Score.AutoSize = true;
            this.Player1Score.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Player1Score.Location = new System.Drawing.Point(202, 10);
            this.Player1Score.Name = "Player1Score";
            this.Player1Score.Size = new System.Drawing.Size(14, 13);
            this.Player1Score.TabIndex = 1;
            this.Player1Score.Text = "0";
            this.Player1Score.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPlayer1
            // 
            this.labelPlayer1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelPlayer1.AutoSize = true;
            this.labelPlayer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelPlayer1.Location = new System.Drawing.Point(139, 10);
            this.labelPlayer1.Name = "labelPlayer1";
            this.labelPlayer1.Size = new System.Drawing.Size(57, 13);
            this.labelPlayer1.TabIndex = 0;
            this.labelPlayer1.Text = "Player 1:";
            this.labelPlayer1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lablesPanel
            // 
            this.lablesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lablesPanel.AutoSize = true;
            this.lablesPanel.BackColor = System.Drawing.SystemColors.Control;
            this.lablesPanel.Controls.Add(this.labelPlayer2);
            this.lablesPanel.Controls.Add(this.Player2Score);
            this.lablesPanel.Controls.Add(this.Player1Score);
            this.lablesPanel.Controls.Add(this.labelPlayer1);
            this.lablesPanel.Location = new System.Drawing.Point(41, 43);
            this.lablesPanel.Name = "lablesPanel";
            this.lablesPanel.Size = new System.Drawing.Size(521, 34);
            this.lablesPanel.TabIndex = 5;
            // 
            // DamkaGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 512);
            this.Controls.Add(this.lablesPanel);
            this.Controls.Add(this.boardPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DamkaGame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Damka";
            this.lablesPanel.ResumeLayout(false);
            this.lablesPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel boardPanel;
        private System.Windows.Forms.Label labelPlayer2;
        private System.Windows.Forms.Label Player2Score;
        private System.Windows.Forms.Label Player1Score;
        private System.Windows.Forms.Label labelPlayer1;
        private System.Windows.Forms.Panel lablesPanel;
    }
}