using Microsoft.AspNetCore.Components;

namespace BlazorMarkDownAppJwt.Client.Components
{
    public partial class UserComponent : ComponentBase
    {
        [Parameter]
        public string? UserMail { get; set; }
    }
}
