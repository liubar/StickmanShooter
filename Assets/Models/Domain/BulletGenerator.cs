using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Domain
{
    /// <summary>
    ///     Pool bullets 
    /// </summary>
    public class BulletGenerator : IBulletGenerator
    {
        private IList<Bullet> _bullets;
        private string _bulletPrefab = "Prefabs/Bullet";

        /// <summary>
        ///     Ctor
        /// </summary>
        public BulletGenerator()
        {
            _bullets = new List<Bullet>();
            AddBullet();
        }

        /// <summary>
        ///     Get active bullets
        /// </summary>
        /// <returns>all active bullets</returns>
        public IBullet[] GetActiveBullets()
        {
            return _bullets.Where(obj => obj.isActiveAndEnabled).ToArray();
        }

        /// <summary>
        ///     Deactivate all bullets
        /// </summary>
        public void Reset()
        {
            foreach (var activeBullet in GetActiveBullets())
            {
                activeBullet.Disable();
            }
        }

        /// <summary>
        ///     Get free bullet from pool
        /// </summary>
        /// <returns>free bullet</returns>
        public IBullet GetBullet()
        {
            if (_bullets.All(obj => obj.isActiveAndEnabled))
            {
                AddBullet();
            }

            return _bullets.First(obj => !obj.isActiveAndEnabled);
        }
        
        /// <summary>
        ///     Add new bullet in pool
        /// </summary>
        private void AddBullet()
        {
            var bullet = GameObject.Instantiate(Resources.Load(_bulletPrefab, typeof(Bullet))) as Bullet;
            bullet.gameObject.SetActive(false);
            bullet.Id = _bullets.Count;

            _bullets.Add(bullet);
        }
    }
}
