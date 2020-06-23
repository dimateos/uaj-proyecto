using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownBehaviour : MonoBehaviour
{
    public Dropdown _dropdown;

    // Start is called before the first frame update
    void Start()
    {
        PopulateList();
        InputTraceManager.GetInstance().SetFilename();
    }

    private void PopulateList()
    {
        List<string> names = InputTraceManager.GetInstance().GetFilenames();
        _dropdown.AddOptions(names);
    }
}
