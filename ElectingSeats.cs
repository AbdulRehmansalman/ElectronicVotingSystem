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
        string elected="elected";
        int totalusers;

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
            string query = "SELECT COUNT(Distinct userid) AS TotalUsers FROM voting WHERE postalcode ='" + PostalCode + "'";
            // Execute query and put in dataset
            //DataTable from DataSet
            dataSet = fn.getData(query);
            dataTable = dataSet.Tables[0];
            // get total users from datatable 
            totalusers = Convert.ToInt16(dataTable.Rows[0]["TotalUsers"]);
            // For ToTal Seats:
            if (!string.IsNullOrWhiteSpace(PostalCode))
            {
               
                query = "select distinct(seats) as totalseats from singletvote where postalcode = '" + PostalCode + "'";
                dataSet = fn.getData(query);
                dataTable = dataSet.Tables[0];
                totalseats = Convert.ToInt16(dataTable.Rows[0]["totalseats"]);

                finalQuota = (totalusers / (totalseats + 1)) + 1;
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
            //int candidateID;
            //string candidateName;
            //string elected = "elected";

            //if (!string.IsNullOrWhiteSpace(PostalCode))
            //{
            //    string query = "SELECT c.cid AS Candidate_ID, c.cname AS Candidate_Name,v.preferences FROM voting v JOIN candidate c ON v.cid = c.cid WHERE v.preferences = 1 AND v.postalcode = '"+PostalCode+"'";
            //    DataSet ds = fn.getData(query);
            //    dataGridView1.DataSource = ds.Tables[0];
            //    query = "SELECT c.cid AS Candidate_ID, c.cname AS Candidate_Name, v.preferences, COUNT(*) AS Total_Votes FROM voting v JOIN candidate c ON v.cid = c.cid WHERE v.preferences = 1 AND v.postalcode = '"+PostalCode+"' GROUP BY c.cid, c.cname, v.preferences HAVING COUNT(*) >= '"+finalQuota+"'";
            //    DataSet dataSet = fn.getData(query);
            //    DataTable dataTable = dataSet.Tables[0];

            //    // Check if DataTable contains rows
            //    if (dataTable.Rows.Count > 0)
            //    {
            //        totalseats -= 1;
            //        // Get the values from the first row of the DataTable
            //        candidateID = Convert.ToInt32(dataTable.Rows[0]["Candidate_ID"]);
            //        candidateName = dataTable.Rows[0]["Candidate_Name"].ToString();
            //        totalVotes = Convert.ToInt32(dataTable.Rows[0]["Total_Votes"]);
            //        DialogResult result = MessageBox.Show("The Candidate Elected is '"+candidateName+"'", "Elected Vote", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        //if (result == DialogResult.OK)
            //        //{
                        
            //        //    query = "UPDATE Candidate SET seat = '" + elected + "' WHERE cid = '" + candidateID + "'";
            //        //    fn.setData(query, "set Seat to Candidate");
            //        //}
            //    }
            //    else
            //    {
            //        Console.WriteLine("No data found in the DataTable.");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("First Fill the Postal Code");

            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int candidateID;
            string candidateName;
           
            if (!string.IsNullOrWhiteSpace(PostalCode))
            {
                string query = "SELECT c.cid AS Candidate_ID, c.cname AS Candidate_Name,v.preferences FROM voting v JOIN candidate c ON v.cid = c.cid WHERE v.preferences = 1 AND v.postalcode = '" + PostalCode + "'";
                DataSet ds = fn.getData(query);
                dataGridView1.DataSource = ds.Tables[0];
                query = "SELECT c.cid AS Candidate_ID, c.cname AS Candidate_Name, v.preferences, COUNT(*) AS Total_Votes FROM voting v JOIN candidate c ON v.cid = c.cid WHERE v.preferences = 1 AND v.postalcode = '" + PostalCode + "' GROUP BY c.cid, c.cname, v.preferences HAVING COUNT(*) >= '" + finalQuota + "'";
                DataSet dataSet = fn.getData(query);
                DataTable dataTable = dataSet.Tables[0];

                // Check if DataTable contains rows
                if (dataTable.Rows.Count > 0)
                {
                    while (totalseats != 0)
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            candidateID = Convert.ToInt32(row["Candidate_ID"]);
                            candidateName = row["Candidate_Name"].ToString();
                            totalVotes = Convert.ToInt32(row["Total_Votes"]);

                            // Check if the total votes fulfill the quota
                            if (totalVotes >= finalQuota)
                            {
                                int checkRemain = totalVotes - finalQuota;
                                // Perform actions for candidates who fulfill the quota
                                query = "SELECT top '" + checkRemain + "' cid, COUNT(userid) AS all_Preference_count FROM voting WHERE preferences > 1 GROUP BY cid";
                                DataSet nextPreferenceDataSet = fn.getData(query);
                                DataTable nextPreferenceDataTable = nextPreferenceDataSet.Tables[0];

                                if (nextPreferenceDataTable.Rows.Count > 0)
                                {
                                    DataRow nextPreferenceRow = nextPreferenceDataTable.Rows[0];
                                    int nextPreferenceCandidateID = Convert.ToInt32(nextPreferenceRow["cid"]);
                                    int nextPreferenceCount = Convert.ToInt32(nextPreferenceRow["all_Preference_count"]);

                                    // add the Reamining and the Next Preference 
                                    int result = nextPreferenceCount + checkRemain;
                                   masla string updateQuery = "UPDATE SingletVote SET totalVotes ='" + result + "' WHERE id = '" + nextPreferenceCandidateID+"'";
                                    // Execute the update query In Singletvote
                                    fn.setData(updateQuery, "Total Votes With Distributed stored");

                                    // You can then proceed with further actions, such as adding the votes of this candidate to the remaining surplus votes
                                }       
                            }
                        }
                    }
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
        private void button5_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PostalCode))
            {
                // Loop until all seats are filled
                while (totalseats > 0)
                {
                    // Query to retrieve candidates who meet or exceed the quota
                    string query = "SELECT c.cid AS Candidate_ID, c.cname AS Candidate_Name, v.preferences, COUNT(*) AS Total_Votes " +
                                   "FROM voting v " +
                                   "JOIN candidate c ON v.cid = c.cid " +
                                   "WHERE v.preferences = 1 AND v.postalcode = '" + PostalCode + "' " +
                                   "GROUP BY c.cid, c.cname, v.preferences " +
                                   "HAVING COUNT(*) >= '" + finalQuota + "'";
                    DataSet dataSet = fn.getData(query);
                    DataTable dataTable = dataSet.Tables[0];

                    // Check if DataTable contains rows
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            int candidateID = Convert.ToInt32(row["Candidate_ID"]);
                            string candidateName = row["Candidate_Name"].ToString();
                            int totalVotes = Convert.ToInt32(row["Total_Votes"]);

                            // Check if the total votes fulfill the quota
                            if (totalVotes >= finalQuota)
                            {
                                // Calculate surplus votes
                                int surplusVotes = totalVotes - finalQuota;

                                // Distribute surplus votes
                                DistributeSurplusVotes(candidateID, surplusVotes);

                                // Update the seat status of the elected candidate in the database
                                query = "UPDATE candidate SET seat = '" + elected + "' WHERE cid = '" + candidateID + "'";
                                fn.setData(query, "Set Seat to Candidate");

                                // Reduce the total number of seats by 1
                                totalseats--;

                                // Display a message indicating the elected candidate
                                MessageBox.Show("The Candidate Elected is '" + candidateName + "'", "Elected Vote", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Check if all seats are filled
                                if (totalseats == 0)
                                    break; // Exit the loop if all seats are filled
                            }
                        }
                    }
                    else
                    {
                        // If no candidates meet the quota, eliminate candidates with the fewest votes
                        EliminateCandidateWithFewestVotes();
                    }
                }
                string query1 = "SELECT cid, cname FROM candidate WHERE seat ='"+elected+"'";
                DataSet ds = fn.getData(query1);
                dataGridView1.DataSource = ds.Tables[0];
            }
            else
            {
                MessageBox.Show("Please enter the Postal Code.");
            }
        }
        private void DistributeSurplusVotes(int candidateID, int surplusVotes)
        {
            // Retrieve next preference candidates and their respective vote counts
            string query = "SELECT c.cid, MAX(preferences) + 1 AS nextPreferenceOrder FROM voting v " +
               "JOIN candidate c ON v.cid = c.cid " +
               "WHERE preferences > 1 AND c.seat != 'eliminated' " + 
               "GROUP BY c.cid ORDER BY nextPreferenceOrder ASC";

            DataSet nextPreferenceDataSet = fn.getData(query);
            DataTable nextPreferenceDataTable = nextPreferenceDataSet.Tables[0];

            // Distribute surplus votes proportionally among next preference candidates
            while (surplusVotes > 0 && nextPreferenceDataTable.Rows.Count > 0)
            {
                // List to store next preference candidates and their preference orders
                List<int> candidateIDs = new List<int>();
                List<int> nextPreferenceOrders = new List<int>();

                // Populate candidateIDs and nextPreferenceOrders lists
                foreach (DataRow nextPrefRow in nextPreferenceDataTable.Rows)
                {
                    int nextPreferenceCandidateID = Convert.ToInt32(nextPrefRow["cid"]);
                    int nextPreferenceOrder = GetNextPreferenceOrder(nextPreferenceCandidateID); // Use GetNextPreferenceOrder function

                    candidateIDs.Add(nextPreferenceCandidateID);
                    nextPreferenceOrders.Add(nextPreferenceOrder);
                }

                // Calculate the total next preference order among the next preference candidates
                int totalNextPreferenceOrder = nextPreferenceOrders.Sum();

                // Distribute surplus votes proportionally among next preference candidates
                for (int i = 0; i < candidateIDs.Count; i++)
                {
                    // Calculate the number of surplus votes to distribute to the current candidate
                    int votesToDistribute = (int)((double)surplusVotes * (nextPreferenceOrders[i] / (double)totalNextPreferenceOrder));

                    // Ensure that each candidate receives at least one vote
                    votesToDistribute = Math.Max(votesToDistribute, 1);

                    // Update the preferences of the candidate based on their next preference order
                    string updateVotesQuery = "UPDATE voting SET preferences = " + nextPreferenceOrders[i] + " WHERE cid = '" + candidateIDs[i] + "'";
                    fn.setData(updateVotesQuery, "Distribute Votes");

                    // Reduce surplus votes
                    surplusVotes -= votesToDistribute;

                    // Remove distributed votes from the candidate's total
                    nextPreferenceOrders[i] -= votesToDistribute;

                    // Remove candidate if they have received all their votes
                    if (nextPreferenceOrders[i] == 0)
                    {
                        nextPreferenceOrders.RemoveAt(i);
                        candidateIDs.RemoveAt(i);
                        i--; // Adjust index due to removal
                    }

                    // Check if surplus votes are fully distributed
                    if (surplusVotes == 0)
                        break;
                }

                // Retrieve next preference candidates and their respective vote counts again for the next iteration
                nextPreferenceDataSet = fn.getData(query);
                nextPreferenceDataTable = nextPreferenceDataSet.Tables[0];
            }
        }
        private int GetNextPreferenceOrder(int candidateID)
        {
            // Retrieve the next preference order for the candidate
            string query = "SELECT MAX(preferences) + 1 AS nextPreferenceOrder FROM voting WHERE cid = '" + candidateID + "'";
            DataSet nextPreferenceOrderDataSet = fn.getData(query);
            int nextPreferenceOrder = Convert.ToInt32(nextPreferenceOrderDataSet.Tables[0].Rows[0]["nextPreferenceOrder"]);

            // Ensure that the next preference order is at least 1
            return Math.Max(nextPreferenceOrder, 1);
        }
        private void EliminateCandidateWithFewestVotes()
        {
                // Retrieve all active candidates and their respective vote counts
                string query = "SELECT v.cid, COUNT(*) AS TotalVotes FROM voting v JOIN candidate c ON v.cid = c.cid WHERE c.seat != '"+"eliminated"+"' GROUP BY v.cid ORDER BY TotalVotes ASC";
                DataSet candidateVotesDataSet = fn.getData(query);
                DataTable candidateVotesTable = candidateVotesDataSet.Tables[0];

                // Check if there are active candidates to eliminate
                if (candidateVotesTable.Rows.Count > 0)
                {
                    // Find the candidate with the fewest votes
                    DataRow candidateWithFewestVotes = candidateVotesTable.Rows[0];
                    int fewestVotes = Convert.ToInt32(candidateWithFewestVotes["TotalVotes"]);
                    int candidateID = Convert.ToInt32(candidateWithFewestVotes["cid"]);

                    // Handle tie-break situations (if necessary)

                    // Update the elimination status of the candidate with the fewest votes
                    string updateStatusQuery = "UPDATE candidate SET seat ='"+ elected +"' WHERE cid = '" + candidateID + "'";
                    fn.setData(updateStatusQuery, "Mark Candidate as Eliminated");

                    // Update preferences of voters who selected the eliminated candidate as their first choice
                    string updatePreferencesQuery = "UPDATE voting SET preferences = preferences - 1 WHERE cid = '" + candidateID + "'";
                    fn.setData(updatePreferencesQuery, "Update Voter Preferences");

                    // Distribute surplus votes of the eliminated candidate among the remaining candidates
                    DistributeSurplusVotes(candidateID, fewestVotes);
                }
                else
                {
                    Console.WriteLine("No active candidates to eliminate.");
                }
        }


    }
}
