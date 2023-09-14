// Copyright (c) Microsoft. All rights reserved.

using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(0)
        .AddDebug();
});

Environment.SetEnvironmentVariable("Global:LlmService", "AzureOpenAI");
Environment.SetEnvironmentVariable("OpenAI:ModelType", "chat-completion");


// Create kernel
var deployment = "gpt-35-turbo";
var endpoint = "";
var apikey = "";

IKernel kernel = new KernelBuilder().WithAzureChatCompletionService(deployment, endpoint, apikey)
    .WithCompletionService()
    .WithLoggerFactory(loggerFactory)
    .Build();

// Import the Math Plugin
var mathPlugin = kernel.ImportSkill(new Plugins.MathPlugin.Math(), "MathPlugin");

// Make a request that runs the Sqrt function
var result = await kernel.RunAsync("12", mathPlugin["Sqrt"]);
Console.WriteLine(result);

// Import the Cog Services Plugin
var csPlugin = kernel.ImportSkill(new Plugins.CogServicesPlugin.CogServices(), "CogServicesPlugin");

// Make a request that runs the Sqrt function
var result2 = await kernel.RunAsync("Are there presentation materials?", csPlugin["VectorSearch"]);
Console.WriteLine(result2);

// Make a request that runs the Sqrt function
var result3 = await kernel.RunAsync("Are there presentation materials?", csPlugin["UseYourOwnData"]);
Console.WriteLine(result3);
