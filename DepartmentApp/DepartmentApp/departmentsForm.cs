using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DepartmentApp
{
    public partial class departmentsForm : Form
    {
        public departmentsForm()
        {
            InitializeComponent();
            LoadData();
        }
        
        private void LoadData()
        {   
            MySqlConnection con = new MySqlConnection(DataBase.connectionSql());
            con.Open();

            string query = "SELECT * FROM departments";
            MySqlCommand command = new MySqlCommand(query, con);
            MySqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();

            while(reader.Read())
            {
                data.Add(new string[2]);
                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
            }
            reader.Close();
            con.Close();

            foreach (string[] s in data)
                dataGridViewDepartments.Rows.Add(s);
        }
    }
}
