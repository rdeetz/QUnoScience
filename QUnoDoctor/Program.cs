// <copyright file="Program.cs" company="Mooville">
//   Copyright (c) 2022 Roger Deetz. All rights reserved.
// </copyright>

using System.CommandLine;
using System.IO;

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
var inputStream = File.OpenRead(inputFile.FullName);

Console.WriteLine($"Number of bytes in input file: {inputStream.Length}");

Console.WriteLine($"The output file is: {outputFile?.FullName ?? String.Empty}");
var outputStream = File.OpenWrite(outputFile.FullName);

// TODO Process the input file and create the output file.

inputStream.Close();
outputStream.Close();

Console.WriteLine("Finished");
