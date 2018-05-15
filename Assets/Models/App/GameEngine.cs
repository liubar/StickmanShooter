using Domain;
using UI;
using UnityEngine;

namespace App
{
    public class GameEngine : MonoBehaviour
    {
        public GameObject topController, botController;

        #region Fields

        private string _playerPref = "Prefabs/Player";
        private string _spawn1 = "SpawnP1";
        private string _spawn2 = "SpawnP2";
        private GameOverMenu _gameOverMenu;
        private IController _controller;
        private Vector3 _spawnP1, _spawnP2;
        private Player _player1, _player2;
        private BulletGenerator _bulletGen;
        private IController _aIController, _multController;
        private AI _ai;

        #endregion

        /// <summary>
        ///     Init ui controls
        /// </summary>
        void Awake()
        {
            _gameOverMenu = FindObjectOfType<GameOverMenu>();
        }

        /// <summary>
        ///     Init main controls
        /// </summary>
        void Start()
        {
            _spawnP1 = GameObject.Find(_spawn1).transform.position;
            _spawnP2 = GameObject.Find(_spawn2).transform.position;

            _bulletGen = new BulletGenerator();
            _aIController = new Controller(new AIMoveController(), new FireController());
            _multController = new Controller(
                topController.GetComponentInChildren<MoveController>(),
                topController.GetComponentInChildren<FireController>());
            _ai = new AI();
        }

        /// <summary>
        ///     Start game logic
        /// </summary>
        /// <param name="vsAI">is true = start vs AI. is false = start vs player</param>
        public void StartGame(bool vsAI = false)
        {
            Time.timeScale = 1;

            if (_player1 == null)
            {
                // Init players
                _player1 = Instantiate(Resources.Load(_playerPref, typeof(Player)), _spawnP1,
                    Quaternion.identity) as Player;
                _player1.Gun = _player1.GetComponentInChildren<Gun>();
                _player1.Gun.BulletGenerator = _bulletGen;

                _player1.Controller = new Controller(
                    botController.GetComponentInChildren<MoveController>(),
                    botController.GetComponentInChildren<FireController>());

                _player2 = Instantiate(Resources.Load(_playerPref, typeof(Player)), _spawnP2,
                    new Quaternion(0, 0, 1, 0)) as Player;
                _player2.Gun = _player2.GetComponentInChildren<Gun>();
                _player2.Gun.BulletGenerator = _bulletGen;
            }
            else
            {
                ResetPlayers();
            }

            // Init enemy player
            if (vsAI)
            {
                _player2.Controller = _aIController;
                _ai.Player = _player2;
                _ai.Enemy = _player1;
                StartCoroutine(_ai.Start());

                /* Need to debug. need testing...
                // Learn neural network
                _player1.Controller = new Controller(new AIMoveController(), new FireController());
                var nnAi = new NeuralNetworkAI();
                StartCoroutine(nnAi.Learn(_player1, _player2));
                */
            }
            else
            {
                _player2.Controller = _multController;
            }
        }

        /// <summary>
        ///     End game logic
        /// </summary>
        public void EndGame()
        {
            _gameOverMenu.gameObject.SetActive(true);
            Time.timeScale = 0;
            StopAllCoroutines();
        }

        /// <summary>
        ///     Reset players position
        /// </summary>
        private void ResetPlayers()
        {
            _player1.gameObject.transform.position = _spawnP1;
            _player2.gameObject.transform.position = _spawnP2;
            _player1.gameObject.SetActive(true);
            _player2.gameObject.SetActive(true);
            _player1.Gun.BulletGenerator.Reset();
        }
    }
}
