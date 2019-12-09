﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Cursor : MonoBehaviour {
    public PlayerPhotos playerPhotos;
    public Sprite defaultCursor;
    public Sprite canPhotographCursor;

    private Image _img;

    void Start() {
        _img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {
        _img.sprite = defaultCursor;

        if (playerPhotos.canPhotograph()) _img.sprite = canPhotographCursor;

        transform.position = Input.mousePosition;
    }
}