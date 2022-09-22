using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace BlazorMarkDownAppJwt.Client.Pages
{
    public partial class MarkDownDetail : ComponentBase
    {
        [Parameter]
        public long? CurrentId { get; set; } = null;

        [Inject]
        private ILocalStorageService LocalStorage { get; set; } = default!;

        private string? UserMail { get; set; }

        private async Task<string?> GetUserMail()
        {
            var jwt = await LocalStorage.GetItemAsync<string>("user");
            if (!string.IsNullOrWhiteSpace(jwt))
            {
                var dataArray = jwt.Split(';', 2);
                if (dataArray.Length == 2)
                    return dataArray[0];
            }
            return null;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            try
            {
                UserMail = await GetUserMail();
            }
            catch (Exception)
            {
                await LocalStorage.RemoveItemAsync("user");
                UserMail = null;
            }
        }
    }
}
