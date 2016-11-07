using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utilities.MsSqlServer;

namespace SqlServerStoredProceduresGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextConnectionString.Text))
            {
                List<string> dbsNames = new List<string>();
                try
                {
                    SqlConnection connection = new SqlConnection(TextConnectionString.Text);
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT name FROM master.dbo.sysdatabases where name not in('master','tempdb', 'model', 'msdb', 'ReportServer','ReportServerTempDB')", connection);
                    var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    while (reader.Read())
                    {
                        dbsNames.Add(reader[0].ToString());
                    }
                    connection.Close();
                    ComboDBs.ItemsSource = dbsNames;
                    ComboDBs.IsEnabled = true;
                    //   ButtonGenerate.IsEnabled = true;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            var gen = new XmlTemplateReader();
            StringBuilder allSps = new StringBuilder();
            ConnectionStringRefresher.RefreshConnectionStringInConfigurationFile(TextConnectionString.Text);
            allSps.Append(gen.ReadInsertStoredProcedure((string)ComboTables.SelectedValue, (string)ComboDBs.SelectedValue));
            allSps.Append(gen.ReadUpdateStoredProcedure((string)ComboTables.SelectedValue, (string)ComboDBs.SelectedValue));
            allSps.Append(gen.ReadSelectStoredProcedure((string)ComboTables.SelectedValue, (string)ComboDBs.SelectedValue));
            allSps.Append(gen.ReadSelectAllStoredProcedure((string)ComboTables.SelectedValue, (string)ComboDBs.SelectedValue));
            allSps.Append(gen.ReadDeleteStoredProcedure((string)ComboTables.SelectedValue, (string)ComboDBs.SelectedValue));
            RichTextBox.Text = allSps.ToString();
        }
        private void ComboDBs_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null)
            {
                string dbName = (string) e.AddedItems[0];
                List<string> tablesNames = new List<string>();

                try
                {
                    SqlConnection connection = new SqlConnection(TextConnectionString.Text);
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT TABLE_NAME FROM " + dbName + ".INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG='" + dbName + "'", connection);
                    var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    while (reader.Read())
                    {
                        tablesNames.Add(reader[0].ToString());
                    }
                    connection.Close();
                    ComboDBs.IsEnabled = true;
                    ComboTables.ItemsSource = tablesNames;
                    ComboTables.IsEnabled = true;
                    TextConnectionString.Text += TextConnectionString.Text.IndexOf("initial catalog=", StringComparison.Ordinal) > 0 ? "" : " initial catalog=" + (string)ComboDBs.SelectedValue;

                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void ComboTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonGenerate.IsEnabled = true;
        }

    }
}
