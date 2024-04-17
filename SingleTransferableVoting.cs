using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ElectronicVotingSystem
{
    public partial class SingleTransferableVoting : Form
    {
        db fn = db.GetInstance();
        DialogResult result;
        string postalCode;

        public SingleTransferableVoting()
        {
            InitializeComponent();
            postalcode.ReadOnly = true;
        }

        private void SingleTransferableVoting_Load(object sender, EventArgs e)
        {
            string query = "SELECT DISTINCT postalcode FROM Candidate";
            DataSet ds = fn.getData(query);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                //to get the postal code
                postalCode = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                postalcode.Text = postalCode;
            }    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            ElectingSeats electingSeats = new ElectingSeats();

            // Show the form
            electingSeats.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void seats_TextChanged(object sender, EventArgs e)
        {

        }
        private void SetSeats()
        {
            string Seats = seats.Text.ToString();
            result = MessageBox.Show("Do you want to confirm the Number of seats", "Confirm Seats", MessageBoxButtons.YesNo, MessageBoxIcon.Information);


            // Check the user's selection
            if (result == DialogResult.Yes)
            {
                if (!string.IsNullOrWhiteSpace(Seats))
                {
                    //first check that the same user votes to same candidate:
                    string query = "UPDATE singletvote SET seats = '" + Convert.ToInt32(Seats) + "' WHERE postalcode = '" + postalCode + "'";
                    fn.setData(query, "Updated SuccessFully");
                }
                else
                {
                    MessageBox.Show("Plx Fill the Seats Field");
                }
            }
            else if (result == DialogResult.No)
            {
                MessageBox.Show("Not Update Seats", "Seats not Placed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    
        private void enter_Click(object sender, EventArgs e)
        {
            SetSeats();
        }   
    }
}
