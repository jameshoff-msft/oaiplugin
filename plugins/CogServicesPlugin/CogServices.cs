// Copyright (c) Microsoft. All rights reserved.

using System.ComponentModel;
using Microsoft.SemanticKernel.SkillDefinition;

namespace Plugins.CogServicesPlugin;

public class CogServices
{
    string index = "rfp";
    string endpoint = "";
    string apikey = "";

    string oaiDeploymentId = "text-embedding-ada-002";
    string oaiEndpoint = "";
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
        return results[0].text;
    }
}
