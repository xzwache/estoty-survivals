using System.Collections.Generic;
using Code.Gameplay.Abilities.Configs;
using Code.Gameplay.Abilities.Services;
using Code.Infrastructure.UIManagement;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI
{
    public class LevelUpWindow : WindowBase
    {
        [SerializeField] private AbilityCard[] _abilityCards;
        [SerializeField] private Text _levelText;

        private IAbilityService _abilityService;
        private IReadOnlyList<AbilityConfig> _currentAbilities;

        public override bool IsUserCanClose => false;

        [Inject]
        private void Construct(IAbilityService abilityService)
        {
            _abilityService = abilityService;
        }

        public void Setup(int newLevel)
        {
            const int abilitiesCount = 3;
            _levelText.text = $"Level {newLevel}!";
            _currentAbilities = _abilityService.GetRandomAbilities(abilitiesCount);

            for (var i = 0; i < _abilityCards.Length && i < _currentAbilities.Count; i++)
            {
                _abilityCards[i].Setup(_currentAbilities[i]);
                _abilityCards[i].OnSelected += OnAbilitySelected;
                _abilityCards[i].gameObject.SetActive(true);
            }

            for (var i = _currentAbilities.Count; i < _abilityCards.Length; i++)
            {
                _abilityCards[i].gameObject.SetActive(false);
            }

            Time.timeScale = 0f;
        }

        private void OnAbilitySelected(AbilityConfig selectedAbility)
        {
            for (var i = 0; i < _abilityCards.Length && i < _currentAbilities.Count; i++)
            {
                _abilityCards[i].OnSelected -= OnAbilitySelected;
            }

            _abilityService.ApplyAbility(selectedAbility.Id);
            Time.timeScale = 1f;
            CloseWindow();
        }

        protected override void OnClose()
        {
            Time.timeScale = 1f;
        }
    }
}