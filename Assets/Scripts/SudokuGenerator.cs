using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SudokuGenerator
{
    string[] newSudoku;
    List<int> numbersGenerated = new List<int>();
    int length;


    public string[] GenerateSudoku(int difficulty, int length)
    {
        newSudoku = new string[length * length];
        this.length = length;


        for (int currentSudoku = 0; currentSudoku < length; currentSudoku++)
        {
            List<string> innerSudoku = new List<string>();

            //Generation of X numbers; X = difficulty
            for (int j = 0; j < difficulty; j++)
            {
                bool numberCorrectlyPositioned = false;
                while (!numberCorrectlyPositioned)
                {
                    string generatedNumber = Random.Range(1, length + 1).ToString();

                    if (innerSudoku.Contains(generatedNumber))
                    {
                        continue;
                    }

                    int currentRow = Random.Range(GetMinRow(currentSudoku), GetMaxRow(currentSudoku));
                    int currentColumn = Random.Range(GetMinColumn(currentSudoku), GetMaxColumn(currentSudoku));

                    if (GetValueOfCoordinate(currentRow, currentColumn) != null)
                    {
                        continue;
                    }

                    string[] rowSquares = GetRowSquares(currentRow);
                    string[] columnSquares = GetColumnSquares(currentColumn);

                    if (!rowSquares.Contains(generatedNumber) && !columnSquares.Contains(generatedNumber))
                    {
                        numberCorrectlyPositioned = true;
                    }
                    else
                    {
                        continue;
                    }

                    newSudoku[(currentRow * length) + currentColumn] = generatedNumber;
                    innerSudoku.Add(generatedNumber);
                }
            }
        }

        for (int i = 0; i < newSudoku.Length; i++)
        {
            if (newSudoku[i] == null)
            {
                newSudoku[i] = "";
            }
        }
       
        return newSudoku;
    }


    public string[] GetRowSquares(int row)
    {
        string[] result = new string[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = newSudoku[i + row * length];
        }

        return result;
    }

    public string[] GetColumnSquares(int column)
    {
        string[] result = new string[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = newSudoku[column + i * length];
        }

        return result;
    }

    public string GetValueOfCoordinate(int row, int column)
    {
        return newSudoku[(row * length) + column];
    }

    private int GetMinColumn(int currentSudoku)
    {
        return (currentSudoku % (int)Mathf.Sqrt(length)) * (int)Mathf.Sqrt(length);
    }

    private int GetMaxColumn(int currentSudoku)
    {
        return (currentSudoku % (int)Mathf.Sqrt(length)) * (int)Mathf.Sqrt(length) + (int)Mathf.Sqrt(length);

    }

    private int GetMinRow(int currentSudoku)
    {
        return (currentSudoku / (int)Mathf.Sqrt(length)) * (int)Mathf.Sqrt(length);

    }

    private int GetMaxRow(int currentSudoku)
    {
        return (currentSudoku / (int)Mathf.Sqrt(length)) * (int)Mathf.Sqrt(length) + (int)Mathf.Sqrt(length);

    }
}
