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
        private bool newRowAdd = false;
        public departmentsForm()
        {
            InitializeComponent();
        }
        
        private void LoadData()
        {
            try
            {
                string query = "SELECT *, 'Delete' AS 'Delete' FROM departments";
                MySqlConnection con = new MySqlConnection(DataBase.connectionSql());

                mySqlDataAdapter = new MySqlDataAdapter(query, con);
                mySqlCommandBuilder = new MySqlCommandBuilder(mySqlDataAdapter);

                mySqlCommandBuilder.GetInsertCommand();
                mySqlCommandBuilder.GetUpdateCommand();
                mySqlCommandBuilder.GetDeleteCommand();

                dataSet = new DataSet();

                mySqlDataAdapter.Fill(dataSet, "departments");

                dataGridViewDepartments.DataSource = dataSet.Tables["departments"];

                for (int i = 0; i < dataGridViewDepartments.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridViewDepartments[2, i] = linkCell;
                }
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

                for (int i = 0; i < dataGridViewDepartments.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridViewDepartments[2, i] = linkCell;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var Main = new Main_Form();
            Main.Closed += (s, args) => this.Close();
            Main.Show();
        }

        private void departmentsForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void dataGridViewDepartments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2)
                {
                    string task = dataGridViewDepartments.Rows[e.RowIndex].Cells[2].Value.ToString();


                    if(task == "Delete")
                    {
                        if(MessageBox.Show("Delete this record?", "Deleting", MessageBoxButtons.YesNo,MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;

                            dataGridViewDepartments.Rows.RemoveAt(rowIndex);
                            dataSet.Tables["departments"].Rows[rowIndex].Delete();

                            mySqlDataAdapter.Update(dataSet, "departments");
                        }
                    }
                    else if(task == "Insert")
                    {
                        int rowIndex = dataGridViewDepartments.Rows.Count - 2;

                        DataRow row = dataSet.Tables["departments"].NewRow();

                        row["dept_no"] = dataGridViewDepartments.Rows[rowIndex].Cells["dept_no"].Value;
                        row["dept_name"] = dataGridViewDepartments.Rows[rowIndex].Cells["dept_name"].Value;

                        dataSet.Tables["departments"].Rows.Add(row);
                        dataSet.Tables["departments"].Rows.RemoveAt(dataSet.Tables["departments"].Rows.Count - 1);

                        dataGridViewDepartments.Rows.RemoveAt(dataGridViewDepartments.Rows.Count - 2);
                        dataGridViewDepartments.Rows[e.RowIndex].Cells[2].Value = "Delete";

                        mySqlDataAdapter.Update(dataSet, "departments");

                        newRowAdd = false;
                    }
                    else if(task == "Update")
                    {
                        int x = e.RowIndex;

                        dataSet.Tables["departments"].Rows[x]["dept_no"] = dataGridViewDepartments.Rows[x].Cells["dept_no"].Value;
                        dataSet.Tables["departments"].Rows[x]["dept_name"] = dataGridViewDepartments.Rows[x].Cells["dept_name"].Value;

                        mySqlDataAdapter.Update(dataSet, "departments");
                        dataGridViewDepartments.Rows[e.RowIndex].Cells[2].Value = "Delete";
                    }

                    ReloadData();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewDepartments_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if(newRowAdd == false)
                {
                    newRowAdd = true;

                    int lastRow = dataGridViewDepartments.Rows.Count - 2;

                    DataGridViewRow row = dataGridViewDepartments.Rows[lastRow];
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridViewDepartments[2, lastRow] = linkCell;

                    row.Cells["Delete"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewDepartments_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if(newRowAdd == false)
                {
                    int rowIndex = dataGridViewDepartments.SelectedCells[0].RowIndex;
                    DataGridViewRow editingRow = dataGridViewDepartments.Rows[rowIndex];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridViewDepartments[2, rowIndex] = linkCell;

                    editingRow.Cells["Delete"].Value = "Update";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
