using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Abilities.Configs;
using Code.Gameplay.Characters.Heroes.Services;
using Code.Gameplay.Projectiles;
using Code.Gameplay.Projectiles.Services;
using Code.Gameplay.Teams.Behaviours;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using Code.Infrastructure.ConfigsManagement;
using UnityEngine;

namespace Code.Gameplay.Abilities.Services
{
    public class AbilityService : IAbilityService
    {
        private readonly IConfigsService _configsService;
        private readonly IHeroProvider _heroProvider;
        private readonly IProjectileFactoryTypeUpdater _factoryTypeUpdater;
        private readonly IOrbitingProjectileFactory _orbitingProjectileFactory;
        private readonly IDictionary<AbilityId, int> _abilityStacks = new Dictionary<AbilityId, int>();

        internal AbilityService(IConfigsService configsService,
                              IHeroProvider heroProvider,
                              IProjectileFactoryTypeUpdater factoryTypeUpdater,
                              IOrbitingProjectileFactory orbitingProjectileFactory)
        {
            _configsService = configsService;
            _heroProvider = heroProvider;
            _factoryTypeUpdater = factoryTypeUpdater;
            _orbitingProjectileFactory = orbitingProjectileFactory;
        }

        public IReadOnlyList<AbilityConfig> GetRandomAbilities(int count)
        {
            var allAbilities = _configsService.GetAllAbilityConfigs();
            var availableAbilities = allAbilities.Where(IsAbilityAvailable).ToList();

            var shuffled = availableAbilities.OrderBy(x => Random.value).ToList();
            return shuffled.Take(count).ToList();
        }

        public void ApplyAbility(AbilityId abilityId)
        {
            var config = _configsService.GetAbilityConfig(abilityId);
            if (!_abilityStacks.ContainsKey(abilityId))
            {
                _abilityStacks[abilityId] = 0;
            }

            _abilityStacks[config.Id]++;
            ApplyAbilityEffect(config);
        }

        public bool HasAbility(AbilityId abilityId)
        {
            return _abilityStacks.ContainsKey(abilityId) && _abilityStacks[abilityId] > 0;
        }

        public int GetAbilityStacks(AbilityId abilityId)
        {
            return _abilityStacks.TryGetValue(abilityId, out var stacks)
                       ? stacks
                       : 0;
        }

        private bool IsAbilityAvailable(AbilityConfig config)
        {
            if (!HasAbility(config.Id))
            {
                return true;
            }

            return config.StackType == AbilityStackType.Multiple;
        }

        private void ApplyAbilityEffect(AbilityConfig config)
        {
            if (config.StackType == AbilityStackType.Single && HasAbility(config.Id))
            {
                return;
            }

            var hero = _heroProvider.Hero;
            if (hero == null)
            {
                return;
            }

            var stats = hero.GetComponent<Stats>();

            switch (config.Id)
            {
                case AbilityId.HealthPotionsBoost:
                    break;
                case AbilityId.PiercingProjectiles:
                    var piercingStacks = GetAbilityStacks(AbilityId.PiercingProjectiles);
                    _factoryTypeUpdater.SetType(ProjectileType.Piercing, piercingStacks);
                    break;

                case AbilityId.BouncingProjectiles:
                    _factoryTypeUpdater.SetType(ProjectileType.Bouncing);
                    break;

                case AbilityId.OrbitingProjectiles:
                    var heroTeam = hero.GetComponent<Team>();
                    var heroDamage = stats.GetStat(StatType.Damage);
                    _orbitingProjectileFactory.Create(hero.transform, heroTeam.Type, heroDamage);
                    break;

                case AbilityId.AgilityUp:
                    var currentRotationSpeed = stats.GetStat(StatType.RotationSpeed);
                    stats.SetBaseStat(StatType.RotationSpeed, currentRotationSpeed + config.Value);
                    break;

                case AbilityId.HealthUp:
                    var currentMaxHealth = stats.GetStat(StatType.MaxHealth);
                    stats.SetBaseStat(StatType.MaxHealth, currentMaxHealth + config.Value);
                    break;

                case AbilityId.DamageUp:
                    var currentDamage = stats.GetStat(StatType.Damage);
                    stats.SetBaseStat(StatType.Damage, currentDamage + config.Value);
                    break;
            }
        }
    }
}
