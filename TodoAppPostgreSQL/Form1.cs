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
    public partial class Form1 : Form
    {
        private string _connectionString;
        public Form1()
        {
            InitializeComponent();
            _connectionString = ConfigurationManager.ConnectionStrings["postgresqlConnection"].ConnectionString;

        }

        void TodoList()
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string query = "Select t.id,t.content,c.name as kategori from todos t inner join categories c on t.categoryId = c.id order by Id";
            var command = new NpgsqlCommand(query, connection);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            connection.Close();
        }

        void CategoryList()
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string query = "select * from Categories order by Id";
            var command = new NpgsqlCommand(query, connection);
            var adapter = new NpgsqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "Id";
            cmbCategory.DataSource = dataTable;
            connection.Close();
        }

        private void btnCategories_Click(object sender, EventArgs e)
        {
            frmCategory frmCategory = new frmCategory();
            frmCategory.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TodoList();
            CategoryList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int categoryId = int.Parse(cmbCategory.SelectedValue.ToString());
            

            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string query = "insert into todos (Content, CategoryId) values (@content,@categoryId)";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@content", txtTodo.Text);
                command.Parameters.AddWithValue("@categoryId",categoryId);
                command.ExecuteNonQuery();
                TodoList();
            }
            connection.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtId.Text);
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string query = "delete from todos where Id=@id";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                TodoList();
            }
            connection.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtId.Text);
            int categoryId = int.Parse(cmbCategory.SelectedValue.ToString());
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string query = "Update Todos set Content=@content,CategoryId=@categoryId where Id=@id";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@categoryId", categoryId);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@content", txtTodo.Text);
                command.ExecuteNonQuery();
                TodoList();
            }
            connection.Close();
        }
    }
}
