using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SudokuGenerator
{
    public string[] GenerateSudoku(int difficulty, int length)
    {
        string[] newSudoku = new string[length];
        int numbersToGenerate = difficulty;
        int introducedNumbers = 0;

        while (introducedNumbers < numbersToGenerate)
        {
            int generatedNumber = 0;
            do
            {
                generatedNumber = Random.Range(1, length);
            }
            while (newSudoku.Contains(generatedNumber.ToString()));

            int squareToIntroduce = 0;
            do
            {
                squareToIntroduce = Random.Range(0, length);
            }
            while (newSudoku[squareToIntroduce] != null);

            introducedNumbers++;
            newSudoku[squareToIntroduce] = generatedNumber.ToString();
        }

        return newSudoku;
    }
}
