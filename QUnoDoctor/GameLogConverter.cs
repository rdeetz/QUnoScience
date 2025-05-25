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
        csvRecords.Add("GameNumber,Player,Action,CardPlayed,ChosenColor");
        
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
            }
            // Parse player actions (played card)
            else if (line.StartsWith("Player ") && line.Contains("played"))
            {
                // Extract player number
                string playerText = line.Substring(7); // Skip "Player "
                string playerNumber = playerText.Substring(0, playerText.IndexOf(" "));
                
                // Extract the card information
                string cardInfo = line.Substring(line.IndexOf("played") + 7).TrimEnd('.');
                
                // Check if it's a wild card
                if (cardInfo.StartsWith("Wild"))
                {
                    string wildType = "";
                    string chosenColor = "";
                    
                    // Parse wild card and chosen color
                    if (cardInfo.Contains("and chose"))
                    {
                        int andChoseIndex = cardInfo.IndexOf("and chose");
                        wildType = cardInfo.Substring(0, andChoseIndex).Trim();
                        chosenColor = cardInfo.Substring(andChoseIndex + 10).TrimEnd('.').Trim();
                    }
                    else
                    {
                        wildType = cardInfo.Trim();
                    }
                    
                    csvRecords.Add($"{currentGameNumber},{playerNumber},Played,{wildType},{chosenColor}");
                }
                else
                {
                    // Regular card (Color Value)
                    string[] cardParts = cardInfo.Split(' ', 2);
                    if (cardParts.Length == 2)
                    {
                        string color = cardParts[0];
                        string value = cardParts[1];
                        csvRecords.Add($"{currentGameNumber},{playerNumber},Played,{color} {value},");
                    }
                }
            }
            // For "drew a card" actions
            else if (line.StartsWith("Player ") && line.Contains("drew a card"))
            {
                string playerText = line.Substring(7); // Skip "Player "
                string playerNumber = playerText.Substring(0, playerText.IndexOf(" "));
                csvRecords.Add($"{currentGameNumber},{playerNumber},Drew,N/A,");
            }
        }
        
        // Write CSV records to output file
        File.WriteAllLines(outputFilePath, csvRecords);

        return;
    }
}
