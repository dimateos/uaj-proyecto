using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{

    public Image _selected;
    public Image _unselected1;
    public Image _unselected2;

    public void Start()
    {
        ChangeColour();
    }

    public void ChangeColour()
    {
        _selected.color = Color.green;
        _unselected1.color = Color.white;
        _unselected2.color = Color.white;
    }
}
