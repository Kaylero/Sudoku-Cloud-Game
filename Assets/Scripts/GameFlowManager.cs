using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField]
    private SudokuManager sudokuManager;
    private SudokuGenerator sudokuGenerator = new SudokuGenerator();

    private readonly string VALIDATE_OK_MESSAGE = "Sudoku solved";
    private readonly string VALIDATE_FAIL_MESSAGE = "Sudoku not solved";

    public void Start()
    {
        GenerateNewSudoku(5);
    }

    public void GenerateNewSudoku(int difficulty)
    {
        sudokuManager.SetNewSudoku(sudokuGenerator.GenerateSudoku(difficulty, 9));
    }

    public void ValidateSudoku()
    {
        if (sudokuManager.CheckSudoku())
        {
            UserMessage.ShowMessage(VALIDATE_OK_MESSAGE);
        }
        else
        {
            UserMessage.ShowMessage(VALIDATE_FAIL_MESSAGE);
        }
    }

    public void EnterNumber(int x, int y, string number)
    {
        sudokuManager.SetNumber(x, y, number);
    }
}
