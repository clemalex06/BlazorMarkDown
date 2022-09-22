using Blazored.LocalStorage;
using BlazorMarkDownAppJwt.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BlazorMarkDownAppJwt.Client.Components
{
    public partial class MarkDownComponent : ComponentBase
    {
        [Parameter]
        public string? UserMail { get; set; }

        [Parameter]
        public long? MarkDownId { get; set; }

        [Inject]
        private HttpClient HttpClient { get; set; } = default!;

        [Inject]
        private ILocalStorageService LocalStorage { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        private MarkupString markupString;

        private string successMessage = string.Empty;
        private string errorMessage = string.Empty;

        private bool isLoading = true;
        private bool checkPreview = true;
        private bool displayEditor = true;

        private MarkDownModel markDownModel = new()
        {
            Id = 0,
            Body = string.Empty
        };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            isLoading = true;
            successMessage = string.Empty;
            errorMessage = string.Empty;
            try
            {
                var urlRoute = MarkDownId == null ? "api/markdown/readme/" : $"/api/markdown?currentId={MarkDownId}";
                var requestMsg = new HttpRequestMessage(HttpMethod.Get, urlRoute);
                var response = await HttpClient.SendAsync(requestMsg);
                if (response.IsSuccessStatusCode)
                {
                    await SetSuccess(response, string.Empty);
                }
                else
                {
                    SetError("an error has occured");
                }
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task<string> GetJWT()
        {
            var jwt = await LocalStorage.GetItemAsync<string>("user");
            if (!string.IsNullOrWhiteSpace(UserMail))
            {
                var dataArray = jwt.Split(';', 2);
                if (dataArray.Length == 2)
                    return dataArray[1];
            }
            return string.Empty;
        }

        private void ReloadPreview()
        {
            markupString = (MarkupString)markDownModel.Html;
        }

        private void CheckPreview(bool val)
        {
            checkPreview = val;
        }

        private void DisplayEditor(bool val)
        {
            displayEditor = val;
        }

        private async Task UpsertMarkDown(HttpMethod method)
        {
            try
            {
                isLoading = true;
                successMessage = string.Empty;
                errorMessage = string.Empty;
                var requestMsg = new HttpRequestMessage(method, $"/api/markdown");
                requestMsg.Headers.Add("Authorization", "Bearer " + await GetJWT());
                requestMsg.Content = new StringContent(JsonSerializer.Serialize(markDownModel), Encoding.UTF8, "application/json"); ;
                var response = await HttpClient.SendAsync(requestMsg);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await LocalStorage.RemoveItemAsync("user");
                    isLoading = false;
                    SetError("You are not authorized, please log in again");
                    NavigationManager.NavigateTo("/", true);
                }
                else if (response.IsSuccessStatusCode)
                {
                    var message = method == HttpMethod.Put ?
                    "Updated successfully" : method == HttpMethod.Post ? "Created successfully" : string.Empty;
                    await SetSuccess(response, message);
                }
                else
                {
                    SetError("an error has occured");
                }
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task SetSuccess(HttpResponseMessage response, string message)
        {

            var content = await response.Content.ReadFromJsonAsync<MarkDownModel>();
            if (content != null)
            {
                markDownModel = content;
            }
            markupString = (MarkupString)markDownModel.Html;
            MarkDownId = markDownModel.Id;
            successMessage = message;
        }

        private void SetError(string message)
        {
            markupString = (MarkupString)markDownModel.Html;
            MarkDownId = markDownModel.Id;
            errorMessage = message;
        }
    }
}
