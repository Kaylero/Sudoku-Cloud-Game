using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SudokuSquareInput : MonoBehaviour, IPointerClickHandler
{
    private Image background;
    [SerializeField]
    private SudokuSquare sudokuSquare;
    private SudokuInputManager sudokuInputManager;

    private void Start()
    {
        sudokuInputManager = FindObjectOfType<SudokuInputManager>();
        background = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (sudokuSquare.GetStarter())
        {
            return;
        }

        background.color = sudokuInputManager.GetHighlightColor();
        sudokuInputManager.OnNewInputClicked(this);
    }

    public SudokuSquare GetSudokuSquare()
    {
        return sudokuSquare;
    }

    public void ClearColor()
    {
        background.color = new Color(0, 0, 0, 0);
    }
}
