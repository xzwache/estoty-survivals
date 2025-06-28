using System;

namespace Code.Gameplay.Experience.Services
{
    public class ExperienceService : IExperienceService
    {
        public const float ExperiencePerLevel = 10;

        public float CurrentExperience { get; private set; }
        public int CurrentLevel { get; private set; } = 1;
        public float ExperienceToNextLevel => CurrentLevel * ExperiencePerLevel - CurrentExperience;

        public event Action<float> ExperienceChanged;
        public event Action<int> LevelUp;

        public void AddExperience(float amount)
        {
            if (amount < 0.0f) return;

            CurrentExperience += amount;
            ExperienceChanged?.Invoke(CurrentExperience);

            if (CurrentExperience >= 10f)
            {
                CurrentLevel++;
                CurrentExperience = 0.0f;
                LevelUp?.Invoke(CurrentLevel);
            }
        }
    }
}