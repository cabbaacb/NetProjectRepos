using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Net.Managers
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private string _playerPrefabName;

        [SerializeField]
        private InputAction _quit;
        [SerializeField, Range(1f, 15f)]
        private float _randomInterval = 7f;

        // Start is called before the first frame update
        void Start()
        {
            _quit.Enable();
            _quit.performed += OnQuit;

            var pos = new Vector3(Random.Range(-_randomInterval, _randomInterval), 0f, Random.Range(-_randomInterval, _randomInterval));
            var GO = PhotonNetwork.Instantiate(_playerPrefabName + PhotonNetwork.NickName, new Vector3(), new Quaternion());
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        private void OnQuit(InputAction.CallbackContext obj)
        {
            PhotonNetwork.LeaveRoom();
#if UNITY_EDITOR
            print("open menu");
            //SceneManager.LoadScene(0);
            SceneManager.LoadScene(0);
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
        SceneManager.LoadScene(0);

#endif


        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
