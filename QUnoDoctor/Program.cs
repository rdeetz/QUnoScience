﻿// <copyright file="Program.cs" company="Mooville">
//   Copyright (c) 2022 Roger Deetz. All rights reserved.
// </copyright>

using System.CommandLine;
using System.IO;

FileInfo? inputFile = null;
FileInfo? outputFile = null;

var fileInOption = new Option<FileInfo>(
    "--input-file",
    "The raw data file to process.");

var fileOutOption = new Option<FileInfo>(
    "--output-file",
    "The processed file.");

var rootCommand = new RootCommand { fileInOption, fileOutOption };
rootCommand.Description = "An Uno-like card game.";

rootCommand.SetHandler(
    (FileInfo fileIn, FileInfo fileOut) => 
    {
        Console.WriteLine($"The input file is: {fileIn?.FullName ?? null}");
        Console.WriteLine($"The output file is: {fileOut?.FullName ?? null}");

        if (fileIn != null) inputFile = fileIn;
        if (fileOut != null) outputFile = fileOut;
    },
    fileInOption, 
    fileOutOption
);

rootCommand.Invoke(args);

if (inputFile != null)
{
    var inputStream = File.OpenRead(inputFile.FullName);
    Console.WriteLine($"Number of bytes in input file: {inputStream.Length}");
}
