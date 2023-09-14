// Copyright (c) Microsoft. All rights reserved.

using System.ComponentModel;
using Azure.Core;
using Microsoft.SemanticKernel.SkillDefinition;

namespace Plugins.CogServicesPlugin;

public class CogServices
{
    string index = "uyod";
    string endpoint = "https://.search.windows.net";
    string apikey = "";

    string oaiDeploymentId = "text-embedding-ada-002";
    string oaiChatDeploymentId = "gpt-35-turbo";
    string oaiEndpoint = "https://.openai.azure.com/";
    string oaiApikey = "";

    [SKFunction, Description("Search an index")]
    public string Search(string query)
    {
        return new services.CogSearch(endpoint, apikey, index).Search(query);
    }
    [SKFunction, Description("Search an index using vector embeddings")]
    public string VectorSearch(string query)
    {
        services.OpenAI openAI = new services.OpenAI(oaiEndpoint, oaiApikey, oaiDeploymentId);
        IReadOnlyList<float> embedding = openAI.GetEmbedding(query);
        var results = new services.CogSearch(endpoint, apikey, index).VectorSearch(query, embedding, 3);
        return results[0].content;
    }

    [SKFunction, Description("Search an index using vector embeddings and Use Your Own Data API")]
    public string UseYourOwnData(string query)
    {
        services.UseYourOwnData uyod = new services.UseYourOwnData(oaiEndpoint, oaiApikey, oaiChatDeploymentId, endpoint, apikey, index);
        string result = uyod.query(query);

        return result;
    }
}
