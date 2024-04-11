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
    public partial class CandidateOverallResult : Form
    {
        db fn =  db.GetInstance();
        public CandidateOverallResult()
        {
            InitializeComponent();
        }

        private void CandidateResult_Load(object sender, EventArgs e)
        {
            string query = "SELECT c.cname as Candidate_Name, COUNT(*) AS total_votes FROM Voting v INNER JOIN Candidate c ON v.cid =c.cid GROUP BY cname ORDER BY total_votes DESC";
            DataSet ds = fn.getData(query);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
