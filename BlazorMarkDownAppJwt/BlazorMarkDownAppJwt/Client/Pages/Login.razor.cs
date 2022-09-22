using Blazored.LocalStorage;
using BlazorMarkDownAppJwt.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorMarkDownAppJwt.Client.Pages
{
    public partial class Login : ComponentBase
    {
        [Inject]
        private HttpClient HttpClient { get; set; } = default!;

        [Inject]
        private ILocalStorageService LocalStorage { get; set; } = default!;

        [Inject]
        NavigationManager NavigationManager { get; set; } = default!;

        private LoginModel User { get; set; } = new LoginModel();
        private string? Message { get; set; } = string.Empty;
        private bool IsDisabled { get; set; } = false;

        private async Task OnValid()
        {
            try
            {
                IsDisabled = true;
                using (var msg = await HttpClient.PostAsJsonAsync("/api/auth/login", User))
                {
                    if (msg.IsSuccessStatusCode)
                    {
                        LoginResult? result = await msg.Content.ReadFromJsonAsync<LoginResult>();
                        Message = result?.Message;

                        if (result != null && result.Success)
                        {
                            await LocalStorage.SetItemAsStringAsync("user", $"{result.Email};{result.JwtBearer}");
                            NavigationManager.NavigateTo("/", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            finally
            {
                IsDisabled = false;
            }

        }
    }
}
