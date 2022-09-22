using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace BlazorMarkDownAppJwt.Client.Components
{
    public partial class NavMenu : ComponentBase
    {
        [Inject]
        private ILocalStorageService LocalStorage { get; set; } = default!;

        private bool CollapseNavMenu { get; set; } = true;

        private string? NavMenuCssClass => CollapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            CollapseNavMenu = !CollapseNavMenu;
        }

        string? userMail;

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
                userMail = await GetUserMail();
            }
            catch (Exception)
            {
                await LocalStorage.RemoveItemAsync("user");
                userMail = null;
            }
        }
    }
}
