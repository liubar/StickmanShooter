using UnityEngine;

namespace Domain
{
    public interface IGun
    {
        IBulletGenerator BulletGenerator { get; set; }
        Transform Position { get; set; }
        WaitForSeconds ReloadingTime { get; set; }
        void Soot();
    }
}