using System.Collections.Generic;
using Code.Gameplay.Abilities;
using Code.Gameplay.Abilities.Configs;
using Code.Gameplay.Characters.Enemies;
using Code.Gameplay.Characters.Enemies.Configs;
using Code.Gameplay.Characters.Heroes.Configs;
using Code.Gameplay.DifficultyScaling;
using Code.Gameplay.PickUps;
using Code.Gameplay.PickUps.Configs;

namespace Code.Infrastructure.ConfigsManagement
{
	public interface IConfigsService
	{
		HeroConfig HeroConfig { get; }
		DifficultyConfig DifficultyConfig { get; }
		
		void Load();
		EnemyConfig GetEnemyConfig(EnemyId id);
		PickUpConfig GetPickUpConfig(PickUpId id);
		AbilityConfig GetAbilityConfig(AbilityId id);
		List<AbilityConfig> GetAllAbilityConfigs();
		DifficultyConfig GetDifficultyConfig();
	}
}