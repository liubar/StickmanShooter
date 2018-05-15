using UnityEngine;

namespace Domain
{
    public interface IBullet
    {
        int Id { get; set; }
        GameObject GameObject { get; }
        int Speed { get; }
        int Damage { get; }
        void Disable();
    }
}
