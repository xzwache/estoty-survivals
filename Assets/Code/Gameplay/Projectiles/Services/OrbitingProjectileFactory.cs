using System.Collections.Generic;
using Code.Gameplay.Identification.Behaviours;
using Code.Gameplay.Projectiles.Behaviours;
using Code.Gameplay.Teams;
using Code.Gameplay.Teams.Behaviours;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.Identification;
using Code.Infrastructure.Instantiation;
using UnityEngine;

namespace Code.Gameplay.Projectiles.Services
{
    public class OrbitingProjectileFactory : IOrbitingProjectileFactory
    {
        private readonly IInstantiateService _instantiateService;
        private readonly IIdentifierService _identifiers;
        private readonly IAssetsService _assetsService;

        private OrbitingProjectile _prefab;
        private readonly List<GameObject> _orbitingProjectiles = new();
        private const int ProjectileCount = 3;

        public bool HasOrbitingProjectiles
        {
            get
            {
                CleanupDestroyedProjectiles();
                return _orbitingProjectiles.Count > 0;
            }
        }

        public OrbitingProjectileFactory(
            IInstantiateService instantiateService,
            IIdentifierService identifiers,
            IAssetsService assetsService)
        {
            _instantiateService = instantiateService;
            _identifiers = identifiers;
            _assetsService = assetsService;
        }

        public void Create(Transform player, TeamType teamType, float damage)
        {
            if (HasOrbitingProjectiles)
                return;

            if (_prefab == null)
                _prefab = _assetsService.LoadAssetFromResources<OrbitingProjectile>("Projectiles/OrbitingProjectile");

            for (var i = 0; i < ProjectileCount; i++)
            {
                var angleOffset = i * 120f; // 120 degrees apart
                var projectile = CreateSingleOrbitingProjectile(player, teamType, damage, angleOffset);
                _orbitingProjectiles.Add(projectile.gameObject);
            }
        }

        public void DestroyOrbitingProjectiles()
        {
            foreach (var projectile in _orbitingProjectiles)
                if (projectile != null)
                    Object.Destroy(projectile);

            _orbitingProjectiles.Clear();
        }

        private void CleanupDestroyedProjectiles()
        {
            // Clean up any destroyed projectiles from the list
            for (var i = _orbitingProjectiles.Count - 1; i >= 0; i--)
                if (_orbitingProjectiles[i] == null)
                    _orbitingProjectiles.RemoveAt(i);
        }

        private OrbitingProjectile CreateSingleOrbitingProjectile(Transform player, TeamType teamType, float damage, float angleOffset)
        {
            var projectile = _instantiateService.InstantiatePrefabForComponent(_prefab, player.position, Quaternion.identity);

            // Setup basic components
            projectile.GetComponent<Id>().Setup(_identifiers.Next());

            projectile.GetComponent<Stats>()
                .SetBaseStat(StatType.MovementSpeed, 0f)
                .SetBaseStat(StatType.Damage, damage);

            projectile.GetComponent<Team>().Type = teamType;
            projectile.Initialize(player, angleOffset);

            return projectile;
        }
    }
}