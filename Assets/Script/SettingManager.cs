using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public GameObject SettingWindow;
    public UIButtons UIButtons;
    void Start()
    {
        SettingWindow.SetActive(false);
    }

    public void OpenSettingWindow()
    {
        SettingWindow.SetActive(true);
        UIButtons.DisableButtons();
    }
    
    public void CloseSettingWindow()
    {
        SettingWindow.SetActive(false);
        UIButtons.EnableButtons();
    }
}
