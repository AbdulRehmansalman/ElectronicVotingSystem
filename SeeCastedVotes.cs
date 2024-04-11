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
    public partial class SeeCastedVotes : Form
    {
        private int userId;
        db fn = db.GetInstance();
        public SeeCastedVotes(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void SeeCastedVotes_Load(object sender, EventArgs e)
        {
            string query = "select u.id,u.name as user_name,c.cname from voting v inner join users u ON v.userid = u.id inner join candidate c ON v.cid = c.cid where v.userid = '"+ userId + "'";
            DataSet dataSet = fn.getData(query);
            dataGridView1.DataSource = dataSet.Tables[0];
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
