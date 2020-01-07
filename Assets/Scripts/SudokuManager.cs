using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SudokuManager : MonoBehaviour
{
    [SerializeField]
    //This list is set on Unity Editor
    private List<SudokuSquare> sudokuSquare;
    private int length;

    public void Start()
    {
        length = (int)Mathf.Sqrt(sudokuSquare.Count);
    }

    private bool CheckLine(int line)
    {
        bool result = false;

        SudokuSquare[] squaresToCheck = new SudokuSquare[length];

        for (int i = 0; i < length; i++)
        {
            squaresToCheck[i] = sudokuSquare[i + line * length];
        }
        
        for (int i = 0; i < length-1; i++)
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
            squaresToCheck[i] = sudokuSquare[i * length + column];
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

        for(int i = 0; i < Mathf.Sqrt(length); i++)
        {
            for (int j = 0; j < Mathf.Sqrt(length); j++)
            {
                int square = i * length + j + (sudoku % 3 * (int)Mathf.Sqrt(length)) + (length * (int)Mathf.Sqrt(length) * ((int)(sudoku / Mathf.Sqrt(length))));

                sudokuSquaresToCheck[counter] = sudokuSquare[square];
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

            if(!CheckInnerSudokus(i))
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
        if (Int32.Parse(number) >= sudokuSquare.Count)
        {
            UserMessage.ShowMessage("Number entered must be between 0 and " + sudokuSquare.Count);
            return;
        }

        if (x >= Math.Sqrt(sudokuSquare.Count) || y >= Math.Sqrt(sudokuSquare.Count))
        {
            UserMessage.ShowMessage("Coordinates number must be between 0 and " + Math.Sqrt(sudokuSquare.Count));
            return;
        }

        int square = y * 3 + x;
        sudokuSquare[square].SetNumber(number);
    }
}
