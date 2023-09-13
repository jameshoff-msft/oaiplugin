using Azure.Search.Documents;
using Azure;
using Azure.Search.Documents.Models;

namespace services
{
    public class CogSearchResult
    {
        public string? text { get; set; }
    }
    class CogSearch
    {
        private string _endpoint;
        private string _apikey;
        private string _index;

        public CogSearch(string endpoint, string apikey, string index)
        {
            this._endpoint = endpoint;
            this._apikey = apikey;
            this._index = index;
        }

        public string Search(string query)
        {
            Uri endpoint = new Uri(_endpoint);
            AzureKeyCredential credential = new AzureKeyCredential(_apikey);
            SearchClient searchClient = new SearchClient(endpoint, _index, credential);

            // Get and report the number of documents in the index
            SearchResults<CogSearchResult> results = searchClient.Search<CogSearchResult>(query);
            foreach (SearchResult<CogSearchResult> result in results.GetResults())
            {
                CogSearchResult doc = result.Document;
                return doc.text;
                //Console.WriteLine($"{doc.text}");
            }
            return null;
        }

        public CogSearchResult[] VectorSearch(string query, IReadOnlyList<float> embedding, int numResults)
        {
            //IReadOnlyList<float> vectorizedResult = VectorSearchEmbeddings.SearchVectorizeDescription; // "Top hotels in town"

            Uri endpoint = new Uri(_endpoint);
            AzureKeyCredential credential = new AzureKeyCredential(_apikey);
            SearchClient searchClient = new SearchClient(endpoint, _index, credential);

            SearchResults<CogSearchResult> response = searchClient.Search<CogSearchResult>(
                    query,
                    new SearchOptions
                    {
                        Vectors = { new() { Value = embedding, KNearestNeighborsCount = numResults, Fields = { "vector" } } },
                    });

            Console.WriteLine($"Simple Hybrid Search Results:");
            var results = new CogSearchResult[numResults];
            int count = 0;
            foreach (SearchResult<CogSearchResult> result in response.GetResults())
            {
                results[count] = result.Document;
                count++;
                if(count == numResults)
                {
                    break;
                }
            }
            return results;
        }
    }
}
