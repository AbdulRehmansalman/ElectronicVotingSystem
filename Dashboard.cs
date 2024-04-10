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
    public partial class Dashboard : Form
    {
        private int userId;
        public Dashboard(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            VoteCast voteCast = new VoteCast(userId);
            voteCast.TopLevel = false;
            voteCast.FormBorderStyle = FormBorderStyle.None;
            voteCast.Dock = DockStyle.Fill;

            // Add the form to the Controls collection of the panel
            panel2.Controls.Clear();
            panel2.Controls.Add(voteCast);

            // Show the form
            voteCast.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SeeCastedVotes seeCastedVotes = new SeeCastedVotes(userId);
            seeCastedVotes.TopLevel = false;
            seeCastedVotes.FormBorderStyle = FormBorderStyle.None;
            seeCastedVotes.Dock = DockStyle.Fill;
            panel2.Controls.Clear();
            panel2.Controls.Add(seeCastedVotes);

            seeCastedVotes.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CandidateOverallResult candidateResult = new CandidateOverallResult();
            candidateResult.TopLevel = false;
            candidateResult.FormBorderStyle = FormBorderStyle.None;
            candidateResult.Dock = DockStyle.Fill;
            panel2.Controls.Clear();
            panel2.Controls.Add(candidateResult);
            candidateResult.Show();
        }

        private void button4_Click(object sender, EventArgs e)
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
