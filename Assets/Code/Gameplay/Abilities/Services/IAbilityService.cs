using System.Collections.Generic;
using Code.Gameplay.Abilities.Configs;

namespace Code.Gameplay.Abilities.Services
{
	public interface IAbilityService
	{
		IReadOnlyList<AbilityConfig> GetRandomAbilities(int count);
		void ApplyAbility(AbilityId abilityId);
		bool HasAbility(AbilityId abilityId);
		int GetAbilityStacks(AbilityId abilityId);
	}
} 