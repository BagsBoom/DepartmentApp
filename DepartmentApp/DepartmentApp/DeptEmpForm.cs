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
    public partial class DeptEmpForm : Form
    {

        private MySqlCommandBuilder mySqlCommandBuilder = null;
        private MySqlDataAdapter mySqlDataAdapter = null;
        private DataSet dataSet = null;
        public DeptEmpForm()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM dept_emp";
                MySqlConnection con = new MySqlConnection(DataBase.connectionSql());

                mySqlDataAdapter = new MySqlDataAdapter(query, con);
                mySqlCommandBuilder = new MySqlCommandBuilder(mySqlDataAdapter);

                dataSet = new DataSet();

                mySqlDataAdapter.Fill(dataSet, "dept_emp");

                dataGridViewDeptEmp.DataSource = dataSet.Tables["dept_emp"];

                dataGridViewDeptEmp.Columns[0].HeaderText = "Employee ID";
                dataGridViewDeptEmp.Columns[1].HeaderText = "Department ID";
                dataGridViewDeptEmp.Columns[2].HeaderText = "From date";
                dataGridViewDeptEmp.Columns[3].HeaderText = "To date";

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

        private void LoadCombo_2()
        {
            string query = "SELECT * FROM departments";
            MySqlConnection con = new MySqlConnection(DataBase.connectionSql());
            MySqlCommand cmdDatabase = new MySqlCommand(query, con);
            MySqlDataReader dataReader;
            try
            {
                con.Open();
                dataReader = cmdDatabase.ExecuteReader();
                while (dataReader.Read())
                {
                    string emp = dataReader.GetString("dept_no");
                    comboBox2.Items.Add(emp);
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
                dataSet.Tables["dept_emp"].Clear();

                mySqlDataAdapter.Fill(dataSet, "dept_emp");

                dataGridViewDeptEmp.DataSource = dataSet.Tables["dept_emp"];
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void DeptEmpForm_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadCombo();
            LoadCombo_2();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            DataRow row = dataSet.Tables["dept_emp"].NewRow();

            if (comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("Error! Fill all the text boxes please!");
            }
            else
            {
                try
                {
                    row["emp_no"] = comboBox1.Text;
                    row["dept_no"] = comboBox2.Text;
                    row["from_date"] = dateTimePicker1.Text;
                    row["to_date"] = dateTimePicker2.Text;
                    dataSet.Tables["dept_emp"].Rows.Add(row);

                    mySqlDataAdapter.Update(dataSet, "dept_emp");
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
                int nRowIndex = dataGridViewDeptEmp.Rows.Count - 1;

                dataGridViewDeptEmp.FirstDisplayedScrollingRowIndex = nRowIndex;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete this record?", "Deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                int rowIndex = dataGridViewDeptEmp.CurrentCell.RowIndex;

                dataGridViewDeptEmp.Rows.RemoveAt(rowIndex);
                dataSet.Tables["dept_emp"].Rows[rowIndex].Delete();

                mySqlDataAdapter.Update(dataSet, "dept_emp");
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            int x = dataGridViewDeptEmp.CurrentCell.RowIndex;

            if (comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("Error! Fill all the text boxes please!");
            }
            else
            {
                dataGridViewDeptEmp.Rows[x].Cells["emp_no"].Value = comboBox1.Text;
                dataGridViewDeptEmp.Rows[x].Cells["dept_no"].Value = comboBox2.Text;
                dataGridViewDeptEmp.Rows[x].Cells["from_date"].Value = dateTimePicker1.Text;
                dataGridViewDeptEmp.Rows[x].Cells["to_date"].Value = dateTimePicker2.Text;

                mySqlDataAdapter.Update(dataSet, "dept_emp");
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            comboBox2.Text = "";
            dateTimePicker1.Text = DateTime.Now.ToString().Substring(0, 10);
            dateTimePicker2.Text = DateTime.Now.ToString().Substring(0, 10);
        }

        private void dataGridViewDeptEmp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int x = e.RowIndex;
            if (e.RowIndex != -1)
            {
                comboBox1.Text = dataGridViewDeptEmp.Rows[x].Cells["emp_no"].Value.ToString();
                comboBox2.Text = dataGridViewDeptEmp.Rows[x].Cells["dept_no"].Value.ToString();
                if (dataGridViewDeptEmp.Rows[x].Cells["from_date"].Value.ToString().Substring(0, 10) != "")
                    dateTimePicker1.Text = dataGridViewDeptEmp.Rows[x].Cells["from_date"].Value.ToString().Substring(0, 10);
                if (dataGridViewDeptEmp.Rows[x].Cells["from_date"].Value.ToString().Substring(0, 10) != "")
                    dateTimePicker2.Text = dataGridViewDeptEmp.Rows[x].Cells["to_date"].Value.ToString().Substring(0, 10);
            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
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

        private void departmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var departments = new departmentsForm();
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
    }
}
