using Code.Gameplay.Teams;
using UnityEngine;

namespace Code.Gameplay.Projectiles.Services
{
    public interface IOrbitingProjectileFactory
    {
        void Create(Transform player, TeamType teamType, float damage);
        void DestroyOrbitingProjectiles();
        bool HasOrbitingProjectiles { get; }
    }
} 