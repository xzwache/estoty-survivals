using System;

namespace Code.Gameplay.DifficultyScaling.Services
{
	public interface IDifficultyService
	{
		float CurrentHealthMultiplier { get; }
		float CurrentDamageMultiplier { get; }
		
		event Action<float, float> OnDifficultyChanged;
		
		void StartScaling();
		void StopScaling();
	}
} 