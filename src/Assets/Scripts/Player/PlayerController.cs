using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement _pMovement;

    [Header("Saved Input")]
    [SerializeField] private bool _useSavedInput;
    [SerializeField] private string _savedInputFilename;

    // Start is called before the first frame update
    void Start()
    {
        _pMovement = GetComponent<PlayerMovement>();
    }

    public void OnMove(InputValue value) {
        Vector2 dir = value.Get<Vector2>();
        Debug.Log(dir);
    }
}
