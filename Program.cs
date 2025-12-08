using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

class TrashCompactorSolver
{
    static void Main()
    {
        string input = System.IO.File.ReadAllText("input.txt");
        
        long grandTotal = SolveWorksheet(input);
        Console.WriteLine($"Grand Total: {grandTotal}");
    }

    static long SolveWorksheet(string input)
    {
        var lines = input.Split('\n')
            .Select(l => l.TrimEnd('\r', '\n'))
            .ToArray();
        
        while (lines.Length > 0 && string.IsNullOrWhiteSpace(lines[lines.Length - 1]))
        {
            lines = lines.Take(lines.Length - 1).ToArray();
        }
        
        if (lines.Length == 0) return 0;
        
        int maxLen = lines.Max(l => l.Length);
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].PadRight(maxLen);
        }
        

        List<(int start, int end)> problemRanges = new List<(int, int)>();
        int col = 0;
        
        while (col < maxLen)
        {
            while (col < maxLen && IsColumnAllSpaces(lines, col))
            {
                col++;
            }
            
            if (col >= maxLen) break;
            
            int start = col;
            
            while (col < maxLen && !IsColumnAllSpaces(lines, col))
            {
                col++;
            }
            
            problemRanges.Add((start, col));
        }
        
        long grandTotal = 0;
        
        for (int p = 0; p < problemRanges.Count; p++)
        {
            var (start, end) = problemRanges[p];
            
            List<string> problemLines = new List<string>();
            for (int row = 0; row < lines.Length; row++)
            {
                string segment = lines[row].Substring(start, end - start).TrimEnd();
                problemLines.Add(segment);
            }
            
            string opLine = problemLines[problemLines.Count - 1].Trim();
            char operation = opLine.Length > 0 ? opLine[0] : '+';
            
            List<long> numbers = new List<long>();
            for (int row = 0; row < problemLines.Count - 1; row++)
            {
                string numStr = problemLines[row].Trim();
                if (!string.IsNullOrWhiteSpace(numStr) && long.TryParse(numStr, out long num))
                {
                    numbers.Add(num);
                }
            }
            
            long result = CalculateProblem(numbers, operation);
            grandTotal += result;
            
            Console.WriteLine($"Problem {p + 1}: {string.Join($" {operation} ", numbers)} = {result}");
        }
        
        return grandTotal;
    }
    
    static bool IsColumnAllSpaces(string[] lines, int col)
    {
        foreach (string line in lines)
        {
            if (col < line.Length && line[col] != ' ')
            {
                return false;
            }
        }
        return true;
    }
    
    static long CalculateProblem(List<long> numbers, char operation)
    {
        if (numbers.Count == 0) return 0;
        
        long result = numbers[0];
        
        for (int i = 1; i < numbers.Count; i++)
        {
            if (operation == '*')
            {
                result *= numbers[i];
            }
            else if (operation == '+')
            {
                result += numbers[i];
            }
        }
        
        return result;
    }
}