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

    private float currentAcceleration;
    private float currentMaxSpeed;
    private float rotationSpeed;
    private float currentRotationSpeed;

    [HideInInspector]
    public float movementModifier = 1;

    private Rigidbody2D _rb;
    private Player _player;

	void Start () {
        _rb = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
    }

    void Update() {
        Vector2 currentVel = _rb.velocity;
        Vector2 currentDir = _player.insideWater() ? Vector2.up : Vector2.down;

        float hMov = Input.GetAxis("Horizontal");
        float vMov = Input.GetAxis("Vertical");

        if (hMov != 0 || vMov != 0) {
            currentDir = new Vector2(hMov, vMov).normalized;
            if (!_player.insideWater()) currentDir.y = -1;

            if (currentDir.magnitude < currentMaxSpeed && _player.insideWater()) {
                _rb.AddForce(Vector2.right * hMov * Time.deltaTime * currentAcceleration * movementModifier);
                _rb.AddForce(Vector2.up * vMov * Time.deltaTime * currentAcceleration * movementModifier);
            }
        }

        float angle = Mathf.Atan2(currentDir.y, currentDir.x) * Mathf.Rad2Deg + rotationOffset;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        currentRotationSpeed = _player.insideWater() ? rotationSpeed : airRotationSpeed;
        currentRotationSpeed *= movementModifier;
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * currentRotationSpeed);
    }

    public void updateLevel()
    {
        float multiplier = _player.GetProgress().getSpeedMultiplier();

        currentAcceleration = initialAcceleration * multiplier;
        currentMaxSpeed = initialMaxSpeed * multiplier;
        rotationSpeed = initialWaterRotationSpeed * multiplier;
    }

    public float getCurrentAcceleration() {
        return currentAcceleration;
    }
}
