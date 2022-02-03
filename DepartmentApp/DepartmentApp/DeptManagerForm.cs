﻿using System;
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
    public partial class DeptManagerForm : Form
    {
        private MySqlCommandBuilder mySqlCommandBuilder = null;
        private MySqlDataAdapter mySqlDataAdapter = null;
        private DataSet dataSet = null;
        public DeptManagerForm()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM dept_manager";
                MySqlConnection con = new MySqlConnection(DataBase.connectionSql());

                mySqlDataAdapter = new MySqlDataAdapter(query, con);
                mySqlCommandBuilder = new MySqlCommandBuilder(mySqlDataAdapter);

                dataSet = new DataSet();

                mySqlDataAdapter.Fill(dataSet, "dept_manager");

                dataGridViewDeptManager.DataSource = dataSet.Tables["dept_manager"];

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
                dataSet.Tables["dept_manager"].Clear();

                mySqlDataAdapter.Fill(dataSet, "dept_manager");

                dataGridViewDeptManager.DataSource = dataSet.Tables["dept_manager"];
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void DeptManagerForm_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadCombo();
            LoadCombo_2();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            DataRow row = dataSet.Tables["dept_manager"].NewRow();

            if (comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("Error! Fill all the text boxes please!");
            }
            else
            {
                row["emp_no"] = comboBox1.Text;
                row["dept_no"] = comboBox2.Text;
                row["from_date"] = dateTimePicker1.Text;
                row["to_date"] = dateTimePicker2.Text;

                dataSet.Tables["dept_manager"].Rows.Add(row);

                mySqlDataAdapter.Update(dataSet, "dept_manager");
                ReloadData();
                int nRowIndex = dataGridViewDeptManager.Rows.Count - 1;
                int nColumnIndex = 1;

                dataGridViewDeptManager.Rows[nRowIndex].Selected = true;
                dataGridViewDeptManager.Rows[nRowIndex].Cells[nColumnIndex].Selected = true;

                dataGridViewDeptManager.FirstDisplayedScrollingRowIndex = nRowIndex;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete this record?", "Deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
    == DialogResult.Yes)
            {
                int rowIndex = dataGridViewDeptManager.CurrentCell.RowIndex;

                dataGridViewDeptManager.Rows.RemoveAt(rowIndex);
                dataSet.Tables["dept_manager"].Rows[rowIndex].Delete();

                mySqlDataAdapter.Update(dataSet, "dept_manager");
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            int x = dataGridViewDeptManager.CurrentCell.RowIndex;
            if (comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("Error! Fill all the text boxes please!");
            }
            else
            {
                dataGridViewDeptManager.Rows[x].Cells["emp_no"].Value = comboBox1.Text;
                dataGridViewDeptManager.Rows[x].Cells["dept_no"].Value = comboBox2.Text;
                dataGridViewDeptManager.Rows[x].Cells["from_date"].Value = dateTimePicker1.Text;
                dataGridViewDeptManager.Rows[x].Cells["to_date"].Value = dateTimePicker2.Text;

                mySqlDataAdapter.Update(dataSet, "dept_manager");
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            comboBox2.Text = "";
            dateTimePicker1.Text = DateTime.Now.ToString().Substring(0, 10);
            dateTimePicker2.Text = DateTime.Now.ToString().Substring(0, 10);
        }

        private void dataGridViewDeptManager_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int x = e.RowIndex;
            if (e.RowIndex != -1)
            {
                comboBox1.Text = dataGridViewDeptManager.Rows[x].Cells["emp_no"].Value.ToString();
                comboBox2.Text = dataGridViewDeptManager.Rows[x].Cells["dept_no"].Value.ToString();
                if (dataGridViewDeptManager.Rows[x].Cells["from_date"].Value.ToString().Substring(0, 10) != "")
                    dateTimePicker1.Text = dataGridViewDeptManager.Rows[x].Cells["from_date"].Value.ToString().Substring(0, 10);
                if (dataGridViewDeptManager.Rows[x].Cells["to_date"].Value.ToString().Substring(0, 10) != "")
                    dateTimePicker2.Text = dataGridViewDeptManager.Rows[x].Cells["to_date"].Value.ToString().Substring(0, 10);
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

        private void departmentEmployeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var departments = new DeptEmpForm();
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
