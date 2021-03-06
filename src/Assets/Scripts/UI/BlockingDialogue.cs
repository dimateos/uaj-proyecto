﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Text))]
public class BlockingDialogue : MonoBehaviour
{
    public string[] dialogues;
    public string nextScene;
    public double minDelay = 0.5;
    private double currentDelay = 0;

    private int _currentText = 0;
    private Text _text;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
        _text.text = dialogues[_currentText];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //custom fixed delay
        if (currentDelay > 0) {
            currentDelay -= Time.fixedDeltaTime;
            return; //skip this frame anyway
        }

        if (Keyboard.current.anyKey.isPressed
            || Mouse.current.leftButton.isPressed
            || Mouse.current.rightButton.isPressed
            || Mouse.current.middleButton.isPressed) nextText();
    }

    private void nextText() {
        //Debug.Log("Next of: " + _text.text);
        currentDelay = minDelay;

        if (_currentText < dialogues.Length - 1)
            _text.text = dialogues[++_currentText];
        else SceneManager.LoadScene(nextScene);
    }
}
