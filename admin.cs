using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicVotingSystem
{
    public partial class admin : Form
    {
        public admin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateCandidate createCandidate = new CreateCandidate();
            createCandidate.TopLevel = false;
            // set the form properties
            createCandidate.FormBorderStyle = FormBorderStyle.None;
            createCandidate.Dock = DockStyle.Fill;
            panel2.Controls.Clear();

            // Add the form to the Controls collection of the panel
            panel2.Controls.Add(createCandidate);

            // Show the form
            createCandidate.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            PartyCreation partyCreation = new PartyCreation();
            partyCreation.TopLevel = false;
            partyCreation.FormBorderStyle = FormBorderStyle.None;
            partyCreation.Dock = DockStyle.Fill;
            panel2.Controls.Clear();
            panel2.Controls.Add(partyCreation);
            // Show the form
            partyCreation.Show();
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            LogOut();
        }
        private void LogOut()
        {
            SignInform s1 = new SignInform();
            s1.Show();
            this.Hide();
        }
    }
}
