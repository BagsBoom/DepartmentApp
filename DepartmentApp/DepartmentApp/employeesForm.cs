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
    public partial class employeesForm : Form
    {
        private MySqlCommandBuilder mySqlCommandBuilder = null;
        private MySqlDataAdapter mySqlDataAdapter = null;
        private DataSet dataSet = null;

        public employeesForm()
        {
            InitializeComponent();
            
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM employees";
                MySqlConnection con = new MySqlConnection(DataBase.connectionSql());

                mySqlDataAdapter = new MySqlDataAdapter(query, con);
                mySqlCommandBuilder = new MySqlCommandBuilder(mySqlDataAdapter);

                dataSet = new DataSet();

                mySqlDataAdapter.Fill(dataSet, "employees");

                dataGridViewEmployees.DataSource = dataSet.Tables["employees"];
                dataGridViewEmployees.Columns[0].HeaderText = "Employee Id";
                dataGridViewEmployees.Columns[1].HeaderText = "First Name";
                dataGridViewEmployees.Columns[2].HeaderText = "Last Name";
                dataGridViewEmployees.Columns[3].HeaderText = "Birth Day";
                dataGridViewEmployees.Columns[4].HeaderText = "Gender";
                dataGridViewEmployees.Columns[5].HeaderText = "Hire Date";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ReloadData()
        {
            try
            {
                dataSet.Tables["employees"].Clear();

                mySqlDataAdapter.Fill(dataSet, "employees");

                dataGridViewEmployees.DataSource = dataSet.Tables["employees"];
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void employeesForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }


        private void AddButton_Click(object sender, EventArgs e)
        {

            DataRow row = dataSet.Tables["employees"].NewRow();

            if(textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Error! Fill all the text boxes please!");
            }
            else 
            {
                row["first_name"] = textBox1.Text;
                row["last_name"] = textBox2.Text;
                row["birth_day"] = dateTimePicker1.Text;
                row["gender"] = textBox3.Text;
                row["hire_date"] = dateTimePicker2.Text;
            
                dataSet.Tables["employees"].Rows.Add(row);
            
                mySqlDataAdapter.Update(dataSet, "employees");
                ReloadData();
                int nRowIndex = dataGridViewEmployees.Rows.Count - 1;

                dataGridViewEmployees.FirstDisplayedScrollingRowIndex = nRowIndex;
            }
        }

        private void dataGridViewEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int x = e.RowIndex;
            if (e.RowIndex != -1)
            {
                textBox1.Text = dataGridViewEmployees.Rows[x].Cells["first_name"].Value.ToString();
                textBox2.Text = dataGridViewEmployees.Rows[x].Cells["last_name"].Value.ToString();
                if (dataGridViewEmployees.Rows[x].Cells["birth_day"].Value.ToString().Substring(0, 10) != "")
                    dateTimePicker1.Text = dataGridViewEmployees.Rows[x].Cells["birth_day"].Value.ToString().Substring(0, 10);
                textBox3.Text = dataGridViewEmployees.Rows[x].Cells["gender"].Value.ToString();
                if (dataGridViewEmployees.Rows[x].Cells["hire_date"].Value.ToString().Substring(0, 10) != "")
                    dateTimePicker2.Text = dataGridViewEmployees.Rows[x].Cells["hire_date"].Value.ToString().Substring(0, 10);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            dateTimePicker1.Text = DateTime.Now.ToString().Substring(0,10);
            textBox3.Text = "";
            dateTimePicker2.Text = DateTime.Now.ToString().Substring(0, 10);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete this record?", "Deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                           == DialogResult.Yes)
            {
                int rowIndex = dataGridViewEmployees.CurrentCell.RowIndex;

                dataGridViewEmployees.Rows.RemoveAt(rowIndex);
                dataSet.Tables["employees"].Rows[rowIndex].Delete();

                mySqlDataAdapter.Update(dataSet, "employees");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int x = dataGridViewEmployees.CurrentCell.RowIndex;
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Error! Fill all the text boxes please!");
            }
            else
            {
                dataGridViewEmployees.Rows[x].Cells["first_name"].Value = textBox1.Text;
                dataGridViewEmployees.Rows[x].Cells["last_name"].Value = textBox2.Text;
                dataGridViewEmployees.Rows[x].Cells["birth_day"].Value = dateTimePicker1.Text;
                dataGridViewEmployees.Rows[x].Cells["gender"].Value = textBox3.Text;
                dataGridViewEmployees.Rows[x].Cells["hire_date"].Value = dateTimePicker2.Text;

                mySqlDataAdapter.Update(dataSet, "employees");
            }
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            (dataGridViewEmployees.DataSource as DataTable).DefaultView.RowFilter =
                $"last_name LIKE '%{searchBox.Text}%' OR first_name LIKE '%{searchBox.Text}%' OR gender LIKE '%{searchBox.Text}%' OR CONVERT(emp_no, System.String) LIKE '%{searchBox.Text}%' OR CONVERT (birth_day, System.String) LIKE '%{searchBox.Text}%' OR CONVERT (hire_date, System.String) LIKE '%{searchBox.Text}%'";  
        }

        private void departmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var departments = new departmentsForm();
            departments.Closed += (s, args) => this.Close();
            departments.Show();
        }

        private void departmentEmployeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var departments = new DeptEmpForm();
            departments.Closed += (s, args) => this.Close();
            departments.Show();
        }

        private void departmentManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var departments = new DeptManagerForm();
            departments.Closed += (s, args) => this.Close();
            departments.Show();
        }

        private void salariesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var departments = new salariesForm();
            departments.Closed += (s, args) => this.Close();
            departments.Show();
        }

        private void titlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var departments = new titlesForm();
            departments.Closed += (s, args) => this.Close();
            departments.Show();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }
    }
}
