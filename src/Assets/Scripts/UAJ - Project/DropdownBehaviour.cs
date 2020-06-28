using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownBehaviour : MonoBehaviour
{
    public Dropdown _dropdown;
    public Text _chosenLabel;
    private InputTraceManager _itm;

    // Start is called before the first frame update
    void Start()
    {
        _itm = InputTraceManager.GetInstance();
        PopulateList();
        _itm.SetFilename();
        _chosenLabel.text = _itm.GetFilename();
        if (_chosenLabel.text == "") _chosenLabel.text = "No file recorded yet...";
    }

    // Fills list with input trace filenames
    private void PopulateList()
    {
        List<string> names = _itm.GetFilenames();
        _dropdown.AddOptions(names);
    }
}
