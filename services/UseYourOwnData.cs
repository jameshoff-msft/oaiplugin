using Azure;
using Azure.AI.OpenAI;
using System.Text.Json;

namespace services
{
    public class UseYourOwnData
    {
        private string _endpoint;
        private string _apikey;
        private string _deploymentId;
        private string _searchEndpoint;
        private string _searchApikey;
        private string _searchIndex;

        public UseYourOwnData(string endpoint, string apikey, string deploymentId, string searchEndpoint, string searchApiKey, string searchIndex)
        {
            this._endpoint = endpoint;
            this._apikey = apikey;
            this._deploymentId = deploymentId;
            this._searchEndpoint = searchEndpoint;
            this._searchApikey = searchApiKey;
            this._searchIndex = searchIndex;
        }

        public string query(string query)
        {
            string azureOpenAIEndpoint = _endpoint;
            string azureOpenAIKey = _apikey;
            string searchEndpoint = _searchEndpoint;
            string searchKey = _searchApikey;
            string searchIndex = _searchIndex;
            string deploymentName = _deploymentId;

            var client = new OpenAIClient(new Uri(azureOpenAIEndpoint), new AzureKeyCredential(azureOpenAIKey));

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages =
                {
                    new ChatMessage(ChatRole.User, query),
                },
                AzureExtensionsOptions = new AzureChatExtensionsOptions()
                {
                    Extensions =
        {
            new AzureCognitiveSearchChatExtensionConfiguration()
            {
                SearchEndpoint = new Uri(searchEndpoint),
                SearchKey = new AzureKeyCredential(searchKey),
                IndexName = searchIndex,
            },
        }
                }
            };

            Response<ChatCompletions> response = client.GetChatCompletions(deploymentName, chatCompletionsOptions);

            ChatMessage responseMessage = response.Value.Choices[0].Message;

            string result = "";
            result += $"Message from {responseMessage.Role}:";
            //Console.WriteLine($"Message from {responseMessage.Role}:");
            //Console.WriteLine("===");
            result += "===";
            //Console.WriteLine(responseMessage.Content);
            result += responseMessage.Content;
            //Console.WriteLine("===");
            result += "===";

            //Console.WriteLine($"Context information (e.g. citations) from chat extensions:");
            result += $"Context information (e.g. citations) from chat extensions:";
            //Console.WriteLine("===");
            result += "===";
            foreach (ChatMessage contextMessage in responseMessage.AzureExtensionsContext.Messages)
            {
                string contextContent = contextMessage.Content;
                try
                {
                    var contextMessageJson = JsonDocument.Parse(contextMessage.Content);
                    contextContent = JsonSerializer.Serialize(contextMessageJson, new JsonSerializerOptions()
                    {
                        WriteIndented = true,
                    });
                }
                catch (JsonException)
                { }
                result += $"{contextMessage.Role}: {contextContent}";
                //Console.WriteLine($"{contextMessage.Role}: {contextContent}");
            }
            Console.WriteLine("===");
            result += "===";
            return result;
        }
    }
}
