using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement _pMovement;
    private PlayerPhotos _pPhotos;
    private Player _player;

    [Header("Saved Input")]
    [SerializeField] private bool _useSavedInput;
    [SerializeField] private bool _saveInput;
    [SerializeField] private string _savedInputFilename;

    private InputEventTrace _trace;
    private InputEventTrace.ReplayController _replayController;

    void Start()
    {
        // Get the necessary components
        _pMovement = GetComponent<PlayerMovement>();
        _pPhotos = GetComponentInChildren<PlayerPhotos>();
        _player = GetComponent<Player>();

        // Init the replay system
        if (_useSavedInput) {
            Debug.Log("Replay: Loading Input Trace from file '" + _savedInputFilename + "'");
            _trace = InputEventTrace.LoadFrom(_savedInputFilename);
            _replayController = _trace.Replay();
            _replayController.PlayAllEventsAccordingToTimestamps();
        }
        else if (_saveInput) {
            Debug.Log("Replay: Input will be saved to file '" + _savedInputFilename + "'");
            _trace = new InputEventTrace();
            _trace.Enable();
        }
    }

    private void OnDestroy() {
        if (!_saveInput || _useSavedInput)
            return;

        Debug.Log("Replay: Saving Input to file '" + _savedInputFilename + "'");
        _trace.WriteTo(_savedInputFilename);
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
}
