using System.Collections;
using System.IO;
using System.Linq;
using Domain;
using Newtonsoft.Json;
using UnityEngine;

namespace App
{
    // Need to debug. need testing...
    public class NeuralNetworkAI
    {
        private const Bullet Bullet = default(Bullet);
        private WeightCollection _w;
        private bool _isLearning = false;
        private float _learningRate;

        public IEnumerator Learn(IPlayer player, IPlayer enemy, float learningRate = 0.08f)
        {
            LoadWeights();
            player.Health = enemy.Health = 1000;
            _isLearning = true;
            _learningRate = learningRate;

            yield return LearnCor(player);
        }

        /// <summary>
        ///     Get actual enemy bullet
        /// </summary>
        /// <returns>finding bullet</returns>
        private IBullet GetEnemyBullet(IPlayer player)
        {
            var plPos = player.GameObject.transform.position;
            var activeBullets = player.Gun.BulletGenerator.GetActiveBullets();

            if (activeBullets.Length == 0 || activeBullets.All(o => o.GameObject.transform.up != Vector3.down))
            {
                return null;
            }

            // take the nearest bullet and check its boundaries
            var enemyBullet = activeBullets
                .OrderBy(o => Vector2.Distance(o.GameObject.transform.position, plPos))
                .FirstOrDefault(o => o.GameObject.transform.up == Vector3.down
                                     && o.GameObject.transform.position.y > plPos.y);

            if (enemyBullet == default(Bullet))
            {
                return null;
            }

            return enemyBullet;
        }

        private IEnumerator LearnCor(IPlayer player)
        {
            while (true)
            {
                var enemyBullet = GetEnemyBullet(player);

                if (enemyBullet == Bullet)
                {
                    yield return new WaitForSeconds(0.1f);
                    continue;
                }

                var plPos = player.GameObject.transform.position;
                var bulPos = enemyBullet.GameObject.transform.position;

                // angle relative to the bullet
                Vector3 targetDirection = bulPos - plPos;
                var angleBetween = Vector3.Angle(player.GameObject.transform.up, targetDirection);
                var ang = angleBetween < 6 ? 1f : 0;

                // Fence positions
                var wallRPos = GameObject.Find("WallRight").transform.position;
                var wallLPos = GameObject.Find("WallLeft").transform.position;
                
                // distance to the walls
                var distLeftWall = Mathf.Abs(plPos.x - wallRPos.x);
                var distRightWall = Mathf.Abs(plPos.x - wallLPos.x);
                var isMoveRight = distRightWall > 1 ? 1 : 0;
                var isMoveLeft = distLeftWall > 1 ? 1 : 0;

                // the bullet is on the left
                var isLeftDirection = bulPos.x < plPos.x ? 1f : 0;

                // run layer 1
                TransferFunc(_w.weights1, _w.sumLayer1, ang, isMoveLeft, isMoveRight, isLeftDirection);

                float moveCoefficient = 0;
                if (_w.sumLayer1[0] < 0.4) moveCoefficient = -1;
                else if (_w.sumLayer1[0] > 0.6) moveCoefficient = 1;
                
                // move player
                (player.Controller.MoveController as AIMoveController).HorizontalInput = moveCoefficient;

                // waiting Coefficient
                yield return new WaitForSeconds(_w.sumLayer1[1]);

                // stop movement
                (player.Controller.MoveController as AIMoveController).HorizontalInput = 0;

                // run layer 2
                TransferFunc(_w.weights2, _w.sumLayer2, _w.sumLayer1[0], _w.sumLayer1[1]);

                yield return new WaitForSeconds(1f);

                if (player.Health == 1000)
                {
                    // expected true
                }
                else
                {
                    // expected false. Change weights
                    var error = _w.sumLayer2[0];
                    var weightsDelta = error * (error * (1 - error));

                    for (var i = 0; i < _w.weights2[0].Length; i++)
                    {
                        _w.weights2[0][i] = _w.weights2[0][i] - (_w.sumLayer2[0] * weightsDelta * _learningRate);
                    }

                    for (int i = 0; i < _w.weights1.Length; i++)
                    {
                        error = _w.weights2[0][i] * weightsDelta;
                        for (int j = 0; j < _w.weights1[i].Length; j++)
                        {
                            _w.weights1[i][j] = _w.weights1[i][j] - (_w.sumLayer1[i] * weightsDelta * _learningRate);
                        }
                    }

                    player.Health = 1000;
                }

                _w.countIterations++;
                // Save result
                if (_w.countIterations % 20 == 0) SaveWeights();

                yield return new WaitForSeconds(0.1f);
            }
        }

        /// <summary>
        ///     Save weights
        /// </summary>
        void SaveWeights()
        {
            var dataAsJson = JsonConvert.SerializeObject(_w);
            string filePath = Application.dataPath + "/StreamingAssets/weights.json";
            File.WriteAllText(filePath, dataAsJson);
        }

        /// <summary>
        ///     Load weights
        /// </summary>
        void LoadWeights()
        {
            string file1Path = Application.dataPath + "/StreamingAssets/weights.json";
            if (File.Exists(file1Path))
            {
                string dataAsJson = File.ReadAllText(file1Path);
                _w = JsonConvert.DeserializeObject<WeightCollection>(dataAsJson);
            }
            else
            {
                _w = new WeightCollection();
            }
        }

        private void TransferFunc(float[][] weights, float[] outputs, params float[] inputs)
        {
            for (var i = 0; i < weights.Length; i++)
            {
                float sum = 0;

                for (int j = 0; j < inputs.Length; j++)
                {
                    sum += inputs[j] * weights[i][j];
                }

                outputs[i] = Sigmoid(sum);
            }
        }

        private float Sigmoid(float x)
        {
            return 1 / (1 + Mathf.Exp(x * -1));
        }
    }
}

