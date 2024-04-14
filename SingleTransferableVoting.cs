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
    public partial class SingleTransferableVoting : Form
    {
        db fn = db.GetInstance();
        DialogResult result;

        public SingleTransferableVoting()
        {
            InitializeComponent();
        }

        private void SingleTransferableVoting_Load(object sender, EventArgs e)
        {
            string query = "SELECT MIN(cid) AS cid, postalcode FROM Candidate GROUP BY postalcode;\r\n";
            DataSet ds = fn.getData(query);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                //to get the postal code
                string postalCode = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                postalcode.Text = postalCode;
                postalcode.ReadOnly = true;
                 string Seats = seats.Text.ToString();
                result = MessageBox.Show("Do you want to confirm the Number of seats", "Confirm Seats", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                

                // Check the user's selection
                if (result == DialogResult.Yes)
                {
                    if (seats.Text != " ")
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
        }
    }
}
