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
        DataSet dataSet;
        DataTable dataTable;
        string PostalCode;
        int totalVotes = 0;
        string elected="elected";

        public ElectingSeats()
        {
            InitializeComponent();
            
        }
       
        private void ElectingSeats_Load(object sender, EventArgs e)
        {            
        }
        private void DistributeSurplusVotes(int candidateID, int surplusVotes)
        {
            string query = "SELECT v.userid, v.preferences FROM voting v " +
                           "WHERE v.cid = '" + candidateID + "' AND v.preferences > 1 " +
                           "ORDER BY v.preferences ASC";

            DataSet voterPreferencesDataSet = fn.getData(query);
            DataTable voterPreferencesTable = voterPreferencesDataSet.Tables[0];

            int totalVoters = voterPreferencesTable.Rows.Count;

            foreach (DataRow voterRow in voterPreferencesTable.Rows)
            {
                int voterID = Convert.ToInt32(voterRow["userid"]);
                int nextPreference = Convert.ToInt32(voterRow["preferences"]) - 1; // Get the Next preference

                string updateVotesQuery = "UPDATE voting SET preferences = " + nextPreference + " WHERE cid = '" + candidateID + "' AND userid = '" + voterID + "'";
                fn.setData(updateVotesQuery, "Distribute Votes");

                surplusVotes--;

                if (surplusVotes == 0)
                    break; // Exit loop When Additional Votes Finished
            }
        }

        private void EliminateCandidateWithFewestVotes()
        {
            string query = "SELECT v.cid, COUNT(*) AS TotalVotes FROM voting v JOIN candidate c ON v.cid = c.cid WHERE c.seat != '" + elected + "' GROUP BY v.cid ORDER BY TotalVotes ASC";
            DataSet candidateVotesDataSet = fn.getData(query);
            DataTable candidateVotesTable = candidateVotesDataSet.Tables[0];

            if (candidateVotesTable.Rows.Count > 0)
            {
                DataRow candidateWithFewestVotes = candidateVotesTable.Rows[0];
                int fewestVotes = Convert.ToInt32(candidateWithFewestVotes["TotalVotes"]);
                int candidateID = Convert.ToInt32(candidateWithFewestVotes["cid"]);

                string updateStatusQuery = "UPDATE candidate SET seat ='" + elected + "' WHERE cid = '" + candidateID + "'";
                fn.setData(updateStatusQuery, "Mark Candidate as Elected " + candidateID);

                string updatePreferencesQuery = "UPDATE voting SET preferences = preferences - 1 WHERE cid = '" + candidateID + "'";
                fn.setData(updatePreferencesQuery, "Update Voter Preferences");

                // Transfer votes of eliminated candidate to next preferences
                DistributeSurplusVotes(candidateID, fewestVotes);
            }
            else
            {
                MessageBox.Show("No active candidates to eliminate.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PostalCode = postalCode.Text;
            if (!string.IsNullOrWhiteSpace(PostalCode))
            {
                int totalUsers;
                int totalSeats;
                int finalQuota;

                // Calculate total users
                string userCountQuery = "SELECT COUNT(distinct userid) AS TotalUsers FROM voting WHERE postalcode ='" + PostalCode + "'";
                DataSet userCountDataSet = fn.getData(userCountQuery);
                DataTable userCountTable = userCountDataSet.Tables[0];

                if (userCountTable.Rows.Count > 0)
                {
                    totalUsers = Convert.ToInt32(userCountTable.Rows[0]["TotalUsers"]);

                    // Calculate total seats
                    string seatCountQuery = "SELECT COUNT(DISTINCT seats) AS TotalSeats FROM singletvote WHERE postalcode = '" + PostalCode + "'";
                    DataSet seatCountDataSet = fn.getData(seatCountQuery);
                    DataTable seatCountTable = seatCountDataSet.Tables[0];

                    if (seatCountTable.Rows.Count > 0)
                    {
                        totalSeats = Convert.ToInt32(seatCountTable.Rows[0]["TotalSeats"]);

                        // Calculate final quota
                        finalQuota = (totalUsers / (totalSeats + 1)) + 1;

                        // Loop until all seats are filled
                        while (totalSeats > 0)
                        {
                            // Query to retrieve candidates who meet or exceed the quota
                            string candidatesQuery = "SELECT c.cid AS Candidate_ID, c.cname AS Candidate_Name, COUNT(*) AS Total_Votes " +
                                                     "FROM voting v " +
                                                     "JOIN candidate c ON v.cid = c.cid " +
                                                     "WHERE v.preferences = 1 AND v.postalcode = '" + PostalCode + "' " +
                                                     "GROUP BY c.cid, c.cname " +
                                                     "HAVING COUNT(*) >= '" + finalQuota + "'";
                            DataSet candidatesDataSet = fn.getData(candidatesQuery);
                            DataTable candidatesTable = candidatesDataSet.Tables[0];

                            if (candidatesTable.Rows.Count > 0)
                            {
                                DataRow electedCandidateRow = candidatesTable.Rows[0];
                                int candidateID = Convert.ToInt32(electedCandidateRow["Candidate_ID"]);
                                string candidateName = electedCandidateRow["Candidate_Name"].ToString();
                                int totalVotes = Convert.ToInt32(electedCandidateRow["Total_Votes"]);

                                // Calculate surplus votes
                                int surplusVotes = totalVotes - finalQuota;

                                // Distribute surplus votes
                                DistributeSurplusVotes(candidateID, surplusVotes);

                                // Update candidate's seat status
                                string updateCandidateQuery = "UPDATE candidate SET seat = '" + elected + "' WHERE cid = '" + candidateID + "'";
                                fn.setData(updateCandidateQuery, "Set Seat to Candidate");

                                // Decrease total seats count
                                totalSeats--;

                                // Display elected candidate
                                MessageBox.Show("The Candidate Elected is '" + candidateName + "'", "Elected Vote", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Remove elected candidate from consideration
                                EliminateCandidateWithFewestVotes();
                            }
                            else
                            {
                                // If no candidates meet the quota, eliminate candidates with the fewest votes
                                EliminateCandidateWithFewestVotes();
                            }
                        }
                        if (totalSeats == 0)
                            MessageBox.Show("All Seats Filled");

                        // Fetch the elected candidates and display them in the DataGridView
                        string electedCandidatesQuery = "SELECT cid, cname FROM candidate WHERE seat ='" + elected + "'";
                        DataSet electedCandidatesDataSet = fn.getData(electedCandidatesQuery);
                        dataGridView1.DataSource = electedCandidatesDataSet.Tables[0];
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve total seats count.");
                    }
                }
                else
                {
                    MessageBox.Show("Failed to retrieve total users count.");
                }
            }
            else
            {
                MessageBox.Show("First fill the Postal Code.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            admin Admin = new admin(); 
            Admin.Show();
            
        }
    }
}
