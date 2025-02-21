using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public Text text;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setChat(string chat)
    {
        text.text = chat;
    }
}
