using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDifficultyMenu : MonoBehaviour
{
    [SerializeField]
    private Button easy;
    [SerializeField]
    private Button medium;
    [SerializeField]
    private Button hard;

    public static Difficulty difficulty = Difficulty.Easy;

    public void Start()
    {
        ColorButtonsOnDifficulty();
    }

    public void SelectDifficulty(int difficulty)
    {
        SelectDifficultyMenu.difficulty = (Difficulty)difficulty;
        ColorButtonsOnDifficulty();
    }

    private void ColorButtonsOnDifficulty()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                easy.image.color = Color.green;
                medium.image.color = Color.white;
                hard.image.color = Color.white;
                break;
            case Difficulty.Medium:
                easy.image.color = Color.white;
                medium.image.color = Color.green;
                hard.image.color = Color.white;
                break;
            case Difficulty.Hard:
                easy.image.color = Color.white;
                medium.image.color = Color.white;
                hard.image.color = Color.green;
                break;
        }
    }
}

public enum Difficulty
{
    Easy = 0,
    Medium = 1,
    Hard = 2
}

