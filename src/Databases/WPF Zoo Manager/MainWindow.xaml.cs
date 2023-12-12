using System;
using System.Collections.Generic;
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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WPF_Zoo_Manager
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;

        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["WPF_Zoo_Manager.Properties.Settings.FranciscoDBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            ShowZoos();
            ShowAnimals();
        }

        private void ShowZoos()
        {
            try
            {
                string query = "SELECT * FROM Zoo";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable zooTable = new DataTable();

                    sqlDataAdapter.Fill(zooTable);
                    listZoos.DisplayMemberPath = "Location";
                    listZoos.SelectedValuePath = "Id";
                    listZoos.ItemsSource = zooTable.DefaultView;
                }
            } catch (Exception ex)
            {
            }
        }
        private void ShowAssociatedAnimals()
        {
            try
            {
                string query = "SELECT * FROM Animal a INNER JOIN ZooAnimal " +
                    "za on a.id = za.AnimalId where za.ZooId = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

                    DataTable animalTable = new DataTable();

                    sqlDataAdapter.Fill(animalTable);
                    listAssociatedAnimals.DisplayMemberPath = "Name";
                    listAssociatedAnimals.SelectedValuePath = "Id";
                    listAssociatedAnimals.ItemsSource = animalTable.DefaultView;
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ShowAnimals()
        {
            try
            {
                string query = "SELECT * FROM Animal";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                using (sqlDataAdapter)
                {
                    DataTable AnimalsTable = new DataTable();

                    sqlDataAdapter.Fill(AnimalsTable);

                    listAnimals.DisplayMemberPath = "Name";
                    listAnimals.SelectedValuePath = "Id";
                    listAnimals.ItemsSource = AnimalsTable.DefaultView;
                }
            } catch (Exception e)
            {
            }
        }

        private void listZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAssociatedAnimals();
            ShowSelectedZooInTextBox(); 
        }

        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            string query = "DELETE FROM Zoo WHERE Zoo.Id = @ZooId";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
            sqlCommand.ExecuteScalar();
            sqlConnection.Close(); 

            ShowZoos();
        }
        private void DeleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            string query = "DELETE FROM Animal WHERE Animal.Id = @AnimalId";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            sqlCommand.Parameters.AddWithValue("@AnimalId",listAnimals.SelectedValue);
            sqlCommand.ExecuteScalar();
            sqlConnection.Close(); 

            ShowAnimals();
        }        
        
        private void DeleteAnimalFromZoo_Click(object sender, RoutedEventArgs e)
        {
            string query = "DELETE FROM ZooAnimal WHERE ZooAnimal.ZooId = @ZooId AND ZooAnimal.AnimalId = @AnimalId";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            sqlCommand.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);
            sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
            sqlCommand.ExecuteScalar();
            sqlConnection.Close(); 

            ShowAssociatedAnimals();
        }       
        
        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO Zoo (Location) VALUES (@Location)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Location", InputBox.Text);
                sqlCommand.ExecuteScalar();
            } catch (Exception ex) { MessageBox.Show(ex.ToString()); }

            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }
        }
        
        private void AddAnimalToZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO ZooAnimal VALUES (@AnimalId, @ZooId)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

            finally
            {
                sqlConnection.Close();
                ShowAssociatedAnimals();
            }
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO Animal VALUES (@Name)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", InputBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

            finally
            {
                sqlConnection.Close();
                ShowAnimals();
            }
        }

        private void ShowSelectedZooInTextBox()
        {
            try
            {
                string query = "SELECT Location from Zoo WHERE Id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    InputBox.Text = dataTable.Rows[0]["Location"].ToString();

                }
            }
            catch (Exception e)
            {
            }

        }        
        
        private void ShowSelectedAnimalInTextBox()
        {
            try
            {
                string query = "SELECT Name from Animal WHERE Id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);

                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    InputBox.Text = dataTable.Rows[0]["Name"].ToString();   
                    
                }
            } catch (Exception e)
            {
            }

        }

        private void UpdateZoo_Click(object  sender, EventArgs e)
        {
            try
            {
                string query = "UPDATE Zoo SET Location = @Location WHERE Zoo.Id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Location", InputBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }
        }

        private void UpdateAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "UPDATE Animal SET Name = @Name WHERE Animal.Id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", InputBox.Text);
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

            finally
            {
                sqlConnection.Close();
                ShowAnimals();
            }
        }

        private void listAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedAnimalInTextBox();
        }
    }
}
