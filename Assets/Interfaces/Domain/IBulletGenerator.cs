namespace Domain
{
    public interface IBulletGenerator
    {
        IBullet GetBullet();
        IBullet[] GetActiveBullets();
        void Reset();
    }
}
