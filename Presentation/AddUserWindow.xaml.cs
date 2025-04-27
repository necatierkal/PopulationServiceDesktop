using Models;
using System.Windows;
namespace Presentation
{
    /// <summary>
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        public User NewUser { get; private set; }

        public AddUserWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(UsernameTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }

            NewUser = new User
            {
                Name = NameTextBox.Text,
                Username = UsernameTextBox.Text,
                Email = EmailTextBox.Text
            };

            DialogResult = true;
            Close();
        }

        public AddUserWindow(User existingUser) : this()
        {
            NameTextBox.Text = existingUser.Name;
            UsernameTextBox.Text = existingUser.Username;
            EmailTextBox.Text = existingUser.Email;
        }



    }
}
