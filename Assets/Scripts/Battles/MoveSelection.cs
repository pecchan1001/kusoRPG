using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelection : MonoBehaviour
{
    [SerializeField]
    RectTransform movesParent;
    SelectableText[] selectableTexts;

    int selectedIndex;

    public int SelectedIndex { get => selectedIndex; }

    public void Init()
    {
        selectableTexts = GetComponentsInChildren<SelectableText>();
        SetMovesUISize(5);

    }

    void SetMovesUISize(int Count)
    {
        Vector2 uiSize = movesParent.sizeDelta;
        uiSize.y = 50 + 50 * Count;
        movesParent.sizeDelta = uiSize;
    }

    public void HandleUpdate()
    {

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex--;
        }
        selectedIndex = Mathf.Clamp(selectedIndex, 0, selectableTexts.Length - 1);


        for (int i = 0; i < selectableTexts.Length; i++)
        {
            if (selectedIndex == i)
            {
                selectableTexts[i].SetSelectedColor(true);
            }
            else
            {
                selectableTexts[i].SetSelectedColor(false);
            }
        }

    }

    public void Open()
    {
        selectedIndex = 0;
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}