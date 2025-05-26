// <copyright file="GameLogConverter.cs" company="Mooville">
//   Copyright (c) 2025 Roger Deetz. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public static class GameLogConverter
{
    public static void ConvertGameLogToCsv(string inputFilePath, string outputFilePath)
    {
        // Read all lines from the input file
        string[] lines = File.ReadAllLines(inputFilePath);
        
        // Create a list to store CSV records
        List<string> csvRecords = new List<string>();
        
        // Add CSV header
        csvRecords.Add("GameNumber,TurnNumber,Player,Action,CurrentCard,CardPlayed,ChosenColor,PlayableCards");
        
        int currentGameNumber = 0;
        
        foreach (string line in lines)
        {
            // Parse game number when a new game starts
            if (line.Contains("Added Player") && line.Contains("to game #"))
            {
                // Extract game number
                int indexOfHash = line.IndexOf("#");
                if (indexOfHash >= 0 && int.TryParse(line.Substring(indexOfHash + 1), out int gameNumber))
                {
                    currentGameNumber = gameNumber;
                }
                continue;
            }
            
            // Parse turn information in new format
            var turnMatch = Regex.Match(line, @"\[Turn (\d+) current card is (.*?)\]");
            if (turnMatch.Success)
            {
                int turnNumber = int.Parse(turnMatch.Groups[1].Value);
                string currentCard = turnMatch.Groups[2].Value;
                
                // Extract player information
                var playerMatch = Regex.Match(line, @"Player (\d+) (\w+)");
                if (playerMatch.Success)
                {
                    string playerNumber = playerMatch.Groups[1].Value;
                    string action = playerMatch.Groups[2].Value;
                    
                    string cardPlayed = "N/A";
                    string chosenColor = "";
                    
                    if (action == "played")
                    {
                        // Extract card played
                        var cardMatch = Regex.Match(line, @"played (.*?)\.");
                        if (cardMatch.Success)
                        {
                            cardPlayed = cardMatch.Groups[1].Value.TrimEnd('.');
                            
                            // Handle wild card color choice
                            if (cardPlayed.Contains("Wild") && line.Contains("and chose"))
                            {
                                var wildMatch = Regex.Match(line, @"Wild Wild and chose (.*?)\.");
                                if (wildMatch.Success)
                                {
                                    chosenColor = wildMatch.Groups[1].Value;
                                    cardPlayed = "Wild Wild";
                                }
                            }
                        }
                    }
                    
                    // Extract playable cards
                    string playableCards = "";
                    var playableMatch = Regex.Match(line, @"\(Could have played (.*?)\)");
                    if (playableMatch.Success)
                    {
                        playableCards = playableMatch.Groups[1].Value;
                    }
                    
                    // Add record to CSV
                    csvRecords.Add($"{currentGameNumber},{turnNumber},{playerNumber},{action},{currentCard},{cardPlayed},{chosenColor},{playableCards}");
                }
            }
        }
        
        // Write CSV records to output file
        File.WriteAllLines(outputFilePath, csvRecords);

        return;
    }
}
