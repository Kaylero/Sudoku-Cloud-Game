using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SudokuSquare : MonoBehaviour
{
    private Text numberText;
    private Image background;

    private bool isStarter;

    private void Start()
    {
        numberText = GetComponentInChildren<Text>();
        background = GetComponent<Image>();
    }

    public void SetStarter(bool isStarter)
    {
        this.isStarter = isStarter;

        if (isStarter)
        {
            try
            {
                numberText.fontStyle = FontStyle.Bold;

            }
            catch { Debug.Log(""); }
        }
        else
        {
            numberText.fontStyle = FontStyle.Italic;
        }
    }

    public bool GetStarter()
    {
        return isStarter;
    }

    public void SetNumber(string number)
    {
        numberText.text = number;
    }

    public string GetNumber()
    {
        return numberText.text;
    }

    public void ChangeColor(Color color)
    {
        background.color = color;
    }
    
}
