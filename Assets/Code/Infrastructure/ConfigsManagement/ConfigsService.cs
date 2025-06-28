using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Abilities;
using Code.Gameplay.Abilities.Configs;
using Code.Gameplay.Characters.Enemies;
using Code.Gameplay.Characters.Enemies.Configs;
using Code.Gameplay.Characters.Heroes.Configs;
using Code.Gameplay.DifficultyScaling;
using Code.Gameplay.PickUps;
using Code.Gameplay.PickUps.Configs;
using Code.Infrastructure.AssetManagement;

namespace Code.Infrastructure.ConfigsManagement
{
	public class ConfigsService : IConfigsService
	{
		private readonly IAssetsService _assets;

		private Dictionary<EnemyId, EnemyConfig> _enemiesById = new();
		private Dictionary<PickUpId, PickUpConfig> _pickupsById = new();
		private Dictionary<AbilityId, AbilityConfig> _abilitiesById = new();

		public HeroConfig HeroConfig { get; private set; }
		public DifficultyConfig DifficultyConfig { get; private set; }

		public ConfigsService(IAssetsService assets)
		{
			_assets = assets;
		}
		
		public void Load()
		{
			LoadHeroConfig();
			LoadEnemyConfigs();
			LoadPickUpConfigs();
			LoadAbilityConfigs();
			LoadDifficultyConfig();
		}

		private void LoadPickUpConfigs()
		{
			var pickUpConfigs = _assets.LoadAssetsFromResources<PickUpConfig>("Configs/PickUps");
			_pickupsById = pickUpConfigs.ToList().ToDictionary(x => x.Id, x => x);
		}

		private void LoadHeroConfig()
		{
			HeroConfig = _assets.LoadAssetFromResources<HeroConfig>("Configs/HeroConfig");
		}

		private void LoadEnemyConfigs()
		{
			var enemyConfigs = _assets.LoadAssetsFromResources<EnemyConfig>("Configs/Enemies");
			_enemiesById = enemyConfigs.ToList().ToDictionary(x => x.Id, x => x);
		}

		public EnemyConfig GetEnemyConfig(EnemyId id)
		{
			if (_enemiesById.TryGetValue(id, out EnemyConfig enemyConfig))
				return enemyConfig;

			throw new KeyNotFoundException($"Enemy config with id {id} not found");
		}
		
		public PickUpConfig GetPickUpConfig(PickUpId id)
		{
			if (_pickupsById.TryGetValue(id, out PickUpConfig pickUpConfig))
				return pickUpConfig;

			throw new KeyNotFoundException($"PickUp config with id {id} not found");
		}

		public AbilityConfig GetAbilityConfig(AbilityId id)
		{
			if (_abilitiesById.TryGetValue(id, out AbilityConfig abilityConfig))
				return abilityConfig;

			throw new KeyNotFoundException($"Ability config with id {id} not found");
		}

		public List<AbilityConfig> GetAllAbilityConfigs()
		{
			return _abilitiesById.Values.ToList();
		}

		public DifficultyConfig GetDifficultyConfig()
		{
			return DifficultyConfig;
		}

		private void LoadAbilityConfigs()
		{
			var abilityConfigs = _assets.LoadAssetsFromResources<AbilityConfig>("Configs/Abilities");
			_abilitiesById = abilityConfigs.ToList().ToDictionary(x => x.Id, x => x);
		}

		private void LoadDifficultyConfig()
		{
			DifficultyConfig = _assets.LoadAssetFromResources<DifficultyConfig>("Configs/DifficultyConfig");
		}
	}
}