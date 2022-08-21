using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Net
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private ProjectileController _bulletPrefab;
        [Space]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CapsuleCollider _collider;
        [SerializeField] private bool _player1;
        [SerializeField, Range(1f, 10f)] private float _moveSpeed = 3f;
        [SerializeField, Range(1f, 50f)] private float _health = 5f;
        [Space, SerializeField, Range(0.1f, 5f)] private float _attackDelay = 0.4f;
        [SerializeField, Range(0.1f, 1f)] private float _rotateDelay = 0.3f;

        public float Health { get => _health; } 

        private Controls _controls;
        private SpawnPoint _bulletspawnPoint;

        private float _maxSpeed = 3f;

        // Start is called before the first frame update
        void Start()
        {
            _collider = GetComponent<CapsuleCollider>();
            _rigidbody = GetComponent<Rigidbody>();

            _controls = new Controls();
            if(_player1)
            _controls.Player1.Enable();
            else
            _controls.Player2.Enable();

            _bulletspawnPoint = GetComponentInChildren<SpawnPoint>();

            StartCoroutine(Fire());
            StartCoroutine(Focus());

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            var directionHor = _player1
                ? _controls.Player1.Horizontal.ReadValue<float>()

                : _controls.Player2.Horizontal.ReadValue<float>();
            var directionVer = _player1
                ? _controls.Player1.Vertical.ReadValue<float>()

                : _controls.Player2.Vertical.ReadValue<float>();

            Vector2 direction = new Vector2(directionHor, directionVer);

            if (direction.x == 0 && direction.y == 0) return;

            var velocity = _rigidbody.velocity;

            velocity += new Vector3(direction.y, 0f, direction.x) * _moveSpeed * Time.fixedDeltaTime;

            velocity.y = 0f;
            velocity = Vector3.ClampMagnitude(velocity, _maxSpeed);
            _rigidbody.velocity = velocity;


        }


        private void OnTriggerEnter(Collider other)
        {
            var bullet = other.GetComponent<ProjectileController>();

            if (bullet == null) return;
            if (bullet.Parent == name) return;

            GetDamage(bullet.Damage);
            Destroy(other.gameObject);

            if (_health <= 0) Debug.Log("YOU'RE DEAD");

        }

        private void OnDestroy()
        {
            _controls.Player1.Disable();
            _controls.Player2.Disable();
        }

        private IEnumerator Fire()
        {
            while(true)
            {
                var bullet = Instantiate(_bulletPrefab, _bulletspawnPoint.transform.position, transform.rotation);
                bullet.Parent = name;
                yield return new WaitForSeconds(_attackDelay);

            }

        }

        private IEnumerator Focus()
        {
            while(true)
            {
                transform.LookAt(_target);
                transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
                yield return new WaitForSeconds(_rotateDelay);
            }
        }


        public void GetDamage(float damage) => _health -= damage;
    }
}
