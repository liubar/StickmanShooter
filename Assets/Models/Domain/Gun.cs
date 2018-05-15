using System.Collections;
using UnityEngine;

namespace Domain
{
    public class Gun : MonoBehaviour, IGun
    {
        private WaitForSeconds _reloadingTime = new WaitForSeconds(0.5f);
        private bool _isReloading;

        public IBulletGenerator BulletGenerator { get; set; }

        public Transform Position { get; set; }

        public WaitForSeconds ReloadingTime
        {
            get { return _reloadingTime; }
            set { _reloadingTime = value; }
        }

        /// <summary>
        ///     Reloading gun
        /// </summary>
        /// <returns>waiting time</returns>
        public IEnumerator Reloading()
        {
            _isReloading = true;
            yield return _reloadingTime;
            _isReloading = false;
        }

        /// <summary>
        ///     Shoot gun
        /// </summary>
        public void Soot()
        {
            if (_isReloading || !isActiveAndEnabled) return;

            var bullet = BulletGenerator.GetBullet();
            bullet.GameObject.transform.position = Position.position;
            bullet.GameObject.transform.rotation = Position.rotation;
            bullet.GameObject.SetActive(true);

            StartCoroutine(Reloading());
        }

        void OnEnable()
        {
            _isReloading = false;
        }
    }
}
