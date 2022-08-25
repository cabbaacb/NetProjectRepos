using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Net
{
    public class PlayerController : MonoBehaviour, IPunObservable
    {
        [SerializeField] private Transform _target;
        [SerializeField] private ProjectileController _bulletPrefab;
        [Space]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CapsuleCollider _collider;

        private bool _firstPlayer;

        [SerializeField, Range(1f, 10f)] private float _moveSpeed = 3f;
        [SerializeField, Range(1f, 50f)] private float _health = 35f;
        [Space, SerializeField, Range(0.1f, 5f)] private float _attackDelay = 0.4f;
        [SerializeField, Range(0.1f, 1f)] private float _rotateDelay = 0.3f;
        [SerializeField] private PhotonView _photonView;

        public delegate void DeathHandler();
        public static event DeathHandler OnDeath;
        public float Health { get => _health; }
        public void SetHealth(float health) => _health = health;


        private List<float[]> _bullets;
        private Controls _controls;
        private SpawnPoint _bulletspawnPoint;

        private float _maxSpeed = 3f;


        public List<float[]> GetBulletsPool() => _bullets;
        public void SetBullets(List<float[]> bull)
        {
            _bullets = bull;
            //if (_photonView.IsMine) return;
            foreach(var bul in bull)
            {
                Vector3 pos = new Vector3(bul[0], bul[1], bul[2]);
                Instantiate(_bulletPrefab, pos, _target.rotation);
            }
            

        }

        // Start is called before the first frame update
        void Start()
        {
            _bullets = new List<float[]>();
            _collider = GetComponent<CapsuleCollider>();
            _rigidbody = GetComponent<Rigidbody>();

            _controls = new Controls();
            _firstPlayer = name.Contains("1");


            _bulletspawnPoint = GetComponentInChildren<SpawnPoint>();

            FindObjectOfType<Managers.GameManager>().AddPlayer(this);

            /*
            if(_firstPlayer)
            _controls.Player1.Enable();
            else
            _controls.Player2.Enable();


            StartCoroutine(Fire());
            StartCoroutine(Focus());
            */

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!_photonView.IsMine) return;


            var directionHor = _controls.Player1.Horizontal.ReadValue<float>();
            var directionVer = _controls.Player1.Vertical.ReadValue<float>();

            Vector2 direction = new Vector2(directionHor, directionVer);

            if (direction.x == 0 && direction.y == 0) return;

            var velocity = _rigidbody.velocity;

            velocity += new Vector3(direction.y, 0f, direction.x) * _moveSpeed * Time.fixedDeltaTime;

            velocity.y = 0f;
            velocity = Vector3.ClampMagnitude(velocity, _maxSpeed);
            _rigidbody.velocity = velocity;


            /*
            var directionHor = _firstPlayer
                ? _controls.Player1.Horizontal.ReadValue<float>()

                : _controls.Player2.Horizontal.ReadValue<float>();
            var directionVer = _firstPlayer
                ? _controls.Player1.Vertical.ReadValue<float>()

                : _controls.Player2.Vertical.ReadValue<float>();

            Vector2 direction = new Vector2(directionHor, directionVer);

            if (direction.x == 0 && direction.y == 0) return;

            var velocity = _rigidbody.velocity;

            velocity += new Vector3(direction.y, 0f, direction.x) * _moveSpeed * Time.fixedDeltaTime;

            velocity.y = 0f;
            velocity = Vector3.ClampMagnitude(velocity, _maxSpeed);
            _rigidbody.velocity = velocity;
            */

        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Wall>() != null)
                Death();

            var bullet = other.GetComponent<ProjectileController>();

            if (bullet == null) return;
            if (bullet.Parent == name) return;

            GetDamage(bullet.Damage);
            Destroy(other.gameObject);

            if (_health <= 0) Death();

        }

        private void OnDisable()
        {

            _controls.Player1.Disable();

            /*
            if (_firstPlayer)
                _controls.Player1.Disable();
            else
                _controls.Player2.Disable();
            */
        }


        private IEnumerator Fire()
        {
            while(true)
            {
                var bullet = Instantiate(_bulletPrefab, _bulletspawnPoint.transform.position, transform.rotation, transform);
                bullet.Parent = name;
                _bullets.Add(new float[]{ bullet.transform.position.x, bullet.transform.position.y, bullet.transform.position.z});
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


        private void Death()
        {
            OnDeath?.Invoke();

            Time.timeScale = 0f;
        }
        public void GetDamage(float damage) => _health -= damage;


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.IsWriting)
            {
                stream.SendNext(PlayerData.Create(this));
            }
            else
            {
                ((PlayerData)stream.ReceiveNext()).Set(this);
            }

        }

        public void SetTarget(Transform target)
        {
            _target = target;

            if (!_photonView.IsMine) return;

            _controls.Player1.Enable();
            StartCoroutine(Fire());
            StartCoroutine(Focus());


        }
    }
}
