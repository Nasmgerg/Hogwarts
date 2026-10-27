namespace Hogwarts.View
{
    partial class Add
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            nameTextBox = new TextBox();
            patronusTextBox = new TextBox();
            roleComboBox = new ComboBox();
            houseComboBox = new ComboBox();
            classUpDown = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            addButton = new Button();
            cancelButton = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)classUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // nameTextBox
            // 
            nameTextBox.Location = new Point(220, 46);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(125, 27);
            nameTextBox.TabIndex = 0;
            // 
            // patronusTextBox
            // 
            patronusTextBox.Location = new Point(220, 97);
            patronusTextBox.Name = "patronusTextBox";
            patronusTextBox.Size = new Size(125, 27);
            patronusTextBox.TabIndex = 1;
            // 
            // roleComboBox
            // 
            roleComboBox.FormattingEnabled = true;
            roleComboBox.Location = new Point(220, 155);
            roleComboBox.Name = "roleComboBox";
            roleComboBox.Size = new Size(151, 28);
            roleComboBox.TabIndex = 2;
            // 
            // houseComboBox
            // 
            houseComboBox.FormattingEnabled = true;
            houseComboBox.Location = new Point(220, 214);
            houseComboBox.Name = "houseComboBox";
            houseComboBox.Size = new Size(151, 28);
            houseComboBox.TabIndex = 3;
            // 
            // classUpDown
            // 
            classUpDown.Location = new Point(220, 275);
            classUpDown.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            classUpDown.Name = "classUpDown";
            classUpDown.Size = new Size(150, 27);
            classUpDown.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(40, 49);
            label1.Name = "label1";
            label1.Size = new Size(50, 20);
            label1.TabIndex = 5;
            label1.Text = "Név:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(40, 100);
            label2.Name = "label2";
            label2.Size = new Size(50, 20);
            label2.TabIndex = 6;
            label2.Text = "Patrónus:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(40, 158);
            label3.Name = "label3";
            label3.Size = new Size(50, 20);
            label3.TabIndex = 7;
            label3.Text = "Szerepkör:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(40, 217);
            label4.Name = "label4";
            label4.Size = new Size(50, 20);
            label4.TabIndex = 8;
            label4.Text = "Ház:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(40, 282);
            label5.Name = "label5";
            label5.Size = new Size(50, 20);
            label5.TabIndex = 9;
            label5.Text = "Osztály:";
            // 
            // addButton
            // 
            addButton.Location = new Point(24, 392);
            addButton.Name = "addButton";
            addButton.Size = new Size(94, 29);
            addButton.TabIndex = 10;
            addButton.Text = "Mentés";
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += Save;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(251, 392);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(94, 29);
            cancelButton.TabIndex = 11;
            cancelButton.Text = "Mégse";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += Cancel;
            // 
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
            // 
            // Add
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(cancelButton);
            Controls.Add(addButton);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(classUpDown);
            Controls.Add(houseComboBox);
            Controls.Add(roleComboBox);
            Controls.Add(patronusTextBox);
            Controls.Add(nameTextBox);
            Name = "Add";
            Text = "Varázsló hozzáadása";
            ((System.ComponentModel.ISupportInitialize)classUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox nameTextBox;
        private TextBox patronusTextBox;
        private ComboBox roleComboBox;
        private ComboBox houseComboBox;
        private NumericUpDown classUpDown;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button addButton;
        private Button cancelButton;
        private ErrorProvider errorProvider;
    }
}