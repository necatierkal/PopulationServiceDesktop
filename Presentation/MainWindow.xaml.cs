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
using Business;
using Models;
using System.Collections.Generic;
using System.Windows;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UserService _userService;
        private bool isDarkMode = false;

        public MainWindow()
        {
            InitializeComponent();
            _userService = new UserService();
            LoadUsers();
        }

        private async void LoadUsers()
        {
            List<User> users = await _userService.GetAllUsersAsync();

            if (users != null && users.Count > 0)
            {
                UserListBox.ItemsSource = users;
            }
            else
            {
                MessageBox.Show("Hiç kullanıcı verisi alınamadı!");
            }
        }
        private async void UserListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserListBox.SelectedItem is User selectedUser)
            {
                IdTextBlock.Text = selectedUser.Id.ToString();
                NameTextBlock.Text = selectedUser.Name;
                UsernameTextBlock.Text = selectedUser.Username;
                EmailTextBlock.Text = selectedUser.Email;
            }
        }
        private async void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUserWindow = new AddUserWindow();
            bool? result = addUserWindow.ShowDialog();

            if (result == true)
            {
                User newUser = addUserWindow.NewUser;

                bool success = await _userService.AddUserAsync(newUser);

                if (success)
                {
                    MessageBox.Show("Yeni kullanıcı eklendi!");
                    await RefreshUserList();
                }
                else
                {
                    MessageBox.Show("Kullanıcı eklenemedi.");
                }
            }
        }

        private async void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserListBox.SelectedItem is User selectedUser)
            {
                var result = MessageBox.Show($"Kullanıcıyı silmek istiyor musun? ({selectedUser.Name})", "Onay", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    bool success = await _userService.DeleteUserAsync(selectedUser.Id);

                    if (success)
                    {
                        MessageBox.Show("Kullanıcı silindi (fake). Listeyi yenileyelim!");
                        await RefreshUserList();
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı silinemedi.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Önce bir kullanıcı seç!");
            }
        }

        private async Task RefreshUserList()
        {
            var users = await _userService.GetAllUsersAsync();
            UserListBox.ItemsSource = users;
        }

        private async void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserListBox.SelectedItem is User selectedUser)
            {
                AddUserWindow editWindow = new AddUserWindow(selectedUser);
                bool? result = editWindow.ShowDialog();

                if (result == true)
                {
                    User updatedUser = editWindow.NewUser;
                    updatedUser.Id = selectedUser.Id; // ID'yi kaybetmeyelim!

                    bool success = await _userService.UpdateUserAsync(updatedUser);

                    if (success)
                    {
                        MessageBox.Show("Kullanıcı güncellendi!");
                        await RefreshUserList();
                    }
                    else
                    {
                        MessageBox.Show("Güncelleme başarısız oldu.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Düzenlemek için bir kullanıcı seçmelisin!");
            }
        }

        private async void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchTextBox.Text.ToLower();

            var allUsers = await _userService.GetAllUsersAsync();
            var filteredUsers = allUsers.Where(u => u.Name.ToLower().Contains(query) || u.Username.ToLower().Contains(query)).ToList();

            UserListBox.ItemsSource = filteredUsers;
        }

        private async void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var users = await _userService.GetAllUsersAsync();

            if (SortComboBox.SelectedIndex == 0) // A-Z
            {
                users = users.OrderBy(u => u.Name).ToList();
            }
            else if (SortComboBox.SelectedIndex == 1) // Z-A
            {
                users = users.OrderByDescending(u => u.Name).ToList();
            }

            UserListBox.ItemsSource = users;
        }



        private void ToggleDarkMode_Click(object sender, RoutedEventArgs e)
        {
            if (isDarkMode)
            {
                Resources["BackgroundBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAFAFA"));
                Resources["CardBrush"] = new SolidColorBrush(Colors.White);
            }
            else
            {
                Resources["BackgroundBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#121212"));
                Resources["CardBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1E1E"));
            }

            isDarkMode = !isDarkMode;
        }


    }
}
