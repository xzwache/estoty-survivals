using Code.Gameplay.Lifetime.Behaviours;
using UnityEngine;

namespace Code.Gameplay.Projectiles.Behaviours
{
    /// <summary>
    /// Component that makes projectiles pierce through multiple enemies.
    /// Prevents projectile destruction until all pierces are exhausted.
    /// </summary>
    [RequireComponent(typeof(DamageAreaApplier))]
    public class PiercingProjectileBehavior : MonoBehaviour
    {
        [Header("Pierce Settings")]
        [SerializeField] private int _piercingCount = 1;
        
        private DamageAreaApplier _damageArea;
        private int _remainingPierces;

        private void Awake()
        {
            _damageArea = GetComponent<DamageAreaApplier>();
            _remainingPierces = _piercingCount;
        }

        private void OnEnable()
        {
            if (_damageArea != null)
                _damageArea.OnDamageApplied += OnDamageApplied;
        }

        private void OnDisable()
        {
            if (_damageArea != null)
                _damageArea.OnDamageApplied -= OnDamageApplied;
        }

        private void OnDamageApplied(Health targetHit)
        {
            if (_remainingPierces > 0)
            {
                _remainingPierces--;
                
                if (_remainingPierces <= 0)
                {
                    // No more pierces left, allow the projectile to be destroyed
                    // Other components (like DestroyOnDamageApplied) will handle the actual destruction
                }
            }
        }

        public void SetPiercingCount(int count)
        {
            _piercingCount = count;
            _remainingPierces = count;
        }

        public bool HasPiercesRemaining => _remainingPierces > 0;
    }
} 