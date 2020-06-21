using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMenu : HiddenUI
{
    public void OnShowInputMenu() {
        if (_visible) hideUI();
        else showUI();
    }
}
