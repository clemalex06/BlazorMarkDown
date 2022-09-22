using BlazorMarkDownAppJwt.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorMarkDownAppJwt.Client.Pages
{
    public partial class Register : ComponentBase
    {
        [Inject]
        private HttpClient HttpClient { get; set; } = default!;

        private RegModel Reg { get; set; } = new RegModel();

        private string? Message { get; set; } = string.Empty;

        private string? Login { get; set; } = string.Empty;

        private bool IsDisabled { get; set; } = false;

        private async Task OnValid()
        {
            try
            {
                IsDisabled = true;
                using (var msg = await HttpClient.PostAsJsonAsync("/api/auth/register", Reg))
                {
                    if (msg.IsSuccessStatusCode)
                    {
                        LoginResult? result = await msg.Content.ReadFromJsonAsync<LoginResult>();
                        Message = result?.Message;
                        if (result != null && result.Success)
                        {
                            Message += " Please LOGIN to continue.";
                            Login = "Click here to LOGIN.";
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
