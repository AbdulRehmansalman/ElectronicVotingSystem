using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ElectronicVotingSystem
{
    public partial class SignUpForm : Form
    {
        db fn = db.GetInstance();
        public SignUpForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Name = name.Text;
            string Username = username.Text;
            string Password = password.Text;
            string Contact = contact.Text;
            string PostalCode=postalCode.Text;
            string role = "user";
            if (ValidationProxy.ValidateSignUpFields(Name, Username, Password, Contact, PostalCode))
            {
                //to check that Duplicate Data Not Inserted
                string query = "select COUNT(*) from Users where name ='" + Name + "' AND postalcode = '" + PostalCode + "' ";
                DataSet ds = fn.getData(query);
                int count = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                if (count > 0)
                {
                    MessageBox.Show("User Already Exists");
                }
                else
                {
                    string hashPassword = fn.ComputeHash(Password);
                    query = "INSERT INTO users (name, username, password, contact,postalcode, role) VALUES ('" + Name + "','" + Username + "','" + hashPassword + "','" + Contact + "','" + PostalCode + "','" + role + "')";
                    fn.setData(query, "User Record Inserted SuccessFully");

                    // to Navigate to Sign in form when SuccesFull
                    SignInform signInform = new SignInform();
                    signInform.Show();
                    this.Hide();
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

        private void button2_Click(object sender, EventArgs e)
        {
            SignInform signInform = new SignInform();
            signInform.Show();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex) {
                case 0:
                    Thread.CurrentThread.CurrentUICulture =new System.Globalization.CultureInfo("en");
                    break;
                case 1:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-GB");
                   
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

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
