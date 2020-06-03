﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TrashCollector))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    private static Progress _progress;

    public Transform initialPos;

    [SerializeField]
    private uint _id = 0;
    [SerializeField]
    private bool _spotlight_active = false;
    [SerializeField]
    private float _waterGravityScale = 0.1f;
    [SerializeField]
    private float _airGravityScale = 1f;
    [SerializeField]
    private float _waterUpperLimit = -1.3f;
    [SerializeField]
    private float _airDrag = 0;
    [SerializeField]
    private float _waterDrag = 2f;

    private PlayerLight[] _playerLights;

    private bool _insideWater = false;
    private Rigidbody2D _rb;
    private TrashCollector _trashCollector;
    private PlayerMovement _playerMovement;
    private Oxygen _oxygen;
    private PlayerPhotos _playerPhotos;

    private List<Progress.Fish> _tempPhotos;

    public PlayerSpotlight _playerSpotlight;
    public FadingUI deadUI;
    public Vector3 initialVel;
    public float movementMultiplierWhenFull = 0.4f;
    private bool _inputInteract = false;

    public void Awake()
    {
        _progress = GetProgress();
        _playerLights = GetComponentsInChildren<PlayerLight>();
    }

    public void Start()
    {
        _tempPhotos = new List<Progress.Fish>();

        _rb = GetComponent<Rigidbody2D>();
        _trashCollector = GetComponent<TrashCollector>();
        _playerMovement = GetComponent<PlayerMovement>();
        _oxygen = GetComponent<Oxygen>();
        _playerPhotos = GetComponentInChildren<PlayerPhotos>();

        updateAllLevels();

        resetTransform();
    }

    public void Update() {
        updateInsideWater();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            _progress.upgradeOxygenLevel();
            _progress.upgradeSpeedLevel();
            _progress.upgradeSpotlightLevel();
            _progress.upgradeTrashLevel();
            updateAllLevels();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            _progress.DEBUG_nextTrashStoryLevel();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            _progress.DEBUG_photographAllFish();
        }
#endif
    }

    public bool isSpotlightActive() {
        return _spotlight_active;
    }

    public void setSpotlightActive(bool spotlight_active) {
        _spotlight_active = spotlight_active;
    }

    public uint getID() {
        return _id;
    }

    public void updateInsideWater()
    {
        if (_insideWater == transform.position.y < _waterUpperLimit) return;
        _insideWater = transform.position.y < _waterUpperLimit;

        if (_insideWater) {
            _rb.gravityScale = _waterGravityScale;
            _rb.drag = _waterDrag;
        } else {
            _rb.gravityScale = _airGravityScale;
            _rb.drag = _airDrag;

            updateProgress(false);
        }

    }

    public bool insideWater()
    {
        return _insideWater;
    }

    public void die() {
        deadUI.gameObject.SetActive(true);

        deactivateBehaviours();
        float deadTime = deadUI.activeTime - deadUI.fadeTime - 0.1f;
        Invoke("activateBehaviours", deadTime);
        Invoke("resetTransform", deadTime);
        updateProgress(true);

        // TELEMETRY
        EventTracker.GetInstance().RegisterDeathtEvent(
        _progress.photosMade(), _progress.getCollectedTrash(),
        _progress.getSpotlightLevel(), _progress.getSpeedLevel(),
        _progress.getOxygenLevel(), _progress.getTrashLevel(), 4);
    }

    public void updateProgress(bool dead) {
        if (dead) {
            foreach (Progress.Fish fish in _tempPhotos)
                _progress.removePhoto(fish);
        }
        else {
        //    _progress.addTrash(_trashCollector.getCurrentTrash());
            _progress.addCoins(_tempPhotos.Count * _progress.getMoneyPerPhoto());
        }

        _tempPhotos.Clear();
        //_trashCollector.clearTrash();
    }

    public void giveTrash()
    {
        _progress.addTrash(_trashCollector.getCurrentTrash());
        _trashCollector.clearTrash();
    }

    public int GetCurrentTrash() {
        return _trashCollector.getCurrentTrash();
    }

    public void resetTransform()
    {
        transform.position = initialPos.position;
        transform.rotation = initialPos.rotation;
        _rb.velocity = initialVel;

        updateInsideWater();
    }

    public void updateTrash() {
        if (_trashCollector.getCurrentTrash() > _trashCollector.maxTrash)
            _playerMovement.movementModifier = movementMultiplierWhenFull;
        else
            _playerMovement.movementModifier = 1;
    }

    public static Progress GetProgress()
    {
        if (_progress == null) _progress = new Progress();
        return _progress;
    }

    public static Progress ResetProgress() {
        _progress = new Progress();
        return _progress;
    }

    public void updateAllLevels()
    {
        _playerMovement.updateLevel();
        foreach (PlayerLight pLight in _playerLights)
            pLight.updateLevel();

        _trashCollector.updateLevel();
        _oxygen.updateLevel();
    }

    private void deactivateBehaviours()
    {
        _oxygen.enabled = false;
        _playerMovement.enabled = false;
        _playerPhotos.enabled = false;
        _rb.gravityScale = 0;
    }

    public void activateBehaviours()
    {
        _oxygen.enabled = true;
        _playerMovement.enabled = true;
        _playerPhotos.enabled = true;
        _rb.gravityScale = _airGravityScale;

        updateInsideWater();
    }

    public void photographFish(Progress.Fish fish) {
        _progress.photographFish(fish, false);
        _tempPhotos.Add(fish);

        // TELEMETRY
        EventTracker.GetInstance().RegisterPhotoEvent(fish);
    }

    public void SetInputInteract(bool interact) {
        _inputInteract = interact;
    }

    public bool GetInputInteract() {
        return _inputInteract;
    }
}
