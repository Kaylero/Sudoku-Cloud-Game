using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField]
    private SudokuManager sudokuManager;
    [SerializeField]
    private SudokuInputManager sudokuInputManager = new SudokuInputManager();
    private SudokuGenerator sudokuGenerator = new SudokuGenerator();

    private readonly string VALIDATE_OK_MESSAGE = "Sudoku solved";
    private readonly string VALIDATE_FAIL_MESSAGE = "Sudoku not solved";

    public void Start()
    {
        GenerateNewSudoku();
    }

    public void GenerateNewSudoku()
    {
        int difficulty;

        switch (SelectDifficultyMenu.difficulty)
        {
            case (Difficulty.Easy):
                difficulty = 5;
                break;
            case (Difficulty.Medium):
                difficulty = 4;
                break;
            case (Difficulty.Hard):
                difficulty = 3;
                break;
            default:
                difficulty = 5;
                break;
        }

        sudokuManager.SetNewSudoku(sudokuGenerator.GenerateSudoku(difficulty, 9));
        sudokuInputManager.ClearCurrentSquareInput();
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
