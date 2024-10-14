using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TodoAppPostgreSQL
{
    public partial class frmCategory : Form
    {
        private string _connectionString;
        public frmCategory()
        {
            InitializeComponent();
            _connectionString = ConfigurationManager.ConnectionStrings["postgresqlConnection"].ConnectionString;
        }

        void CategoryList()
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string query = "Select * from Categories order by Id";
            var command = new NpgsqlCommand(query, connection);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            connection.Close();
        }
        private void frmCategory_Load(object sender, EventArgs e)
        {
            CategoryList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string query = "insert into Categories (Name) values (@name)";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", txtCategory.Text);
                command.ExecuteNonQuery();
                CategoryList();
            }
            connection.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtId.Text);

            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string query = "Delete from Categories Where Id=@id";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                CategoryList();
            }
            connection.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtId.Text);

            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string query = "Update Categories set Name = @name where Id=@id";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", txtCategory.Text);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                CategoryList();
            }
            connection.Close();
        }
    }
}
