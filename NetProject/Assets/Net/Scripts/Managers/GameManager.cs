using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Net.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private InputAction _quit;

        // Start is called before the first frame update
        void Start()
        {
            _quit.Enable();
        }

        private void OnEnable()
        {
            _quit.performed += OnQuit;
        }

        private void OnQuit(InputAction.CallbackContext obj)
        {
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
