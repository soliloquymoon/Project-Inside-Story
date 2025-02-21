using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhoneManager : MonoBehaviour
{
    public GameObject Notification;
    public GameObject IglooWindow;
    public GameObject PhoneWindow;
    public GameObject OurtalkWindow;
    public UIButtons UIButtons;

    void Start()
    {
        Notification.SetActive(false);
        IglooWindow.SetActive(false);
        PhoneWindow.SetActive(false);
        OurtalkWindow.SetActive(false);
    }

    void Update()
    {
    }

    public void OpenPhoneWindow()
    {
        PhoneWindow.SetActive(true);
        UIButtons.DisableButtons();
    }

    public void MusicApp()
    {

    }

    public void MemoApp()
    {

    }

    public void SettingApp()
    {

    }

    public void Igloo()
    {
        IglooWindow.SetActive(true);
    }

    public void OurTalk()
    {
        OurtalkWindow.SetActive(true);
    }

    public void ClosePhoneWindow()
    {
        PhoneWindow.SetActive(false);
        UIButtons.EnableButtons();
    }

    public void CloseOurtalk()
    {
        OurtalkWindow.SetActive(false);
    }

    public void CloseIgloo()
    {
        IglooWindow.SetActive(false);
    }
}
