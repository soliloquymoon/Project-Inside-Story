using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour
{
    public Button[] Buttons;

    public void DisableButtons()
    {
        foreach(Button UIButton in Buttons)
            UIButton.interactable = false;
    }
    public void EnableButtons()
    {
        foreach(Button UIButton in Buttons)
            UIButton.interactable = true;
    }
}
