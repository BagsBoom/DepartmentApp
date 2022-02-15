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

        private MySqlCommandBuilder mySqlCommandBuilder = null;
        private MySqlDataAdapter mySqlDataAdapter = null;
        private DataSet dataSet = null;
        public departmentsForm()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM departments";
                MySqlConnection con = new MySqlConnection(DataBase.connectionSql());

                mySqlDataAdapter = new MySqlDataAdapter(query, con);
                mySqlCommandBuilder = new MySqlCommandBuilder(mySqlDataAdapter);

                dataSet = new DataSet();

                mySqlDataAdapter.Fill(dataSet, "departments");

                dataGridViewDepartments.DataSource = dataSet.Tables["departments"];

                dataGridViewDepartments.Columns[0].HeaderText = "Department Id";
                dataGridViewDepartments.Columns[1].HeaderText = "Department Name";

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
                dataSet.Tables["departments"].Clear();

                mySqlDataAdapter.Fill(dataSet, "departments");

                dataGridViewDepartments.DataSource = dataSet.Tables["departments"];
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void departmentsForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            DataRow row = dataSet.Tables["departments"].NewRow();
            if (textBox1.Text == "")
            {
                MessageBox.Show("Error! Fill all the text boxes please!");
            }
            else
            {
                row["dept_name"] = textBox1.Text;

                dataSet.Tables["departments"].Rows.Add(row);

                mySqlDataAdapter.Update(dataSet, "departments");
                ReloadData();
                int nRowIndex = dataGridViewDepartments.Rows.Count - 1;

                dataGridViewDepartments.FirstDisplayedScrollingRowIndex = nRowIndex;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete this record?", "Deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                           == DialogResult.Yes)
            {
                int rowIndex = dataGridViewDepartments.CurrentCell.RowIndex;

                dataGridViewDepartments.Rows.RemoveAt(rowIndex);
                dataSet.Tables["departments"].Rows[rowIndex].Delete();

                mySqlDataAdapter.Update(dataSet, "departments");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int x = dataGridViewDepartments.CurrentCell.RowIndex;
            if (textBox1.Text == "")
            {
                MessageBox.Show("Error! Fill all the text boxes please!");
            }
            else 
            {
                dataGridViewDepartments.Rows[x].Cells["dept_name"].Value = textBox1.Text;

                mySqlDataAdapter.Update(dataSet, "departments");
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void dataGridViewDepartments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int x = e.RowIndex;
            if(e.RowIndex != -1)
                textBox1.Text = dataGridViewDepartments.Rows[x].Cells["dept_name"].Value.ToString();
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            (dataGridViewDepartments.DataSource as DataTable).DefaultView.RowFilter = $"dept_name LIKE '%{searchBox.Text}%' OR CONVERT(dept_no, System.String) LIKE '%{searchBox.Text}%'";
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
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

        private void dataGridViewDepartments_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Tab)
            {
                textBox1.Text = dataGridViewDepartments.CurrentRow.Cells["dept_name"].Value.ToString();
            }
        }
    }
}
