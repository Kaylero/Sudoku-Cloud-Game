using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuManager : MonoBehaviour
{
    [SerializeField]
    //This list is set on Unity Editor
    private List<SudokuSquare> sudoku;
    private int length;
    private SudokuChecker sudokuChecker;

    public void Start()
    {
        length = (int)Mathf.Sqrt(sudoku.Count);
        sudokuChecker = new SudokuChecker(sudoku);
    }

    public bool CheckSudoku()
    {
        return sudokuChecker.CheckSudoku();
    }

    public void SetNewSudoku(string[] newSudoku)
    {
        for (int i = 0; i < newSudoku.Length; i++)
        {
            sudoku[i].SetStarter(newSudoku[i] != "");
            sudoku[i].SetNumber(newSudoku[i]);
            length = newSudoku.Length;
        }
    }

    public void SetNumber(int x, int y, string number)
    {
        if (Int32.Parse(number) >= sudoku.Count)
        {
            UserMessage.ShowMessage("Number entered must be between 1 and " + sudoku.Count);
            return;
        }

        if (x >= Math.Sqrt(sudoku.Count) || y >= Math.Sqrt(sudoku.Count))
        {
            UserMessage.ShowMessage("Coordinates number must be between 0 and " + (Math.Sqrt(sudoku.Count) - 1));
            return;
        }

        int square = y * (int)(Mathf.Sqrt(length)) + x;
        sudoku[square].SetNumber(number);
    }

    public void SetNumber(SudokuSquare sudokuSquare, string number)
    {
        if (Int32.Parse(number) >= sudoku.Count)
        {
            UserMessage.ShowMessage("Number entered must be between 1 and " + sudoku.Count);
            return;
        }

        sudokuSquare.SetNumber(number);
    }

    //TODO: Move this to a class that stores the lastchecked position
    public bool IsSudokuComplete()
    {
        bool complete = false;
        int index = 0;

        while (!complete)
        {
            if (index >= sudoku.Count)
            {
                complete = true;
            }

            if (sudoku[index].GetNumber() == "")
            {
                return false;
            }

            index++;
        }

        return true;
    }

    public Vector2 GetNextIncompleteSquare()
    {
        bool complete = false;
        int index = 0;

        while (!complete)
        {
            if (index >= sudoku.Count)
            {
                complete = true;
            }

            if (sudoku[index].GetNumber() == "")
            {
                return new Vector2(index % length, index / length);
            }

            index++;
        }

        return new Vector2(55,55);
    }

    public List<SudokuSquare> GetSudoku()
    {
        return sudoku;
    }
}
