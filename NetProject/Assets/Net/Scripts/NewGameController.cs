using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Net
{
    public class NewGameController : MonoBehaviour
    {
        [SerializeField] private Button _backtoMenuButton;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Text _winText;


        void Start()
        {
            _backtoMenuButton.onClick.AddListener(BackToMenu);
            _newGameButton.onClick.AddListener(StartNewGame);
        }

        private void OnEnable()
        {
            PlayerController.OnDeath += ShowMenu;
        }

        private void OnDisable()
        {
            PlayerController.OnDeath -= ShowMenu;
        }

        private void OnDestroy()
        {
            _backtoMenuButton.onClick.RemoveAllListeners();
            _newGameButton.onClick.RemoveAllListeners();
        }

        private void ShowMenu()
        {
            _winText.text = "Game Over!";
            _backtoMenuButton.gameObject.SetActive(true);
            _newGameButton.gameObject.SetActive(true);
        }

        private void BackToMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void StartNewGame()
        {


            print("New Game");
            _backtoMenuButton.gameObject.SetActive(false);
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            _newGameButton.gameObject.SetActive(false);
        }
    }
}
