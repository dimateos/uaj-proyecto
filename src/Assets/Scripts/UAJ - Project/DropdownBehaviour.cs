using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownBehaviour : MonoBehaviour
{
    public Dropdown _dropdown;
    public Text _chosenLabel;

    // Start is called before the first frame update
    void Start()
    {
        PopulateList();
        InputTraceManager.GetInstance().SetFilename();
        _chosenLabel.text = InputTraceManager.GetInstance().GetFilename();
        if (_chosenLabel.text == "") _chosenLabel.text = "No file recorded yet...";
    }

    private void PopulateList()
    {
        List<string> names = InputTraceManager.GetInstance().GetFilenames();
        _dropdown.AddOptions(names);
    }
}
