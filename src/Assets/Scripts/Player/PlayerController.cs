using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement _pMovement;
    private PlayerPhotos _pPhotos;
    private PlayerSpotlight _pSpotlight;
    private Player _player;

    [SerializeField] private HiddenUI _hiddenUI;

    void Start()
    {
        // Get the necessary components
        _pMovement = GetComponent<PlayerMovement>();
        _pPhotos = GetComponentInChildren<PlayerPhotos>();
        _pSpotlight = GetComponentInChildren<PlayerSpotlight>();
        _player = GetComponent<Player>();
    }

    public void Move(Vector2 dir) {
        _pMovement.Move(dir);
    }

    public void OnMove(InputValue value) {
        Vector2 dir = value.Get<Vector2>();
        Move(dir);
    }

    public void Boost(bool pressed) {
        _pMovement.Boost(pressed);
    }

    public void OnBoost(InputValue value) {
        Boost(value.isPressed);
    }

    public void Photo() {
        _pPhotos.photographFish();
    }

    public void OnPhoto() {
        Photo();
    }

    public void Interact() {
        _player.SetInputInteract(true);
    }

    public void OnInteract() {
        Interact();
    }

    public void Spotlight() {
        _pSpotlight.TurnOnOff();
    }

    public void OnSpotlight() {
        Spotlight();
    }

    public void OnShowUI(InputValue value) {
        if (value.isPressed) _hiddenUI.showUI();
        else _hiddenUI.hideUI();
    }
}
