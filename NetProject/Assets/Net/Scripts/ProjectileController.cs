using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Net
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField, Range(1f, 10f)] private float _moveSpeed = 3f;
        [SerializeField, Range(1f, 10f)] private float _damage = 1f;
        [SerializeField, Range(1f, 15f)] private float _lifetime = 7f;


        
        private void OnEnable()
        {
            StartCoroutine(OnDie());
        }


        public float Damage => _damage;

        public string Parent { get; set; }

        // Update is called once per frame
        void Update()
        {
            transform.position += transform.forward * _moveSpeed * Time.deltaTime;
        }

        private IEnumerator OnDie()
        {
            yield return new WaitForSeconds(_lifetime);
            Destroy(gameObject);
        }
    }
}