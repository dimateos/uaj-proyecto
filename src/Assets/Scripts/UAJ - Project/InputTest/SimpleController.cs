using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class SimpleController : MonoBehaviour
{
    private SimpleMovement _pMovement;


    void Start()
    {
        // Get the necessary components
        _pMovement = GetComponent<SimpleMovement>();
    }

    public void Move(Vector2 dir)
    {
        _pMovement.Move(dir);
    }

    public void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        Move(dir);
    }

    public void Boost(bool pressed)
    {
        _pMovement.Boost(pressed);
    }

    public void OnBoost(InputValue value)
    {
        Boost(value.isPressed);
    }
}
