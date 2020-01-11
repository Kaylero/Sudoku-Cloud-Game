using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SudokuChecker
{
    private List<SudokuSquare> sudoku;
    private int length;

    public SudokuChecker(List<SudokuSquare> sudoku)
    {
        this.sudoku = sudoku;
        length = (int)Mathf.Sqrt(sudoku.Count);
    }

    private bool CheckLine(int line)
    {
        bool result = false;

        SudokuSquare[] squaresToCheck = new SudokuSquare[length];

        for (int i = 0; i < length; i++)
        {
            squaresToCheck[i] = sudoku[i + line * length];
        }

        for (int i = 0; i < length - 1; i++)
        {
            for (int j = i + 1; j < length; j++)
            {
                if (CheckSquares(squaresToCheck[i], squaresToCheck[j]))
                {
                    result = false;
                }
            }
        }

        return result;
    }

    private bool CheckColumn(int column)
    {
        bool result = true;

        SudokuSquare[] squaresToCheck = new SudokuSquare[length];

        for (int i = 0; i < length; i++)
        {
            squaresToCheck[i] = sudoku[i * length + column];
        }

        for (int i = 0; i < length - 1; i++)
        {
            for (int j = i + 1; j < length; j++)
            {
                if (CheckSquares(squaresToCheck[i], squaresToCheck[j]))
                {
                    result = false;
                }
            }
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

    private bool CheckInnerSudokus(int sudoku)
    {
        SudokuSquare[] sudokuSquaresToCheck = new SudokuSquare[length];

        int counter = 0;

        for (int i = 0; i < Mathf.Sqrt(length); i++)
        {
            for (int j = 0; j < Mathf.Sqrt(length); j++)
            {
                int square = i * length + j + (sudoku % 3 * (int)Mathf.Sqrt(length)) + (length * (int)Mathf.Sqrt(length) * ((int)(sudoku / Mathf.Sqrt(length))));

                sudokuSquaresToCheck[counter] = this.sudoku[square];
                counter++;
            }
        }

        string[] numbersToCheck = new string[length];

        for (int i = 0; i < length; i++)
        {
            if (numbersToCheck.Contains(sudokuSquaresToCheck[i].GetNumber()))
            {
                return false;
            }
            numbersToCheck[i] = sudokuSquaresToCheck[i].GetNumber();
        }

        return true;
    }

    public bool CheckSudoku(List<SudokuSquare> newSudoku)
    {
        sudoku = newSudoku;
        return CheckSudoku();
    }

    public bool CheckSudoku()
    {
        bool result = true;

        for (int i = 0; i < length; i++)
        {
            if (!CheckLine(i))
            {
                result = false;
            }

            if (!CheckColumn(i))
            {
                result = false;
            }

            if (!CheckInnerSudokus(i))
            {
                result = false;
            }
        }

        return result;
    }
}
