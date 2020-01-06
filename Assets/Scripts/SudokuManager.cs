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
        bool result = false;

        SudokuSquare square1 = sudokuSquare[line * 3];
        SudokuSquare square2 = sudokuSquare[line * 3 + 1];
        SudokuSquare square3 = sudokuSquare[line * 3 + 2];

        if (CheckSquares(square1, square2))
        {
            result = false;
        }

        if (CheckSquares(square2, square3))
        {
            result = false;
        }

        if (CheckSquares(square1, square3))
        {
            result = false;
        }

        return result;
    }

    private bool CheckColumn(int column)
    {
        bool result = true;

        SudokuSquare square1 = sudokuSquare[column];
        SudokuSquare square2 = sudokuSquare[column + 3];
        SudokuSquare square3 = sudokuSquare[column + 6];

        if (CheckSquares(square1, square2))
        {
            result = false;
        }

        if (CheckSquares(square2, square3))
        {
            result = false;
        }

        if (CheckSquares(square1, square3))
        {
            result = false;
        }

        return result;
    }

    private bool CheckSquares(SudokuSquare square1, SudokuSquare square2)
    {
        bool result = true;

        if (square1.GetNumber() == "")
        {
            square1.ChangeColor(Color.red);
            result = false;
        }

        if (square2.GetNumber() == "")
        {
            square2.ChangeColor(Color.red);
            result = false;
        }

        if (square1.GetNumber() == square2.GetNumber())
        {
            square1.ChangeColor(Color.red);
            square2.ChangeColor(Color.red);
            result = false;
        }

        return result;
    }

    public bool CheckSudoku()
    {
        bool result = true;

        for (int i = 0; i < sudokuSquare.Length/3; i++)
        {
            if (!CheckLine(i))
            {
                result = false;
            }
            if (!CheckColumn(i))
            {
                result = false;
            }
        }
        return result;
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
        if (Int32.Parse(number) >= sudokuSquare.Length)
        {
            UserMessage.ShowMessage("Number entered must be between 0 and " + sudokuSquare.Length);
            return;
        }

        if (x >= Math.Sqrt(sudokuSquare.Length) || y >= Math.Sqrt(sudokuSquare.Length))
        {
            UserMessage.ShowMessage("Coordinates number must be between 0 and " + Math.Sqrt(sudokuSquare.Length));
            return;
        }

        int square = y * 3 + x;
        sudokuSquare[square].SetNumber(number);
    }
}
