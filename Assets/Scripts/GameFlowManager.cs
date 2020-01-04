using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField]
    private SudokuManager sudokuManager;
    private SudokuGenerator sudokuGenerator = new SudokuGenerator();

    [SerializeField]
    private Text gameInfo;

    private readonly string VALIDATE_OK_MESSAGE = "Sudoku solved";
    private readonly string VALIDATE_FAIL_MESSAGE = "Sudoku not solved";

    public void Start()
    {
        sudokuManager.SetNewSudoku(sudokuGenerator.GenerateSudoku(Difficulty.Easy));
    }

    public void ValidateSudoku()
    {
        if (sudokuManager.CheckSudoku())
        {
            gameInfo.text = VALIDATE_OK_MESSAGE;
        }
        else
        {
            gameInfo.text = VALIDATE_FAIL_MESSAGE;
        }
    }

    public void EnterNumber(int x, int y, string number)
    {
        sudokuManager.SetNumber(x, y, number);
    }
}
