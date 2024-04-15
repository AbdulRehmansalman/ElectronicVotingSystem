using System;
using System.Collections;
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
    public partial class ElectingSeats : Form
    {
        db fn = db.GetInstance();
        private int finalQuota;
        DataSet dataSet;
        DataTable dataTable;
        string PostalCode;
        int totalseats;
        int totalVotes = 0;

        public ElectingSeats()
        {
            InitializeComponent();
            quota.Hide();
            quota.ReadOnly = true;
        }
       
        private void ElectingSeats_Load(object sender, EventArgs e)
        {
            
        }

        private void quotaDisplay_Click(object sender, EventArgs e)
        {
            //to find Quota: (TotalNumberoFUsers/No ofSeats +1)+1
            PostalCode = postalCode.Text;
            string query = "SELECT COUNT(userid) AS TotalUsers FROM voting WHERE postalcode ='" + PostalCode + "'";
            // Execute query and put in dataset
            //DataTable from DataSet
            dataSet = fn.getData(query);
            dataTable = dataSet.Tables[0];
            // get total users from datatable 
            int totalUsers = Convert.ToInt16(dataTable.Rows[0]["TotalUsers"]);
            // For ToTal Seats:
            if (!string.IsNullOrWhiteSpace(PostalCode))
            {
               
                query = "select distinct(seats) as totalseats from singletvote where postalcode = '" + PostalCode + "'";
                dataSet = fn.getData(query);
                dataTable = dataSet.Tables[0];
                totalseats = Convert.ToInt16(dataTable.Rows[0]["totalseats"]);

                finalQuota = (totalUsers / (totalseats + 1)) + 1;
                quota.Show();
                quota.Text = finalQuota.ToString();
            }
            else
            {
                MessageBox.Show("First Fill the Postal Code");
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int candidateID;
            string candidateName;
            string elected = "elected";

            if (!string.IsNullOrWhiteSpace(PostalCode))
            {
                string query = "SELECT c.cid AS Candidate_ID, c.cname AS Candidate_Name,v.preferences FROM voting v JOIN candidate c ON v.cid = c.cid WHERE v.preferences = 1 AND v.postalcode = '"+PostalCode+"'";
                DataSet ds = fn.getData(query);
                dataGridView1.DataSource = ds.Tables[0];
                query = "SELECT c.cid AS Candidate_ID, c.cname AS Candidate_Name, v.preferences, COUNT(*) AS Total_Votes FROM voting v JOIN candidate c ON v.cid = c.cid WHERE v.preferences = 1 AND v.postalcode = '"+PostalCode+"' GROUP BY c.cid, c.cname, v.preferences HAVING COUNT(*) >= '"+finalQuota+"'";
                DataSet dataSet = fn.getData(query);
                DataTable dataTable = dataSet.Tables[0];

                // Check if DataTable contains rows
                if (dataTable.Rows.Count > 0)
                {
                    totalseats -= 1;
                    // Get the values from the first row of the DataTable
                    candidateID = Convert.ToInt32(dataTable.Rows[0]["Candidate_ID"]);
                    candidateName = dataTable.Rows[0]["Candidate_Name"].ToString();
                    totalVotes = Convert.ToInt32(dataTable.Rows[0]["Total_Votes"]);
                    DialogResult result = MessageBox.Show("The Candidate Elected is '"+candidateName+"'", "Elected Vote", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //if (result == DialogResult.OK)
                    //{
                        
                    //    query = "UPDATE Candidate SET seat = '" + elected + "' WHERE cid = '" + candidateID + "'";
                    //    fn.setData(query, "set Seat to Candidate");
                    //}
                }
                else
                {
                    Console.WriteLine("No data found in the DataTable.");
                }
            }
            else
            {
                MessageBox.Show("First Fill the Postal Code");

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            int checkRemain = totalVotes - finalQuota;
            if(checkRemain > 0)
            {

            }
        }
    }
}
