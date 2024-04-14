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
    public partial class FirstPastthepost : Form
    {
        db fn = db.GetInstance();
        public FirstPastthepost()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FirstPastthepost_Load(object sender, EventArgs e)
        {
            string query = "SELECT c.cname as Candidate_Name, c.postalcode as Postal_Code, COUNT(*) AS total_votes FROM Candidate c LEFT JOIN Voting v ON c.cid = v.cid GROUP BY cname, postalcode ORDER BY total_votes DESC";
            DataSet ds = fn.getData(query);
            dataGridView1.DataSource = ds.Tables[0];
        }
    }
}
