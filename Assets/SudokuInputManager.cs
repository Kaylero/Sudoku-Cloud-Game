using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SudokuInputManager : MonoBehaviour
{
    [SerializeField]
    private Color highlightColor;
    private SudokuSquareInput currentInput;

    public Color GetHighlightColor()
    {
        return highlightColor;
    }

    public SudokuSquareInput GetCurrentSudokuSquareInput()
    {
        return currentInput;
    }

    public void OnNewInputClicked(SudokuSquareInput sudokuSquareInput)
    {
        if (currentInput != null && currentInput != sudokuSquareInput)
        {
            currentInput.ClearColor();
        }

        currentInput = sudokuSquareInput;
    }

    public void ClearCurrentSquareInput()
    {
        if (currentInput == null)
        {
            return;
        }

        currentInput.ClearColor();
        currentInput = null;
    }
}
