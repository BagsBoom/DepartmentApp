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
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace DepartmentApp
{
    public partial class salariesForm : Form
    {
        private MySqlCommandBuilder mySqlCommandBuilder = null;
        private MySqlDataAdapter mySqlDataAdapter = null;
        private DataSet dataSet = null;
        private MySqlCommand cmd;
        
        public salariesForm()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM salaries";
                MySqlConnection con = new MySqlConnection(DataBase.connectionSql());

                mySqlDataAdapter = new MySqlDataAdapter(query, con);
                mySqlCommandBuilder = new MySqlCommandBuilder(mySqlDataAdapter);

                dataSet = new DataSet();

                mySqlDataAdapter.Fill(dataSet, "salaries");

                dataGridViewSalaries.DataSource = dataSet.Tables["salaries"];

                dataGridViewSalaries.Columns[0].HeaderText = "Employee ID";
                dataGridViewSalaries.Columns[1].HeaderText = "Salary";
                dataGridViewSalaries.Columns[2].HeaderText = "From date";
                dataGridViewSalaries.Columns[3].HeaderText = "To date";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void LoadCombo()
        {
            string query = "SELECT * FROM employees";
            MySqlConnection con = new MySqlConnection(DataBase.connectionSql());
            MySqlCommand cmdDatabase = new MySqlCommand(query, con);
            MySqlDataReader dataReader;
            try
            {
                con.Open();
                dataReader = cmdDatabase.ExecuteReader();
                while (dataReader.Read())
                {
                    string emp = dataReader.GetString("emp_no");
                    comboBox1.Items.Add(emp);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }

        }

        private void ReloadData()
        {
            try
            {
                dataSet.Tables["salaries"].Clear();

                mySqlDataAdapter.Fill(dataSet, "salaries");

                dataGridViewSalaries.DataSource = dataSet.Tables["salaries"];
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void salariesForm_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadCombo();
        }


        private void AddButton_Click(object sender, EventArgs e)
        {

            DataRow row = dataSet.Tables["salaries"].NewRow();

            if (comboBox1.Text == "" || numericUpDown1.Value < 1)
            {
                MessageBox.Show("Error! Fill all the text boxes please with correct data!");
            }
            else
            {
                try
                {
                    row["emp_no"] = comboBox1.Text;
                    row["salary"] = numericUpDown1.Text;
                    row["from_date"] = dateTimePicker1.Text;
                    row["to_date"] = dateTimePicker2.Text;
                    dataSet.Tables["salaries"].Rows.Add(row);

                    mySqlDataAdapter.Update(dataSet, "salaries");
                }
                catch (MySqlException ex)
                {
                    if (ex.Number > 0)
                    {
                        ReloadData();
                        MessageBox.Show("Please, check the ID which you want to add!");
                        return;
                    }
                }

                    int nRowIndex = dataGridViewSalaries.Rows.Count - 1;

                    dataGridViewSalaries.FirstDisplayedScrollingRowIndex = nRowIndex;
                }

        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            numericUpDown1.Value = 0;
            dateTimePicker1.Text = DateTime.Now.ToString().Substring(0, 10);
            dateTimePicker2.Text = DateTime.Now.ToString().Substring(0, 10);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete this record?", "Deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
               == DialogResult.Yes)
            {
                int rowIndex = dataGridViewSalaries.CurrentCell.RowIndex;

                dataGridViewSalaries.Rows.RemoveAt(rowIndex);
                dataSet.Tables["salaries"].Rows[rowIndex].Delete();

                mySqlDataAdapter.Update(dataSet, "salaries");
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            int x = dataGridViewSalaries.CurrentCell.RowIndex;
            if (comboBox1.Text == "" || numericUpDown1.Value < 1)
            {
                MessageBox.Show("Error! Fill all the text boxes please!");
            }
            else
            {
                try
                {
                    dataGridViewSalaries.Rows[x].Cells["emp_no"].Value = comboBox1.Text;
                    dataGridViewSalaries.Rows[x].Cells["salary"].Value = numericUpDown1.Text;
                    dataGridViewSalaries.Rows[x].Cells["from_date"].Value = dateTimePicker1.Text;
                    dataGridViewSalaries.Rows[x].Cells["to_date"].Value = dateTimePicker2.Text;

                    mySqlDataAdapter.Update(dataSet, "salaries");
                }
                catch (MySqlException ex)
                {
                    if (ex.Number > 0)
                    {
                        ReloadData();
                        MessageBox.Show("Please, check the ID which you want to add!");
                        return;
                    }
                }
            }
        }

        private void dataGridViewSalaries_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int x = e.RowIndex;
            if (e.RowIndex != -1)
            {
                comboBox1.Text = dataGridViewSalaries.Rows[x].Cells["emp_no"].Value.ToString();
                numericUpDown1.Text = dataGridViewSalaries.Rows[x].Cells["salary"].Value.ToString();
                if(dataGridViewSalaries.Rows[x].Cells["from_date"].Value.ToString().Substring(0, 10) != "")
                    dateTimePicker1.Text = dataGridViewSalaries.Rows[x].Cells["from_date"].Value.ToString().Substring(0, 10);
                if (dataGridViewSalaries.Rows[x].Cells["to_date"].Value.ToString().Substring(0, 10) != "")
                    dateTimePicker2.Text = dataGridViewSalaries.Rows[x].Cells["to_date"].Value.ToString().Substring(0, 10);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    (dataGridViewSalaries.DataSource as DataTable).DefaultView.RowFilter = $"salary > 0";
                    break;
                case 1:
                    (dataGridViewSalaries.DataSource as DataTable).DefaultView.RowFilter = $"salary <= 10000";
                    break;
                case 2:
                    (dataGridViewSalaries.DataSource as DataTable).DefaultView.RowFilter = $"salary > 10000 AND salary <= 25000";
                    break;
                case 3:
                    (dataGridViewSalaries.DataSource as DataTable).DefaultView.RowFilter = $"salary > 25000 AND salary <= 50000";
                    break;
                case 4:
                    (dataGridViewSalaries.DataSource as DataTable).DefaultView.RowFilter = $"salary > 50000 AND salary <= 100000";
                    break;
                case 5:
                    (dataGridViewSalaries.DataSource as DataTable).DefaultView.RowFilter = $"salary > 100000";
                    break;
            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void EmployeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var departments = new employeesForm();
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

        private void DepartmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var departments = new departmentsForm();
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

        private void dataGridViewSalaries_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Tab)
            {
                comboBox1.Text = dataGridViewSalaries.CurrentRow.Cells["emp_no"].Value.ToString();
                numericUpDown1.Text = dataGridViewSalaries.CurrentRow.Cells["salary"].Value.ToString();
                if (dataGridViewSalaries.CurrentRow.Cells["from_date"].Value.ToString().Substring(0, 10) != "")
                    dateTimePicker1.Text = dataGridViewSalaries.CurrentRow.Cells["from_date"].Value.ToString().Substring(0, 10);
                if (dataGridViewSalaries.CurrentRow.Cells["to_date"].Value.ToString().Substring(0, 10) != "")
                    dateTimePicker2.Text = dataGridViewSalaries.CurrentRow.Cells["to_date"].Value.ToString().Substring(0, 10);
            }
        }
    }
}
