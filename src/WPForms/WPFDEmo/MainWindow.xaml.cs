using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFDEmo.Data;

namespace WPFDEmo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Person> People  = new List<Person>()
        {
            new Person{Name="Jannick", Age = 30},
            new Person{Name="Marc", Age = 20},
            new Person{Name="Maria", Age = 40},
            new Person{Name="Scott", Age = 35},
        };
        public MainWindow()
        {
            InitializeComponent();

            ListBoxPeople.ItemsSource = People;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var SelectedItems = ListBoxPeople.SelectedItems;

            foreach (var SelectedItem in SelectedItems)
            {
                var person = (Person)SelectedItem;
                MessageBox.Show(person.Name);
            }
        }
    }
}