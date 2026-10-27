namespace Hogwarts.View
{
    partial class StartUp
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            wizardGridView = new DataGridView();
            listButton = new Button();
            addButton = new Button();
            ((System.ComponentModel.ISupportInitialize)wizardGridView).BeginInit();
            SuspendLayout();
            // 
            // wizardGridView
            // 
            wizardGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            wizardGridView.Location = new Point(12, 12);
            wizardGridView.Name = "wizardGridView";
            wizardGridView.ReadOnly = true;
            wizardGridView.RowHeadersWidth = 51;
            wizardGridView.Size = new Size(715, 400);
            wizardGridView.TabIndex = 0;
            // 
            // listButton
            // 
            listButton.Location = new Point(417, 433);
            listButton.Name = "listButton";
            listButton.Size = new Size(120, 53);
            listButton.TabIndex = 1;
            listButton.Text = "Listázás";
            listButton.UseVisualStyleBackColor = true;
            listButton.Click += ListWizards;
            // 
            // addButton
            // 
            addButton.Location = new Point(603, 433);
            addButton.Name = "addButton";
            addButton.Size = new Size(103, 53);
            addButton.TabIndex = 2;
            addButton.Text = "Hozzáadás";
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += AddWizard;
            // 
            // StartUp
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(958, 600);
            Controls.Add(addButton);
            Controls.Add(listButton);
            Controls.Add(wizardGridView);
            Margin = new Padding(3, 4, 3, 4);
            Name = "StartUp";
            Text = "Roxfort Varázslóképző";
            Load += LoadWizards;
            ((System.ComponentModel.ISupportInitialize)wizardGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView wizardGridView;
        private Button listButton;
        private Button addButton;
    }
}
