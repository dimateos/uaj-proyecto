using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SimplePlayer))]
public class SimpleMovement : MonoBehaviour
{
    public float rotationOffset;

    public float initialAcceleration;
    public float initialMaxSpeed;
    public float initialWaterRotationSpeed;
    public float airRotationSpeed;

    public float initialBoostCooldown = 5;
    public float initialBoostForce = 2500;
    public float multiplier = 1;

    private float currentAcceleration;
    private float currentMaxSpeed;
    private float rotationSpeed;
    private float currentRotationSpeed;
    private float currentBoostForce;
    private float currentBoostCooldown;

    [HideInInspector]
    public float movementModifier = 1;

    private Rigidbody2D _rb;
    private SimplePlayer _player;

    private float _boostTimer = 0;

    private Vector2 _inputDir;
    private bool _inputBoost = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GetComponent<SimplePlayer>();

        updateLevel();
        updateGravity();
    }

    void Update()
    {
        Vector2 currentDir = Vector2.up;

        if (_inputDir.x != 0 || _inputDir.y != 0)
        {
            currentDir = _inputDir;

            if (currentDir.magnitude < currentMaxSpeed)
            {
                Vector2 force = _inputDir.normalized;

                _rb.AddForce(force * Time.deltaTime * currentAcceleration * movementModifier);
            }
        }

        float angle = Mathf.Atan2(currentDir.y, currentDir.x) * Mathf.Rad2Deg + rotationOffset;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        currentRotationSpeed = rotationSpeed;
        currentRotationSpeed *= movementModifier;
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * currentRotationSpeed);


        // Boost
        if (_boostTimer > 0) _boostTimer -= Time.deltaTime;
        else if (_inputBoost)
        {
            _boostTimer = currentBoostCooldown;
            _rb.AddForce(_rb.velocity * currentBoostForce * movementModifier);
        }
    }

    public void Move(Vector2 dir)
    {
        _inputDir = dir;
    }

    public void Boost(bool boost)
    {
        _inputBoost = boost;
    }

    public void updateLevel()
    {
        currentAcceleration = initialAcceleration * multiplier;
        currentMaxSpeed = initialMaxSpeed * multiplier;
        rotationSpeed = initialWaterRotationSpeed * multiplier;
        currentBoostForce = initialBoostForce * multiplier;
        currentBoostCooldown = initialBoostCooldown;
    }

    private void updateGravity()
    {
        if(_rb != null)
            _rb.simulated = enabled;
    }

    public void OnEnable()
    {
        updateGravity();
    }

    public void OnDisable()
    {
        updateGravity();
    }
}
