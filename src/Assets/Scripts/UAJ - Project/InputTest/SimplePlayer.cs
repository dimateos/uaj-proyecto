using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SimpleMovement))]
[RequireComponent(typeof(SimpleController))]
public class SimplePlayer : MonoBehaviour
{
    public Transform initialPos;

    [SerializeField]
    private float _waterGravityScale = 0.1f;
    [SerializeField]
    private float _waterDrag = 2f;

    private Rigidbody2D _rb;
    private SimpleMovement _playerMovement;

    public Vector3 initialVel;
    public bool _inputInteract = false;

    public void Start()
    {

        _rb = GetComponent<Rigidbody2D>();
        _playerMovement = GetComponent<SimpleMovement>();

        _rb.gravityScale = _waterGravityScale;
        _rb.drag = _waterDrag;

        resetTransform();
    }

    public void resetTransform()
    {
        transform.position = initialPos.position;
        transform.rotation = initialPos.rotation;
        _rb.velocity = initialVel;
    }

    private void deactivateBehaviours()
    {
        _playerMovement.enabled = false;
    }

    public void activateBehaviours()
    {
        _playerMovement.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Trash trash = collision.gameObject.GetComponent<Trash>();
        if (trash != null)
        {
            collision.gameObject.SetActive(false);
        }
    }
}
