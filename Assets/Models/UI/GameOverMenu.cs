using UnityEngine;

namespace UI
{
    public class GameOverMenu : MonoBehaviour
    {
        private MainMenu _mainMenu;
        
        void Start()
        {
            _mainMenu = FindObjectOfType<MainMenu>();
            gameObject.SetActive(false);
        }

        /// <summary>
        ///     Start main menu
        /// </summary>
        public void RestartGame()
        {
            _mainMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        ///     Exit the game
        /// </summary>
        public void Exit()
        {
            Application.Quit();
        }
        
        /// <summary>
        ///     Hide controls
        /// </summary>
        void OnEnable()
        {
            if(_mainMenu != null) _mainMenu.HideControls();
        }
    }
}
