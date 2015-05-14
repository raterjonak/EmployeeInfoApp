using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace EmployeeInfoApp
{
    public partial class EmployeeInfoUI : Form
    {
        private String connectionString =ConfigurationManager.ConnectionStrings["EmployeeInfoConnString"].ConnectionString;
        Employee employeeObj=new Employee();
        public EmployeeInfoUI()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            employeeObj.name = nameTextBox.Text;
            employeeObj.address = addressTextBox.Text;
            employeeObj.email = emailTExtBox.Text;
            employeeObj.salary = Convert.ToDouble(salaryTextBox.Text);


            if (IsEmailExists(employeeObj.email))
            {
                MessageBox.Show("Email name  already exists!");
                return;
            }


            SqlConnection connection = new SqlConnection(connectionString);

            String query = "INSERT INTO Employee Values( '" + employeeObj.name + "','" + employeeObj.address + "','" +
                           employeeObj.email + "','" + employeeObj.salary + "');";

            SqlCommand command=new SqlCommand(query,connection);

            connection.Open();

           int rowAffected= command.ExecuteNonQuery();
            connection.Close();

            if (rowAffected>0)
            {
                MessageBox.Show("Insertion successful.");
                ShowAllEmplyeeInfo();
            }

            else
            {
                MessageBox.Show("Insertion Fail!!!");
            }
        }

        public bool IsEmailExists(string email)
        {
            
            SqlConnection connection = new SqlConnection(connectionString);

            //2. write query 

            string query = "SELECT * FROM Employee WHERE email ='" + email + "'";


            // 3. execute query 

            SqlCommand command = new SqlCommand(query, connection);

            bool isEmailExists = false;

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                isEmailExists = true;
                break;
            }
            reader.Close();
            connection.Close();

            return isEmailExists;

        }


        public void ShowAllEmplyeeInfo()
        {
            SqlConnection connection=new SqlConnection(connectionString);
            string query = "SELECT * FROM Employee";
            SqlCommand command=new SqlCommand(query,connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<Employee>employeeList=new List<Employee>();
            while (reader.Read())
            {
                Employee employee=new Employee();

                employee.id = int.Parse(reader["id"].ToString());
                employee.name = reader["name"].ToString();
                employee.address = reader["address"].ToString();
                employee.email = reader["email"].ToString();
                employee.salary = reader["salary"].GetHashCode();

                employeeList.Add(employee);
            }

            LoadAllEmployeeListView(employeeList);
        }

        public void LoadAllEmployeeListView(List<Employee> employees)
        {
            foreach (var employee in employees)
            {
                ListViewItem item =new ListViewItem(employee.id.ToString());
                item.SubItems.Add(employee.name);
                item.SubItems.Add(employee.address);
                item.SubItems.Add(employee.email);
                item.SubItems.Add(employee.salary.ToString());

                EmployeeListView.Items.Add(item);
            }
        }

        private void EmployeeInfoUI_Load(object sender, EventArgs e)
        {
            ShowAllEmplyeeInfo();
        }

        
    }
}
