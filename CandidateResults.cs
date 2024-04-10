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
    public partial class CandidateResults : Form
    {
        db fn = new db();
        private int UserId;    
        public CandidateResults(int userID)
        {
            InitializeComponent();
            UserId = userID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //to get Candidate id with Candidate Name
            string query = "SELECT cid FROM candidate WHERE userid = '"+ UserId +"'";
            DataSet ds = fn.getData(query);
            int candidateId = Convert.ToInt32(ds.Tables[0].Rows[0]["cid"]);

            // Particular Candidate Results
            query = "SELECT c.cname as Candidate_name, p.partname as Party_name, COUNT(*) AS total_votes FROM Voting v INNER JOIN Candidate c ON v.cid = c.cid INNER JOIN Party p ON c.pid = p.pid WHERE c.cid = '"+ candidateId +"'GROUP BY p.partname, c.cname ORDER BY total_votes DESC";
            ds = fn.getData(query);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void button1_Click_1(object sender, EventArgs e)
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
