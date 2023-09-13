using Azure;
using Azure.AI.OpenAI;

namespace services
{
    public class OpenAI
    {
        private string _endpoint;
        private string _apikey;
        private string _deploymentId;

        public OpenAI(string endpoint, string apikey, string deploymentId)
        {
            this._endpoint = endpoint;
            this._apikey = apikey;
            this._deploymentId = deploymentId;
        }

        public IReadOnlyList<float> GetEmbedding(string query)
        {
            AzureKeyCredential credentials = new(_apikey);

            Uri oaiEndpoint = new(_endpoint);
            OpenAIClient openAIClient = new(oaiEndpoint, credentials);

            EmbeddingsOptions embeddingOptions = new(query);

            var returnValue = openAIClient.GetEmbeddings(_deploymentId, embeddingOptions);

            return returnValue.Value.Data[0].Embedding;
            
        }
    }
}
