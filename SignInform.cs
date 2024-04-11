using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Xml.Linq;
using System.Collections;
using System.Data.SqlClient;
using System.Threading;

namespace ElectronicVotingSystem
{
    public partial class SignInform : Form
    {
        db fn = db.GetInstance();
        DataSet dataSet = new DataSet();

        public SignInform()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
             string Username = username.Text;
             string Password = password.Text;
             int userId = 0;
             string role = "";
            //Validate Fields
            if (Validation.ValidateLoginFields(Username, Password))
            {
                
                // SQL Query 
                 string query = "SELECT id, role, password FROM Users WHERE username = @Username";
                bool loginSuccess = false;

                try
                {
                    // Establishing connection from db class  
                    using (SqlConnection connection = fn.GetConnectionForExternal())
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Add parameters 
                            command.Parameters.AddWithValue("@Username", Username);
                            
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    userId = reader.GetInt32(reader.GetOrdinal("id"));
                                    role = reader["role"].ToString();
                                    string hashOrSimplePassFromDb = reader["password"].ToString();
                                    //For Admin Person add
                                    if(hashOrSimplePassFromDb == Password) { 
                                        loginSuccess = true;
                                    }
                                    else
                                    {
                                        // Hash the password entered by the user
                                        string hashPassEntered = fn.ComputeHash(Password);

                                        if (hashOrSimplePassFromDb == hashPassEntered)
                                        {
                                            loginSuccess = true;
                                        }
                                        else
                                        {
                                            loginSuccess = false;
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    MessageBox.Show("User not Found");
                                    loginSuccess = false;
                                }
                            }
                        }
                    }
                    // Check if the login was successful
                    if (loginSuccess)
                    {
                        MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // to Navigate through Various Roles

                        if (role.Equals("admin"))
                        {
                            admin a1 = new admin();
                            a1.Show();
                            this.Hide();
                        }
                        else if (role.Equals("candidate"))
                        {
                            CandidateResults results = new CandidateResults(userId);
                            results.Show();
                            this.Hide();
                        }
                        else  // If it is user
                        {
                            // pass user id in constructor because it can't be changed after obj creat

                            Dashboard dashboard = new Dashboard(userId);
                            dashboard.Show();
                            this.Hide();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                    break;
                case 1:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");

                    break;
                case 2:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR");
                    break;
                case 3:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-PT");
                    break;
            }

            this.Controls.Clear();
            InitializeComponent();
        }
    }
}
