﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour {
    public float rotationOffset;

    public float initialAcceleration;
    public float initialMaxSpeed;
    public float initialWaterRotationSpeed;
    public float airRotationSpeed;

    public float initialBoostCooldown = 5;
    public float initialBoostForce = 2500;

    private float currentAcceleration;
    private float currentMaxSpeed;
    private float rotationSpeed;
    private float currentRotationSpeed;
    private float currentBoostForce;
    private float currentBoostCooldown;

    [HideInInspector]
    public float movementModifier = 1;

    private Rigidbody2D _rb;
    private Player _player;

    private float _boostTimer = 0;

    private Vector2 _inputDir;
    private bool _inputBoost = false;

	void Start () {
        _rb = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
    }

    void Update() {
        Vector2 currentVel = _rb.velocity;
        Vector2 currentDir = _player.insideWater() ? Vector2.up : Vector2.down;

        if (_inputDir.x != 0 || _inputDir.y != 0) {
            currentDir = _inputDir;
            if (!_player.insideWater()) currentDir.y = -1;

            if (currentDir.magnitude < currentMaxSpeed && _player.insideWater()) {
                Vector2 force = _inputDir.normalized;

                _rb.AddForce( force * Time.deltaTime * currentAcceleration * movementModifier);
            }
        }

        float angle = Mathf.Atan2(currentDir.y, currentDir.x) * Mathf.Rad2Deg + rotationOffset;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        currentRotationSpeed = _player.insideWater() ? rotationSpeed : airRotationSpeed;
        currentRotationSpeed *= movementModifier;
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * currentRotationSpeed);


        // Boost
        if (_boostTimer > 0) _boostTimer -= Time.deltaTime;
        else if (_player.insideWater() && _inputBoost) {
            _boostTimer = currentBoostCooldown;
            _rb.AddForce(_rb.velocity * currentBoostForce * movementModifier);
        }
    }

    public void Move(Vector2 dir) {
        _inputDir = dir;
    }

    public void Boost(bool boost) {
        _inputBoost = boost;
    }

    public void updateLevel()
    {
        float multiplier = Player.GetProgress().getSpeedMultiplier();

        currentAcceleration = initialAcceleration * multiplier;
        currentMaxSpeed = initialMaxSpeed * multiplier;
        rotationSpeed = initialWaterRotationSpeed * multiplier;
        currentBoostForce = initialBoostForce * multiplier;
        currentBoostCooldown = initialBoostCooldown * multiplier;
    }

    public float getCurrentAcceleration() {
        return currentAcceleration;
    }
}
