namespace Cobi_Cognitive_Architecture
{
    partial class MainScreen
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
            this.ProteinSequenceTextBox = new System.Windows.Forms.TextBox();
            this.SerineRadioBtn = new System.Windows.Forms.RadioButton();
            this.ThreonineRadioBtn = new System.Windows.Forms.RadioButton();
            this.TyrosineRadioBtn = new System.Windows.Forms.RadioButton();
            this.AllRadioBtn = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.SubmirBtn = new System.Windows.Forms.Button();
            this.ResultGrid = new System.Windows.Forms.DataGridView();
            this.ProgressBarPanel = new System.Windows.Forms.Panel();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.Percentage = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ResultGrid)).BeginInit();
            this.ProgressBarPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProteinSequenceTextBox
            // 
            this.ProteinSequenceTextBox.Location = new System.Drawing.Point(78, 148);
            this.ProteinSequenceTextBox.Multiline = true;
            this.ProteinSequenceTextBox.Name = "ProteinSequenceTextBox";
            this.ProteinSequenceTextBox.Size = new System.Drawing.Size(450, 173);
            this.ProteinSequenceTextBox.TabIndex = 1;
            // 
            // SerineRadioBtn
            // 
            this.SerineRadioBtn.AutoSize = true;
            this.SerineRadioBtn.Location = new System.Drawing.Point(77, 387);
            this.SerineRadioBtn.Name = "SerineRadioBtn";
            this.SerineRadioBtn.Size = new System.Drawing.Size(70, 21);
            this.SerineRadioBtn.TabIndex = 2;
            this.SerineRadioBtn.TabStop = true;
            this.SerineRadioBtn.Text = "Serine";
            this.SerineRadioBtn.UseVisualStyleBackColor = true;
            // 
            // ThreonineRadioBtn
            // 
            this.ThreonineRadioBtn.AutoSize = true;
            this.ThreonineRadioBtn.Location = new System.Drawing.Point(181, 387);
            this.ThreonineRadioBtn.Name = "ThreonineRadioBtn";
            this.ThreonineRadioBtn.Size = new System.Drawing.Size(94, 21);
            this.ThreonineRadioBtn.TabIndex = 3;
            this.ThreonineRadioBtn.TabStop = true;
            this.ThreonineRadioBtn.Text = "Threonine";
            this.ThreonineRadioBtn.UseVisualStyleBackColor = true;
            // 
            // TyrosineRadioBtn
            // 
            this.TyrosineRadioBtn.AutoSize = true;
            this.TyrosineRadioBtn.Location = new System.Drawing.Point(308, 387);
            this.TyrosineRadioBtn.Name = "TyrosineRadioBtn";
            this.TyrosineRadioBtn.Size = new System.Drawing.Size(84, 21);
            this.TyrosineRadioBtn.TabIndex = 4;
            this.TyrosineRadioBtn.TabStop = true;
            this.TyrosineRadioBtn.Text = "Tyrosine";
            this.TyrosineRadioBtn.UseVisualStyleBackColor = true;
            // 
            // AllRadioBtn
            // 
            this.AllRadioBtn.AutoSize = true;
            this.AllRadioBtn.Location = new System.Drawing.Point(424, 387);
            this.AllRadioBtn.Name = "AllRadioBtn";
            this.AllRadioBtn.Size = new System.Drawing.Size(81, 21);
            this.AllRadioBtn.TabIndex = 5;
            this.AllRadioBtn.TabStop = true;
            this.AllRadioBtn.Text = "All three";
            this.AllRadioBtn.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(73, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "Enter Protein Sequence:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(74, 349);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(227, 24);
            this.label2.TabIndex = 7;
            this.label2.Text = "Select residues to predict:";
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Font = new System.Drawing.Font("Cooper Black", 16.2F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderLabel.Location = new System.Drawing.Point(106, 36);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(391, 32);
            this.HeaderLabel.TabIndex = 8;
            this.HeaderLabel.Text = "Protein Submission Portal";
            // 
            // SubmirBtn
            // 
            this.SubmirBtn.Font = new System.Drawing.Font("Cooper Black", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SubmirBtn.Location = new System.Drawing.Point(367, 447);
            this.SubmirBtn.Name = "SubmirBtn";
            this.SubmirBtn.Size = new System.Drawing.Size(161, 43);
            this.SubmirBtn.TabIndex = 9;
            this.SubmirBtn.Text = "Submit";
            this.SubmirBtn.UseVisualStyleBackColor = true;
            this.SubmirBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // ResultGrid
            // 
            this.ResultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultGrid.Location = new System.Drawing.Point(49, 106);
            this.ResultGrid.Name = "ResultGrid";
            this.ResultGrid.RowTemplate.Height = 24;
            this.ResultGrid.Size = new System.Drawing.Size(341, 396);
            this.ResultGrid.TabIndex = 10;
            // 
            // ProgressBarPanel
            // 
            this.ProgressBarPanel.Controls.Add(this.label4);
            this.ProgressBarPanel.Controls.Add(this.ResultGrid);
            this.ProgressBarPanel.Controls.Add(this.Percentage);
            this.ProgressBarPanel.Controls.Add(this.ProgressBar);
            this.ProgressBarPanel.Location = new System.Drawing.Point(77, 91);
            this.ProgressBarPanel.Name = "ProgressBarPanel";
            this.ProgressBarPanel.Size = new System.Drawing.Size(451, 511);
            this.ProgressBarPanel.TabIndex = 11;
            this.ProgressBarPanel.Visible = false;
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(63, 49);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(311, 23);
            this.ProgressBar.TabIndex = 0;
            // 
            // Percentage
            // 
            this.Percentage.AutoSize = true;
            this.Percentage.Location = new System.Drawing.Point(344, 75);
            this.Percentage.Name = "Percentage";
            this.Percentage.Size = new System.Drawing.Size(28, 17);
            this.Percentage.TabIndex = 1;
            this.Percentage.Text = "0%";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(60, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 24);
            this.label4.TabIndex = 7;
            this.label4.Text = "Loading Models:";
            // 
            // MainScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 603);
            this.Controls.Add(this.ProgressBarPanel);
            this.Controls.Add(this.SubmirBtn);
            this.Controls.Add(this.HeaderLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AllRadioBtn);
            this.Controls.Add(this.TyrosineRadioBtn);
            this.Controls.Add(this.ThreonineRadioBtn);
            this.Controls.Add(this.SerineRadioBtn);
            this.Controls.Add(this.ProteinSequenceTextBox);
            this.Name = "MainScreen";
            this.Text = "CoBi (A Cognitive Agent for Bioinformatics)";
            ((System.ComponentModel.ISupportInitialize)(this.ResultGrid)).EndInit();
            this.ProgressBarPanel.ResumeLayout(false);
            this.ProgressBarPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton SerineRadioBtn;
        private System.Windows.Forms.RadioButton ThreonineRadioBtn;
        private System.Windows.Forms.RadioButton TyrosineRadioBtn;
        private System.Windows.Forms.RadioButton AllRadioBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.Button SubmirBtn;
        public System.Windows.Forms.TextBox ProteinSequenceTextBox;
        private System.Windows.Forms.DataGridView ResultGrid;
        private System.Windows.Forms.Panel ProgressBarPanel;
        private System.Windows.Forms.Label Percentage;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.Label label4;
    }
}

