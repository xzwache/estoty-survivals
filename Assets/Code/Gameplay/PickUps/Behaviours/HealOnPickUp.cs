using Code.Gameplay.Abilities;
using Code.Gameplay.Abilities.Services;
using Code.Gameplay.Lifetime.Behaviours;
using UnityEngine;
using Zenject;

namespace Code.Gameplay.PickUps.Behaviours
{
    [RequireComponent(typeof(PickUp))]
    public class HealOnPickUp : MonoBehaviour
    {
        [SerializeField] private float _healAmount;

        private PickUp _pickUp;
        private IAbilityService _abilityService;

        [Inject]
        private void Construct(IAbilityService abilityService)
        {
            _abilityService = abilityService;
        }

        private void Awake()
        {
            _pickUp = GetComponent<PickUp>();
        }

        private void OnEnable()
        {
            _pickUp.OnPickUp += HandlePickup;
        }

        private void OnDisable()
        {
            _pickUp.OnPickUp -= HandlePickup;
        }

        private void HandlePickup(GameObject pickUpper)
        {
            if (pickUpper.TryGetComponent(out Health health))
            {
                var finalHealAmount = _healAmount;
                if (_abilityService.HasAbility(AbilityId.HealthPotionsBoost))
                {
                    finalHealAmount *= 2f;
                }

                health.Heal(finalHealAmount);
            }
        }
    }
}