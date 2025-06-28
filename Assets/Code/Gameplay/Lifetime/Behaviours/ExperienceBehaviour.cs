using System;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Gameplay.Lifetime.Behaviours
{
    [RequireComponent(typeof(Stats))]
    public class ExperienceBehaviour : MonoBehaviour
    {
        [field: SerializeField]
        public float Experience { get; private set; }

        private Stats _stats;
        private float _experienceAmount;

        public event Action<float> OnExperienceGained;

        private void Awake()
        {
            _stats = GetComponent<Stats>();

            _stats.OnStatChanged += HandleStatChanged;
        }

        private void OnDestroy()
        {
            _stats.OnStatChanged -= HandleStatChanged;
        }

        public void GiveExperience(float amount)
        {
            OnExperienceGained?.Invoke(amount);
        }

        private void HandleStatChanged(StatType statType, float value)
        {
            if (statType == StatType.Experience)
            {
                Experience = value;
            }
        }
    }
}
