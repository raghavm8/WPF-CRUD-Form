using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_CRUD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        SqlConnection con = new SqlConnection("Data Source=SONY\\RAGHAV;Initial Catalog=WPF_CRUD;User ID=sa;Password=raghav");

        public MainWindow()
        {
            InitializeComponent();
            Load_Grid();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("update Details set Name = '" + name.Text + "',Age = '" + age.Text + "',Gender = '" + gender.Text + "',City = '" + city.Text + "' where id= " + id.Text + "", con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Updated", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
                ClearTextBoxes();
                Load_Grid();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from Details where Id=" + id.Text + " ", con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Deleted", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
                ClearTextBoxes();
                Load_Grid();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validData() == true)
                {
                    SqlCommand cmd = new SqlCommand("insert into details values(@Name, @Age, @Gender, @City)", con);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@Name", name.Text);
                    cmd.Parameters.AddWithValue("@Age", age.Text);
                    cmd.Parameters.AddWithValue("@Gender", gender.Text);
                    cmd.Parameters.AddWithValue("@City", city.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    Load_Grid();

                    MessageBox.Show("Successfully Added", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearTextBoxes();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBoxes();
        }

        private void ClearTextBoxes()
        {
            name.Clear();
            age.Clear();
            gender.Clear();
            city.Clear();
            id.Clear();
        }

        public void Load_Grid()
        {
            SqlCommand cmd = new SqlCommand("select * from Details", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            con.Close();
            data_grid.ItemsSource = dt.DefaultView;
        }
        public bool validData()
        {
            if (name.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (age.Text == string.Empty)
            {
                MessageBox.Show("Age is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (gender.Text == string.Empty)
            {
                MessageBox.Show("Gender is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (city.Text == string.Empty)
            {
                MessageBox.Show("City is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}
