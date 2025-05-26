// <copyright file="Program.cs" company="Mooville">
//   Copyright (c) 2022 Roger Deetz. All rights reserved.
// </copyright>

using System.CommandLine;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;

FileInfo? inputFile = null;
FileInfo? outputFile = null;

var inputFileOption = new Option<FileInfo>(
    "--input-file",
    "The raw data file to process.");

var outputFileOption = new Option<FileInfo>(
    "--output-file",
    "The processed file.");

var rootCommand = new RootCommand { inputFileOption, outputFileOption };
rootCommand.Description = "An Uno-like card game.";

rootCommand.SetHandler(
    (FileInfo inFile, FileInfo outFile) => 
    {
        inputFile = inFile;
        outputFile = outFile;
    },
    inputFileOption, 
    outputFileOption
);

rootCommand.Invoke(args);

Console.WriteLine($"The input file is: {inputFile?.FullName ?? String.Empty}");

//Console.WriteLine($"Number of bytes in input file: {inputStream.Length}");

Console.WriteLine($"The output file is: {outputFile?.FullName ?? String.Empty}");

// Process the input file and create the output file.
//GameLogConverter.ConvertGameLogToCsv(inputFile.FullName, outputFile.FullName);

const int _maxTokenLength = 1024;

IChatClient? chatClient = null;
CancellationTokenSource? cts = null;
var topic = "The history of Uno card game.";

OnnxRuntimeGenAIChatClientFactory.InitializeGenAI();

try
{
    chatClient = await OnnxRuntimeGenAIChatClientFactory.CreateAsync(@"C:\Users\roger\.cache\aigallery\microsoft--Phi-3.5-mini-instruct-onnx\main\cpu_and_mobile\cpu-int4-awq-block-128-acc-level-4", new LlmPromptTemplate
    {
        System = "<|system|>\n{{CONTENT}}<|end|>\n",
        User = "<|user|>\n{{CONTENT}}<|end|>\n",
        Assistant = "<|assistant|>\n{{CONTENT}}<|end|>\n",
        Stop = ["<|system|>", "<|user|>", "<|assistant|>", "<|end|>"]
    });

    await Task.Run(
        async () =>
        {
            string systemPrompt = "You generate text based on a user-provided topic. Respond with only the generated content and no extraneous text.";
            string userPrompt = "Generate text based on the topic: " + topic;

            cts = new CancellationTokenSource();

            await foreach (var messagePart in chatClient?.GetStreamingResponseAsync(
                [
                    new ChatMessage(ChatRole.System, systemPrompt),
                    new ChatMessage(ChatRole.User, userPrompt)
                ],
                null,
                cts.Token))
            {
                if (messagePart is ChatResponseUpdate responseUpdate && responseUpdate.Contents.Count > 0)
                {
                    foreach (var content in responseUpdate.Contents)
                    {
                        if (content is TextContent textContent)
                        {
                            Console.Write(textContent.Text);
                        }
                        else
                        {
                            Console.WriteLine("Unknown content type.");
                        }
                    }
                }
            }

            cts?.Dispose();
            cts = null;
        });
}
catch (Exception ex)
{
    // Log an error.
}
finally
{
    chatClient?.Dispose();
    chatClient = null;
    cts?.Dispose();
    cts = null;
}


Console.WriteLine("Finished");
