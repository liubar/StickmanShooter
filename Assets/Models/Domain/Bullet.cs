using System.Collections;
using UnityEngine;

namespace Domain
{
    public class Bullet : MonoBehaviour, IBullet
    {
        private readonly int _speed = 10;
        private readonly int _damage = 100;

        public int Id { get; set; }

        public int Speed
        {
            get { return _speed; }
        }

        public int Damage
        {
            get { return _damage; }
        }

        public GameObject GameObject
        {
            get { return gameObject; }
        }

        public void Disable()
        {
            StopAllCoroutines();
            this.gameObject.SetActive(false);
        }

        /// <summary>
        ///     Move bullet
        /// </summary>
        void Update()
        {
            transform.Translate(Vector2.up * Time.deltaTime * _speed);
        }

        /// <summary>
        ///     autodisable bullet after timer
        /// </summary>
        /// <returns></returns>
        IEnumerator Timer()
        {
            yield return new WaitForSeconds(1.2f);
            Disable();
        }

        void OnEnable()
        {
            StartCoroutine(Timer());
        }
    }
}

