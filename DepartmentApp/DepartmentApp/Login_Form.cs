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
    public partial class Login_Form : Form
    {
        public Login_Form()
        {
            InitializeComponent();
        }

        // login button
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(DataBase.connectionSqlLogin());
            MySqlCommand com = new MySqlCommand("SELECT * FROM loginTable WHERE idlogin=@idlogin and pass=@pass", con);
            con.Open();
            com.Parameters.AddWithValue("@idlogin", textBoxLogin.Text);
            com.Parameters.AddWithValue("@pass", textBoxPassword.Text);
            MySqlDataReader Dr = com.ExecuteReader();
            if (Dr.HasRows == true)
            {
                this.Hide();
                var Main = new employeesForm();
                Main.Closed += (s, args) => this.Close();
                Main.Show();
            }
            else
            {
                MessageBox.Show("Wrong login or password!");
            }
        }

        // sign up label
        private void SignUpLabel_Click(object sender, EventArgs e)
        {
            this.Hide();
            var Main = new SignUpForm();
            Main.Closed += (s, args) => this.Close();
            Main.Show();
        }
    }
}
