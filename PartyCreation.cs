using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Xml.Linq;
using System.Threading;

namespace ElectronicVotingSystem
{
    public partial class PartyCreation : Form
    {
        db fn = new db();
        public PartyCreation()
        {
            InitializeComponent();
        }

        private void PartyCreation_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void Create_Click(object sender, EventArgs e)
        {
            string PartyName = partyName.Text;
            string query = "INSERT INTO party (partname) VALUES ('" + PartyName + "')";
            fn.setData(query, "Party Created SuccessFully");

            partyName.Text = "";
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                    break;
                case 1:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                    break;
                case 2:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR");
                    break;
                case 3:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-PT");
                    break;
            }

            this.Controls.Clear();
            InitializeComponent();
        }
    }
}
