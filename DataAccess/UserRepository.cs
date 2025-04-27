using Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace DataAccess
{
    public class UserRepository
    {
        private readonly HttpClient _httpClient;

        public UserRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<User>>(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}");
            }

            return new List<User>();
        }

        public async Task<bool> AddUserAsync(User user)
        {
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://jsonplaceholder.typicode.com/users", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var response = await _httpClient.DeleteAsync($"https://jsonplaceholder.typicode.com/users/{userId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"https://jsonplaceholder.typicode.com/users/{user.Id}", content);

            return response.IsSuccessStatusCode;
        }




    }

}
