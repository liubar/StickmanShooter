using System;
using UnityEngine;

namespace App
{
    public class FireController : MonoBehaviour, IFireController
    {
        [SerializeField]
        string axisName = "Fire1";
        public event Action OnFire = () => { };

        public void Fire()
        {
            OnFire();
        }

        void Update()
        {
            if(Input.GetAxis(axisName) > 0) Fire();
        }
    }
}
