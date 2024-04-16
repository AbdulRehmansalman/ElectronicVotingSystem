using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicVotingSystem
{
    public partial class VoteCast : Form
    {
        private bool btnClicked = false;
        DialogResult result;
        //userid taken from login 
        private int userId;
        db fn = db.GetInstance();
        public VoteCast(int userId)
        {
            InitializeComponent();
            this.userId = userId;   
            panel2.Hide();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            btnClicked = true;
            panel2.Hide();
        }

        private void VoteCast_Load(object sender, EventArgs e)
        {
            string query = "SELECT postalcode FROM Users WHERE id = '" + userId +"'";
            DataSet postalCodeDS = fn.getData(query); // Assume userId is the current user's ID
            string userPostalCode = postalCodeDS.Tables[0].Rows[0]["postalcode"].ToString();

            //to see that the Current User Vote Candidate is Present then  
             query = "SELECT Distinct(c.cid) AS CandidateID, c.cname AS CandidateName, c.votingsign, p.partName AS PartyName,c.postalcode FROM candidate c JOIN party p ON c.pid = p.pid JOIN Users u ON c.postalcode = u.postalcode WHERE c.postalcode = '"+userPostalCode+"'";
            DataSet ds = fn.getData(query);
            dataGridView1.DataSource =ds.Tables[0];

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string Preference = preference.Text;
            // cell is clicked not the header 
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {  
                // get candidate id form the clicked cell   
                int candidateId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                string postalCode = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();

                panel2.Show();
                if (btnClicked)
                {
                    // Display a MessageBox with OK button
                    result = MessageBox.Show("Do you want to vote for this candidate?", "Confirm Vote", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    // Check the user's selection
                    if (result == DialogResult.Yes)
                    {
                        //first check that the same user votes to same candidate:
                        string query = "SELECT COUNT(*) FROM Voting WHERE cid = '" + candidateId + "' AND userid = '" + userId + "'";
                        DataSet ds = fn.getData(query);
                        int count = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                        if (count > 0)
                        {
                            MessageBox.Show("Already Voted For This Candidate");
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Preference))
                            {
                                query = "INSERT INTO voting (cid, userid,preferences,postalcode) VALUES ('" + candidateId + "', '" + userId + "', '" + Convert.ToInt16(Preference) + "', '" + postalCode + "')";
                                fn.setData(query, "Vote Done Successfully");
                            }
                            else
                            {
                                MessageBox.Show("Plz Fill the Preferenece");

                            }

                        }

                    }
                    else if (result == DialogResult.No)
                    {
                        MessageBox.Show("Do you want to cancel the Vote.", "Cancel Vote", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                btnClicked = false;
                
                
            }
        }

        
    }
}
