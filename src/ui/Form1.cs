using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mississippi_DLL;

namespace Mississippi_Form
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            int k, l;
            Stopwatch stopwatch;
            Dictionary<string, ushort> result;
            result = new Dictionary<string,ushort>();

            try
            {
                k = Convert.ToInt32(kTextBox.Text);
                k = k == 0 ? 1 : k;
            }
            catch
            {
                k = 1;
            }
            try
            {
                l = Convert.ToInt32(lTextBox.Text);
                l = l == 0 ? 1 : l;
            }
            catch
            {
                l = 1;
            }

            klLable.Text = "Rechne mit k = " + k + " und l = " + l + "...";
            UseWaitCursor = true;
            Application.DoEvents();
            inputBox.Enabled = false;
            kTextBox.Enabled = false;
            lTextBox.Enabled = false;
            checkBox1.Enabled = false;
            goButton.Enabled = false;
            helpLabelLink.Enabled = false;
            stopwatch = Stopwatch.StartNew();
            //
            try
            {
                result = new Mississippi().main(inputBox.Text, k, l, checkBox1.Checked);
            }
            catch (OutOfMemoryException ex)
            {
                GC.Collect();
                MessageBox.Show("Nicht genug Speicher!\nEinen höheren Wert für l wählen.", "Ergebnismenge zu groß", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);   
            }
            //
            stopwatch.Stop();
            inputBox.Enabled = true;
            kTextBox.Enabled = true;
            lTextBox.Enabled = true;
            checkBox1.Enabled = true;
            goButton.Enabled = true;
            helpLabelLink.Enabled = true;
            UseWaitCursor = false;
            klLable.Text = "Berechnet mit k = " + k + " und l = " + l;
            timeLabel.Text = "Verstrichene Zeit: " + stopwatch.ElapsedMilliseconds / 1000 + " Sekunden";
            if (result.Count != 0)
            {
                var lines = result.Select(i => i.Key + " (" + i.Value + "x)");
                outputBox.Text = string.Join(Environment.NewLine, lines);
                outputCount.Text = "(" + outputBox.Lines.Count().ToString() + " Zeilen)";
            }
            else
                outputBox.Text = "keine Ergebnisse";
        }

        private void inputBox_TextChanged(object sender, EventArgs e)
        {
            klLable.ResetText();
            timeLabel.ResetText();
            outputBox.ResetText();
            inputCount.Text = "(" + inputBox.Text.Length.ToString() + " Zeichen)";
            outputCount.ResetText();
        }

        private void helpLabelLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Ist diese Option aktiviert, werden nur Teilzeichenketten, die kein Bestandteil anderer gleich häufiger Zeichenketten sind, ausgegeben.", "Maximale Teilzeichenketten");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            outputBox.ResetText();
        }
    }
}
