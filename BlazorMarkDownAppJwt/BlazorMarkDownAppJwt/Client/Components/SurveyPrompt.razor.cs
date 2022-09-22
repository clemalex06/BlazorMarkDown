using Microsoft.AspNetCore.Components;

namespace BlazorMarkDownAppJwt.Client.Components
{
    public partial class SurveyPrompt : ComponentBase
    {
        [Parameter]
        public string? Title { get; set; }
    }
}
