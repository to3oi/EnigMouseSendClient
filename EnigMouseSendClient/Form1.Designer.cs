﻿namespace EnigMouseSendClient
{
    partial class Form1
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
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            testSend = new Button();
            SendIP = new TextBox();
            MasterPC_Connection = new Button();
            button1 = new Button();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(testSend);
            groupBox1.Controls.Add(SendIP);
            groupBox1.Controls.Add(MasterPC_Connection);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(794, 444);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Info";
            // 
            // testSend
            // 
            testSend.Location = new Point(268, 100);
            testSend.Name = "testSend";
            testSend.Size = new Size(75, 23);
            testSend.TabIndex = 2;
            testSend.Text = "送信開始";
            testSend.UseVisualStyleBackColor = true;
            testSend.Click += testInitSend_Click;
            // 
            // SendIP
            // 
            SendIP.Location = new Point(254, 56);
            SendIP.Name = "SendIP";
            SendIP.Size = new Size(100, 23);
            SendIP.TabIndex = 1;
            // 
            // MasterPC_Connection
            // 
            MasterPC_Connection.Location = new Point(27, 88);
            MasterPC_Connection.Name = "MasterPC_Connection";
            MasterPC_Connection.Size = new Size(75, 23);
            MasterPC_Connection.TabIndex = 0;
            MasterPC_Connection.Text = "受信開始";
            MasterPC_Connection.UseVisualStyleBackColor = true;
            MasterPC_Connection.Click += MasterPC_Connection_Click;
            // 
            // button1
            // 
            button1.Location = new Point(268, 140);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 3;
            button1.Text = "送信";
            button1.UseVisualStyleBackColor = true;
            button1.Click += testSend_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "Form1";
            Text = "EnigMouseSendClient";
            FormClosing += Form1_Closing;
            Load += Form1_Load;
            tableLayoutPanel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private Button MasterPC_Connection;
        private Button testSend;
        private TextBox SendIP;
        private Button button1;
    }
}