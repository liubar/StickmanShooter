using System;

namespace App
{
    public interface IFireController
    {
        event Action OnFire;
        void Fire();
    }
}
