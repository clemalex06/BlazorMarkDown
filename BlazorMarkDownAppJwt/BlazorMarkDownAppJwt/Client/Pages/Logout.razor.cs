using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace BlazorMarkDownAppJwt.Client.Pages
{
    public partial class Logout : ComponentBase
    {
        [Inject]
        private ILocalStorageService LocalStorage { get; set; } = default!;

        [Inject]
        NavigationManager NavigationManager { get; set; } = default!;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LocalStorage.RemoveItemAsync("user");
            NavigationManager.NavigateTo("/", true);
        }
    }
}
