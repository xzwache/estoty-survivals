using Code.Gameplay.Identification.Behaviours;
using Code.Gameplay.Lifetime.Behaviours;
using Code.Gameplay.Movement.Behaviours;
using Code.Gameplay.Projectiles.Behaviours;
using Code.Gameplay.Teams;
using Code.Gameplay.Teams.Behaviours;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using Code.Gameplay.Vision.Behaviours;
using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.Identification;
using Code.Infrastructure.Instantiation;
using UnityEngine;

namespace Code.Gameplay.Projectiles.Services
{
    public class ProjectileFactory : IProjectileFactory, IProjectileFactoryTypeUpdater
    {
        private readonly IInstantiateService _instantiateService;
        private readonly IIdentifierService _identifiers;
        private readonly IAssetsService _assetsService;
        private Projectile _prefab;

        private ProjectileType _projectileType = ProjectileType.Regular;
        private int _stackCount = 0;

        public ProjectileFactory(
            IInstantiateService instantiateService,
            IIdentifierService identifiers,
            IAssetsService assetsService)
        {
            _instantiateService = instantiateService;
            _identifiers = identifiers;
            _assetsService = assetsService;
        }

        public void SetType(ProjectileType projectileType, int stackCount = 1)
        {
            _projectileType = projectileType;
            _stackCount = stackCount;
        }

        public Projectile Create(Vector3 at, Vector2 direction, TeamType teamType, float damage, float movementSpeed)
        {
            TryGetPrefab();
            var projectile = _instantiateService.InstantiatePrefabForComponent(_prefab, at, Quaternion.FromToRotation(Vector3.up, direction));

            projectile.GetComponent<Id>()
                .Setup(_identifiers.Next());

            projectile.GetComponent<Stats>()
                .SetBaseStat(StatType.MovementSpeed, movementSpeed)
                .SetBaseStat(StatType.Damage, damage);

            projectile.GetComponent<Team>()
                .Type = teamType;

            projectile.GetComponent<IMovementDirectionProvider>()
                .SetDirection(direction);

            TryApplyBehaviour(projectile.gameObject);

            return projectile;
        }

        private void TryApplyBehaviour(GameObject projectile)
        {
            switch (_projectileType)
            {
                case ProjectileType.Piercing:
                    var piercingComponent = projectile.AddComponent<PiercingProjectileBehaviour>();
                    piercingComponent.Initialize(_stackCount);
                    break;
                case ProjectileType.Bouncing:
                    projectile.AddComponent<VisionSight>();
                    projectile.AddComponent<BouncingProjectileBehaviour>();
                    projectile.GetComponent<Stats>()
                              .SetBaseStat(StatType.VisionRange, 20f)
                              .SetBaseStat(StatType.BounceCount, 5)
                              .SetBaseStat(StatType.VisionTargetsCount, 10);
                    break;
                case ProjectileType.Regular:
                default:
                    projectile.AddComponent<DestroyOnDamageApplied>();
                    break;
            }
        }

        private void TryGetPrefab()
        {
            if (_prefab == null)
            {
                _prefab = _assetsService.LoadAssetFromResources<Projectile>("Projectiles/Projectile");
            }
        }
    }
}