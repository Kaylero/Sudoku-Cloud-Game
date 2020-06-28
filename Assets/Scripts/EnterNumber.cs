using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterNumber : MonoBehaviour
{
    [SerializeField]
    private InputField x;
    [SerializeField]
    private InputField y;
    [SerializeField]
    private InputField number;

    [SerializeField]
    private SudokuManager sudokuManager;
    [SerializeField]
    private SudokuInputManager sudokuInputManager;

    public void SetNumber()
    {
        if (sudokuInputManager.GetCurrentSudokuSquareInput() == null)
        {
            UserMessage.ShowMessage("Choose a square by clicking it.");
            return;
        }

        if (number.text != "")
        {
            sudokuManager.SetNumber(sudokuInputManager.GetCurrentSudokuSquareInput().GetSudokuSquare(), number.text);
        }
    }
}
