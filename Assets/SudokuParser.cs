using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class SudokuParser
{
    private static string lastChar = "";

    public static string[] SudokuStringToSudokuArray(string sudoku)
    {
        List<string> result = new List<string>();

        foreach (char c in sudoku)
        {
            string cStr = c.ToString();

            if (IsNumeric(c.ToString()))
            {
                lastChar = cStr;
            }
            else if (cStr == ",")
            {
                result.Add(lastChar);
                lastChar = "";
            }
        }

        return result.ToArray();
    }

    public static string[] SudokuSquareListToStringArray(List<SudokuSquare> squares)
    {
        string[] result  = new string[squares.Count];
        
        for (int i = 0; i < squares.Count; i++)
        {
            result[i] = squares[i].GetNumber();
        }

        return result;
    }

    private static bool IsNumeric(string strToCheck)
    {
        Regex rg = new Regex(@"^[0-9]*$");
        return rg.IsMatch(strToCheck);
    }
}
