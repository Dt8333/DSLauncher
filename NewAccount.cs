﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;

namespace DSLauncherV2
{
    public partial class NewAccount : MetroFramework.Forms.MetroForm
    {
        private UserSettings UserSettings = UserSettings.Instance;
        public NewAccount()
        {
            InitializeComponent();
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            string md5Hash1 = this.GetMD5Hash(Guid.NewGuid().ToString() + DateTime.Now.ToLongTimeString());
            this.CodeTextbox.Text = "";
            int startIndex1 = 0;
            while (startIndex1 < 32)
            {
                string str = md5Hash1.Substring(startIndex1, 8);
                if (startIndex1 != 0)
                    this.CodeTextbox.Text += "-";
                this.CodeTextbox.Text += str;
                startIndex1 += 8;
            }

            string md5Hash2 = this.GetMD5Hash(this.CodeTextbox.Text);
            if (this.CodeTextbox.Text.Length != 35 && md5Hash2.Length != 32)
            {
                this.SigTextbox.Text = "Invalid code";
            }

            else
            {
                this.SigTextbox.Text = "";
                int startIndex2 = 0;
                while (startIndex2 < 32)
                {
                    string str1 = "";
                    string str2 = md5Hash2.Substring(startIndex2, 8);
                    int startIndex3 = 6;
                    while (startIndex3 >= 0)
                    {
                        str1 += str2.Substring(startIndex3, 2);
                        startIndex3 -= 2;
                    }
                    if (startIndex2 != 0)
                        this.SigTextbox.Text += "-";
                    this.SigTextbox.Text += str1;
                    startIndex2 += 8;
                }
            }
        }

        public string GetMD5Hash(string input)
        {
            byte[] hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte num in hash)
                stringBuilder.Append(num.ToString("x2").ToLower());
            return stringBuilder.ToString();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (this.NameTextbox.Text.Length == 0)
            {
                MetroMessageBox.Show(this, "Name field is empty.", "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            if (this.DescriptionTextbox.Text.Length == 0)
            {
                MetroMessageBox.Show(this, "Description field is empty.", "", MessageBoxButtons.OK,
                    MessageBoxIcon.Hand);
                return;
            }

            if (this.CategoryTextbox.Text.Length == 0)
            {
                MetroMessageBox.Show(this, "Category field is empty.", "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            if (this.SigTextbox.Text.Length == 0 || this.CodeTextbox.Text.Length == 0)
            {
                MetroMessageBox.Show(this,
                    "Signature and Code fields have not been generated. Hit the generate button.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            this.UserSettings.AName = NameTextbox.Text;
            this.UserSettings.ADescription = DescriptionTextbox.Text;
            this.UserSettings.AccountCategory = CategoryTextbox.Text;
            this.UserSettings.AFavorite = false;
            this.UserSettings.ACode = CodeTextbox.Text;
            this.UserSettings.ASignature = SigTextbox.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void NewAccount_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.UserSettings.AName))
            {
                this.NameTextbox.Text = this.UserSettings.AName;
                this.DescriptionTextbox.Text = this.UserSettings.ADescription;
                this.CategoryTextbox.Text = this.UserSettings.AccountCategory;
                this.CodeTextbox.Text = this.UserSettings.ACode;
                this.SigTextbox.Text = this.UserSettings.ASignature;
            }
        }
    }
}
