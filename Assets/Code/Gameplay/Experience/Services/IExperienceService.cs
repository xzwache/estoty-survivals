using System;

namespace Code.Gameplay.Experience.Services
{
	public interface IExperienceService
	{
		float CurrentExperience { get; }
		int CurrentLevel { get; }
		float ExperienceToNextLevel { get; }
		
		event Action<float> ExperienceChanged;
		event Action<int> LevelUp;
		
		void AddExperience(float amount);
	}
} 