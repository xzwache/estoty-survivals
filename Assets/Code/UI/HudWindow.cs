using Code.Gameplay.Characters.Enemies.Services;
using Code.Gameplay.Characters.Heroes.Services;
using Code.Gameplay.Experience.Services;
using Code.Infrastructure.UIManagement;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI
{
    public class HudWindow : WindowBase
    {
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Text _killedEnemiesText;
        [SerializeField] private Slider _experienceBar;
        [SerializeField] private Text _levelText;

        private IHeroProvider _heroProvider;
        private IEnemyDeathTracker _enemyDeathTracker;
        private IExperienceService _experienceService;

        public override bool IsUserCanClose => false;

        [Inject]
        private void Construct(IHeroProvider heroProvider,
            IEnemyDeathTracker enemyDeathTracker,
            IExperienceService experienceService)
        {
            _enemyDeathTracker = enemyDeathTracker;
            _heroProvider = heroProvider;
            _experienceService = experienceService;
        }

        protected override void OnUpdate()
        {
            UpdateHealthBar();
            UpdateKilledEnemiesText();
            UpdateExperienceBar();
            UpdateLevelText();
        }

        private void UpdateKilledEnemiesText()
        {
            _killedEnemiesText.text = _enemyDeathTracker.TotalKilledEnemies.ToString();
        }

        private void UpdateHealthBar()
        {
            if (_heroProvider.Hero != null)
            {
                _healthBar.value = _heroProvider.Health.CurrentHealth / _heroProvider.Health.MaxHealth;
            }
            else
            {
                _healthBar.value = 0;
            }
        }

        private void UpdateExperienceBar()
        {
            // Calculate experience progress within current level
            var experienceInCurrentLevel = _experienceService.CurrentExperience % ExperienceService.ExperiencePerLevel;
            var experienceProgress = experienceInCurrentLevel / ExperienceService.ExperiencePerLevel;
            _experienceBar.value = experienceProgress;
        }

        private void UpdateLevelText()
        {
            _levelText.text = $"Level {_experienceService.CurrentLevel}";
        }
    }
}