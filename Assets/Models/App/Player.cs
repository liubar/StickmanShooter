using Domain;
using UnityEngine;

namespace App
{
    public class Player : MonoBehaviour, IPlayer
    {
        public Transform gunPosition;

        private int _score;
        private int _speed;
        private IController _controller;
        private IGun _gun;

        Vector2 _movePos = new Vector2(0, 0);
        private int _health = 100;

        #region Properties

        public string Name { get; set; }

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public GameObject GameObject { get { return gameObject; } }

        public int Score
        {
            get { return _score; }
        }

        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public IController Controller
        {
            get { return _controller; }
            set
            {
                _controller = value;
                _controller.FireController.OnFire += () => _gun.Soot();
            }
        }

        public IGun Gun
        {
            get { return _gun; }
            set
            {
                _gun = value;
                _gun.Position = gunPosition;
            }
        }

        #endregion

        private void Start()
        {
            _speed = 10;
            // Change scale player 
            gameObject.transform.localScale = Vector3.one * Camera.main.orthographicSize / 6f;
        }

        /// <summary>
        ///     Check collision to bullet hit
        /// </summary>
        /// <param name="coll"></param>
        private void OnCollisionEnter2D(Collision2D coll)
        {
            var bullet = coll.gameObject.GetComponent<IBullet>();
            if (bullet != null)
            {
                Damage(bullet);
                bullet.Disable();
            }
        }

        /// <summary>
        ///     Causing injury to the player
        /// </summary>
        /// <param name="bullet">the bullet which hit</param>
        private void Damage(IBullet bullet)
        {
            Health -= bullet.Damage;

            if (Health <= 0)
            {
                gameObject.SetActive(false);
                GameObject.FindObjectOfType<GameEngine>().EndGame();
            }
        }

        /// <summary>
        ///     Update position player
        /// </summary>
        private void Update()
        {
            if (_controller == null) return;

            _movePos.x = _controller.MoveController.HorizontalInput;
            transform.Translate(_movePos * Time.deltaTime * _speed);
        }
    }
}