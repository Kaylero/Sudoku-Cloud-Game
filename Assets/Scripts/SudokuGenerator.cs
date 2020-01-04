using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SudokuGenerator
{
    public string[] GenerateSudoku(Difficulty difficulty)
    {
        string[] newSudoku = new string[9];
        int numbersToGenerate = (int)difficulty;
        int introducedNumbers = 0;

        while (introducedNumbers < numbersToGenerate)
        {
            int generatedNumber = 0;
            do
            {
                generatedNumber = Random.Range(1, 10);
            }
            while (newSudoku.Contains(generatedNumber.ToString()));

            int squareToIntroduce = 0;
            do
            {
                squareToIntroduce = Random.Range(0, 9);
            }
            while (newSudoku[squareToIntroduce] != null);

            introducedNumbers++;
            newSudoku[squareToIntroduce] = generatedNumber.ToString();
        }

        return newSudoku;
    }
}

public enum Difficulty
{
    Easy = 5,
    Medium = 4,
    Hard = 3
}
