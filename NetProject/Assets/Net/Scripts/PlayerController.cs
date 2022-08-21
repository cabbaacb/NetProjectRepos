using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private bool _player1;
    [SerializeField, Range(1f, 10f)] private float _moveSpeed = 3f;

    private float _maxSpeed = 3f;


    private Controls _controls;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();

        _controls = new Controls();
        _controls.Player1.Enable();
        _controls.Player2.Enable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var direction = _player1
            ? _controls.Player1.Movement.ReadValue<Vector2>()
            : _controls.Player2.Movement.ReadValue<Vector2>();

        if (direction.x == 0 && direction.y == 0) return;

        var velocity = _rigidbody.velocity;

        velocity += new Vector3(direction.y, 0f, direction.x) * _moveSpeed * Time.fixedDeltaTime;

        velocity.y = 0f;
        velocity = Vector3.ClampMagnitude(velocity, _maxSpeed);
    }


    private void OnDestroy()
    {
        _controls.Player1.Disable();
        _controls.Player2.Disable();
    }
}
