using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuManager : MonoBehaviour
{
    [SerializeField]
    private SudokuSquare[] sudokuSquare = new SudokuSquare[9];

    private bool CheckLine(int line)
    {
        string square1 = sudokuSquare[line * 3].GetNumber();
        string square2 = sudokuSquare[line * 3 + 1].GetNumber();
        string square3 = sudokuSquare[line * 3 + 2].GetNumber();

        return !(square1 == square2 || square2 == square3 || square2 == square3);
    }

    private bool CheckColumn(int column)
    {
        string square1 = sudokuSquare[column].GetNumber();
        string square2 = sudokuSquare[column + 3].GetNumber();
        string square3 = sudokuSquare[column + 6].GetNumber();

        return !(square1 == square2 || square2 == square3 || square2 == square3);
    }

    public bool CheckSudoku()
    {
        for (int i = 0; i < sudokuSquare.Length/3; i++)
        {
            if (!CheckLine(i) || !CheckColumn(i))
            {
                return false;
            }
        }
        return true;
    }

    public void SetNewSudoku(string[] newSudoku)
    {
        for (int i = 0; i < newSudoku.Length; i++)
        {
            sudokuSquare[i].SetNumber(newSudoku[i]);
        }
    }

    public void SetNumber(int x, int y, string number)
    {
         //TODO: check when number is not in range
        if (x >= Math.Sqrt(sudokuSquare.Length) || y >= Math.Sqrt(sudokuSquare.Length))
        {
            //TODO: display correct number instead of saying just length
            Debug.LogError("Coordinates number must be between 0 and sudoku length: ");
            return;
        }

        int square = y * 3 + x;

        sudokuSquare[square].SetNumber(number);
    }
}
