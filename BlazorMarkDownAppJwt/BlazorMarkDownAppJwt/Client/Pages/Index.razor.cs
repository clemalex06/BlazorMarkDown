using BlazorMarkDownAppJwt.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorMarkDownAppJwt.Client.Pages
{
    public partial class Index : ComponentBase
    {


        [Inject]
        private HttpClient HttpClient { get; set; } = default!;

        private bool isLoading { get; set; } = false;

        private List<MarkDownModel> markDowns { get; set; } = new List<MarkDownModel>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            isLoading = true;
            try
            {
                var urlRoute = "/api/markdowns/";
                var requestMsg = new HttpRequestMessage(HttpMethod.Get, urlRoute);
                var response = await HttpClient.SendAsync(requestMsg);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<MarkDownModel>>();
                    if (content != null)
                    {
                        markDowns = content;
                    }
                }
            }
            catch (Exception)
            {
                markDowns = new List<MarkDownModel>();
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}
