namespace Code.Gameplay.Projectiles.Services
{
    public interface IProjectileFactoryTypeUpdater
    {
        void SetType(ProjectileType type, int stackCount = 1);
    }
}
