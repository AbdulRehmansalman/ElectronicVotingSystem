using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace ElectronicVotingSystem
{
    public class db
    {
        protected SqlConnection getConnection()
        {
            SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=evoting; Integrated Security=True");
            return con;
        }

        // this function will return the query
        // this function accept the connection string
        public DataSet getData(string query)
        {
            SqlConnection con = getConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = query;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            // here we have filled the dataset object with the data
            // coming form the database
            da.Fill(ds);

            con.Close();
            return ds;
        }

        // function to set the data

        public void setData(string query, string message)
        {
            SqlConnection con = getConnection();
            SqlCommand cmd = new SqlCommand();

            //  passing the connection  in the cmd;
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        public string ComputeHash(string input)
        {
            //hash password with sha256
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                // conversion into byte to hexadecimal
                builder.Append(bytes[i].ToString("x2"));
            }
            string hashedString = builder.ToString();
            // remove SHA256 instance 
            sha256.Dispose();

            return hashedString;
        }
        //to call Protected Connection in Sign in Class
        public SqlConnection GetConnectionForExternal()
        {
            return getConnection();
        }
    }
}
