using Code.Gameplay.Characters.Enemies.Behaviours;
using Code.Gameplay.DifficultyScaling.Services;
using Code.Gameplay.Identification.Behaviours;
using Code.Gameplay.Lifetime.Behaviours;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using Code.Infrastructure.ConfigsManagement;
using Code.Infrastructure.Identification;
using Code.Infrastructure.Instantiation;
using UnityEngine;

namespace Code.Gameplay.Characters.Enemies.Services
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly IConfigsService _configsService;
        private readonly IInstantiateService _instantiateService;
        private readonly IIdentifierService _identifiers;
        private readonly IDifficultyService _difficultyService;

        public EnemyFactory(
            IConfigsService configsService,
            IInstantiateService instantiateService,
            IIdentifierService identifiers,
            IDifficultyService difficultyService)
        {
            _configsService = configsService;
            _instantiateService = instantiateService;
            _identifiers = identifiers;
            _difficultyService = difficultyService;
        }

        public Enemy CreateEnemy(EnemyId id, Vector3 at, Quaternion rotation)
        {
            var enemyConfig = _configsService.GetEnemyConfig(id);
            var enemy = _instantiateService.InstantiatePrefabForComponent(enemyConfig.Prefab, at, rotation);

            enemy.GetComponent<Id>()
                .Setup(_identifiers.Next());

            var scaledHealth = enemyConfig.Health * _difficultyService.CurrentHealthMultiplier;
            var scaledDamage = enemyConfig.Damage * _difficultyService.CurrentDamageMultiplier;

            enemy.GetComponent<Stats>()
                .SetBaseStat(StatType.MaxHealth, scaledHealth)
                .SetBaseStat(StatType.MovementSpeed, enemyConfig.MovementSpeed)
                .SetBaseStat(StatType.Damage, scaledDamage);

            enemy.GetComponent<Health>()
                .Setup(scaledHealth, scaledHealth);

            return enemy;
        }
    }
}