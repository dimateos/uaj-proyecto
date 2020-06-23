using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{

    public Button _selected;
    public Button _unselected1;
    public Button _unselected2;

    public void ChangeColour()
    {
        _selected.GetComponent<Image>().color = Color.red;
        _unselected1.GetComponent<Image>().color = Color.white;
        _unselected2.GetComponent<Image>().color = Color.white;
    }
}
