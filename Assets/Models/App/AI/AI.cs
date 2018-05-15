using System.Collections;
using System.Linq;
using Domain;
using UnityEngine;
using Random = System.Random;

namespace App
{
    public class AI
    {
        private AIMoveController _moveCtrl;
        private Random _rn = new Random();

        public IPlayer Player { get; set; }
        public IPlayer Enemy { get; set; }

        /// <summary>
        ///     Start player control
        /// </summary>
        /// <returns>WaitForSeconds</returns>
        public IEnumerator Start()
        {
            _moveCtrl = Player.Controller.MoveController as AIMoveController;

            while (true)
            {
                yield return Attak();
                yield return Defense();
            }
        }

        /// <summary>
        ///     Attak ai logic
        /// </summary>
        /// <returns>WaitForSeconds</returns>
        IEnumerator Attak()
        {
            for (int i = 0; i < _rn.Next(0, 5); i++)
            {
                var enPos = Enemy.GameObject.transform.position.x;
                var plPos = Player.GameObject.transform.position.x;

                var tookAim = Mathf.Abs(enPos - plPos) < 0.6f;
                if (tookAim) Player.Controller.FireController.Fire();


                if (enPos < plPos) _moveCtrl.HorizontalInput = 0.5f;
                else _moveCtrl.HorizontalInput = -0.5f;

                yield return new WaitForSeconds(0.2f);
            }
        }

        /// <summary>
        ///     Defense ai logic
        /// </summary>
        /// <returns>WaitForSeconds</returns>
        IEnumerator Defense()
        {
            for (int i = 0; i < _rn.Next(0, 5); i++)
            {
                var enemyBullet = GetEnemyBullet();

                if (enemyBullet == null)
                {
                    yield return null;
                    continue;
                }

                // wall position
                var wallRPos = GameObject.Find("WallRight").transform.position;
                var wallLPos = GameObject.Find("WallLeft").transform.position;

                var bulPos = enemyBullet.GameObject.transform.position;
                var plPos = Player.GameObject.transform.position;

                // angle relative to the bullet
                Vector3 targetDirection = bulPos - plPos;
                var angleBetween = Vector3.Angle(Player.GameObject.transform.up, targetDirection);
                var isDanger = angleBetween < 6;

                // the bullet is on the left
                var isLeftDirection = bulPos.x > plPos.x;

                // distance to the walls
                var distLeftWall = Mathf.Abs(plPos.x - wallRPos.x);
                var distRightWall = Mathf.Abs(plPos.x - wallLPos.x);

                var isMoveRight = distRightWall > 1;
                var isMoveLeft = distLeftWall > 1;

                if (isDanger)
                {
                    if (isLeftDirection)
                    {
                        if (isMoveLeft)
                            _moveCtrl.HorizontalInput = -1f;
                        else
                            _moveCtrl.HorizontalInput = 1f;
                    }
                    else
                    {
                        if (isMoveRight)
                            _moveCtrl.HorizontalInput = 1f;
                        else
                            _moveCtrl.HorizontalInput = -1f;
                    }
                }

                yield return new WaitForSeconds(0.1f);

                // stop movement
                _moveCtrl.HorizontalInput = 0;
            }
        }

        /// <summary>
        ///     Get actual enemy bullet
        /// </summary>
        /// <returns>finding bullet</returns>
        private IBullet GetEnemyBullet()
        {
            var activeBullets = Player.Gun.BulletGenerator.GetActiveBullets();

            if (activeBullets.Length == 0 || activeBullets.All(o => o.GameObject.transform.up == Vector3.down))
            {
                return null;
            }

            // take the nearest bullet and check its boundaries
            var enemyBullet = activeBullets
                .OrderBy(o => Vector2.Distance(o.GameObject.transform.position, Player.GameObject.transform.position))
                .FirstOrDefault(o => o.GameObject.transform.up == Vector3.up &&
                                     o.GameObject.transform.position.y < Player.GameObject.transform.position.y);

            if (enemyBullet == default(Bullet))
            {
                return null;
            }

            return enemyBullet;
        }
    }
}
