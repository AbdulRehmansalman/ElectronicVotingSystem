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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ElectronicVotingSystem
{
    public partial class CreateCandidate : Form
    {
        db fn =  new db();
        DataSet dataSet = new DataSet();

        public CreateCandidate()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Name = name.Text;
            string Username = username.Text;
            string Password = password.Text;
            string Contact = contact.Text;
            string VotingSign = votingSign.Text;
            string PartyName =  partyname.SelectedItem as string;
            string PostalCode = postalCode.Text;
            string role = "candidate";
            if (Validation.CreateCandidateValidate(Name, Username, Password, Contact, VotingSign, PartyName, PostalCode))
            {
                //No more than one Candidate Exist
                string query = "SELECT COUNT(*) FROM candidate WHERE cname = '" + Name + "' AND postalcode = '" + PostalCode + "'";
                DataSet candidateExistsDS = fn.getData(query);
                int candidateCount = Convert.ToInt32(candidateExistsDS.Tables[0].Rows[0][0]);

                if (candidateCount > 0)
                {
                    MessageBox.Show("Candidate already exists.");
                }
                else
                {
                    // First Hash the password and Insert into users For Candidate Record
                    string hashPassword = fn.ComputeHash(Password);
                    query = "INSERT INTO users (name,username, password,contact, role) VALUES ('" + Name + "','" + Username + "','" + hashPassword + "','" + Contact + "','" + role + "')";
                    fn.setData(query, "Inserted SuccesFully");

                    // to insert get id by Party Name into Candidate Tables
                    query = "select * from party where partname = '" + PartyName + "'";
                    dataSet = fn.getData(query);
                    DataRow[] rows = dataSet.Tables[0].Select("partname = '" + PartyName + "'");

                    // to get the userID to identify Candidate
                    query = "SELECT MAX(id) FROM Users";
                    dataSet = fn.getData(query);
                    int maxUserId = Convert.ToInt32(dataSet.Tables[0].Rows[0][0]);

                    // Check if a matching row is found
                    if (rows.Length > 0)
                    {
                        int pId = Convert.ToInt32(rows[0]["pid"]);
                        query = "INSERT INTO candidate (cname, votingsign, pid, userid, postalcode) VALUES ('" + Name + "','" + votingSign + "'," + pId + "," + maxUserId + ",'" + PostalCode + "')";
                        fn.setData(query, "Candidate Created SuccesFully");
                        Clear();
                    }
                    else
                    {
                        MessageBox.Show("The Name Does not found");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*";
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // Load the selected image into the PictureBox
                            pictureBox1.Image = new System.Drawing.Bitmap(openFileDialog.FileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error loading image: " + ex.Message);
                        }
                    }
                }
            }
        }
       

        private void Clear()
        {
            name.Clear();
            username.Clear();
            password.Clear();
            contact.Clear();
            votingSign.Clear();
            partyname.SelectedItem = null;
           postalCode.Clear();
        }
        private void createCandidate_Load(object sender, EventArgs e)
        {
            // get Names of the party
            string query = "select * from party";
            dataSet = fn.getData(query);

            partyname.Items.Clear();

            // Loop through the retrieved data and fill ComboBox
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                string itemName = row[1].ToString();
                // Add item to the ComboBox
                partyname.Items.Add(itemName);
            }
        }
    }
}
