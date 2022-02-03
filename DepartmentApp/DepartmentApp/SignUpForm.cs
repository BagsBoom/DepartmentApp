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
    public partial class SignUpForm : Form
    {
        public SignUpForm()
        {
            InitializeComponent();
        }

        private void ReturnToLoginLabel_Click(object sender, EventArgs e)
        {
            this.Hide();
            var Main = new Login_Form();
            Main.Closed += (s, args) => this.Close();
            Main.Show();
        }

        private void buttonSignUp_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(DataBase.connectionSqlLogin());
            MySqlCommand cmd = new MySqlCommand("INSERT into loginTable(idlogin, pass) VALUES (@idlogin, @pass)", con);
            cmd.Parameters.AddWithValue("@idlogin", textBoxLogin.Text);
            cmd.Parameters.AddWithValue("@pass", textBoxPassword.Text);

            if (textBoxLogin.Text == "")
            {
                MessageBox.Show("Please enter values");
                textBoxLogin.Focus();
                return;
            }

            else if (textBoxPassword.Text == "" || textBoxPasswordConfirm.Text == "")
            {
                MessageBox.Show("Please enter values");
                textBoxPassword.Focus();
                return;
            }

            else if (textBoxPassword.Text != textBoxPasswordConfirm.Text)
            {
                MessageBox.Show("Password not matching");
                textBoxPasswordConfirm.Focus();
                return;
            }

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Your account has been registered!");

            this.Hide();
            var Main = new Login_Form();
            Main.Closed += (s, args) => this.Close();
            Main.Show();
        }
    }
}
