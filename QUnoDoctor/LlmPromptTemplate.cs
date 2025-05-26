// <copyright file="LlmPromptTemplate.cs" company="Mooville">
//   Copyright (c) 2025 Roger Deetz. All rights reserved.
// </copyright>

internal class LlmPromptTemplate
{
    public string? System { get; init; }
    public string? User { get; init; }
    public string? Assistant { get; init; }
    public string[]? Stop { get; init; }
}
