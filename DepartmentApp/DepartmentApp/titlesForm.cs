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
    public partial class titlesForm : Form
    {
        private MySqlCommandBuilder mySqlCommandBuilder = null;
        private MySqlDataAdapter mySqlDataAdapter = null;
        private DataSet dataSet = null;
        public titlesForm()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM titles";
                MySqlConnection con = new MySqlConnection(DataBase.connectionSql());

                mySqlDataAdapter = new MySqlDataAdapter(query, con);
                mySqlCommandBuilder = new MySqlCommandBuilder(mySqlDataAdapter);

                dataSet = new DataSet();

                mySqlDataAdapter.Fill(dataSet, "titles");

                dataGridViewTitles.DataSource = dataSet.Tables["titles"];

                dataGridViewTitles.Columns[0].HeaderText = "Employee ID";
                dataGridViewTitles.Columns[1].HeaderText = "Title";
                dataGridViewTitles.Columns[2].HeaderText = "Other details";
                dataGridViewTitles.Columns[3].HeaderText = "From date";
                dataGridViewTitles.Columns[4].HeaderText = "To date";

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
                dataSet.Tables["titles"].Clear();

                mySqlDataAdapter.Fill(dataSet, "titles");

                dataGridViewTitles.DataSource = dataSet.Tables["titles"];
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void titlesForm_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadCombo();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            DataRow row = dataSet.Tables["titles"].NewRow();

            if (textBox1.Text == "" || textBox2.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("Error! Fill all the text boxes please!");
            }
            else
            {
                row["emp_no"] = comboBox1.Text;
                row["title"] = textBox1.Text;
                row["other"] = textBox2.Text;
                row["from_date"] = dateTimePicker1.Text;
                row["to_date"] = dateTimePicker2.Text;

                dataSet.Tables["titles"].Rows.Add(row);

                mySqlDataAdapter.Update(dataSet, "titles");
                ReloadData();
                int nRowIndex = dataGridViewTitles.Rows.Count - 1;

                dataGridViewTitles.FirstDisplayedScrollingRowIndex = nRowIndex;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete this record?", "Deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                int rowIndex = dataGridViewTitles.CurrentCell.RowIndex;

                dataGridViewTitles.Rows.RemoveAt(rowIndex);
                dataSet.Tables["titles"].Rows[rowIndex].Delete();

                mySqlDataAdapter.Update(dataSet, "titles");
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            int x = dataGridViewTitles.CurrentCell.RowIndex;
            if (textBox1.Text == "" || textBox2.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("Error! Fill all the text boxes please!");
            }
            else
            {
                dataGridViewTitles.Rows[x].Cells["emp_no"].Value = comboBox1.Text;
                dataGridViewTitles.Rows[x].Cells["title"].Value = textBox1.Text;
                dataGridViewTitles.Rows[x].Cells["other"].Value = textBox2.Text;
                dataGridViewTitles.Rows[x].Cells["from_date"].Value = dateTimePicker1.Text;
                dataGridViewTitles.Rows[x].Cells["to_date"].Value = dateTimePicker2.Text;

                mySqlDataAdapter.Update(dataSet, "titles");
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            dateTimePicker1.Text = DateTime.Now.ToString().Substring(0, 10);
            dateTimePicker2.Text = DateTime.Now.ToString().Substring(0, 10);
        }

        private void dataGridViewTitles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int x = e.RowIndex;
            if (e.RowIndex != -1)
            {
                comboBox1.Text = dataGridViewTitles.Rows[x].Cells["emp_no"].Value.ToString();
                textBox1.Text = dataGridViewTitles.Rows[x].Cells["title"].Value.ToString();
                textBox2.Text = dataGridViewTitles.Rows[x].Cells["other"].Value.ToString();
                if (dataGridViewTitles.Rows[x].Cells["from_date"].Value.ToString().Substring(0, 10) != "")
                    dateTimePicker1.Text = dataGridViewTitles.Rows[x].Cells["from_date"].Value.ToString().Substring(0, 10);
                if (dataGridViewTitles.Rows[x].Cells["to_date"].Value.ToString().Substring(0, 10) != "")
                    dateTimePicker2.Text = dataGridViewTitles.Rows[x].Cells["to_date"].Value.ToString().Substring(0, 10);
            }
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            (dataGridViewTitles.DataSource as DataTable).DefaultView.RowFilter = $"title LIKE '%{searchBox.Text}%' OR CONVERT(emp_no, System.String) LIKE '%{searchBox.Text}%'";
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

        private void SalariesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var departments = new salariesForm();
            departments.Closed += (s, args) => this.Close();
            departments.Show();
        }
    }
}
