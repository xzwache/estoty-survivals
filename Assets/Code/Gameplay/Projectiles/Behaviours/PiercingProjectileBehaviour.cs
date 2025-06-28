using Code.Gameplay.Lifetime.Behaviours;
using UnityEngine;

namespace Code.Gameplay.Projectiles.Behaviours
{
    [RequireComponent(typeof(DamageAreaApplier))]
    public class PiercingProjectileBehaviour : MonoBehaviour
    {
        [SerializeField] private int _maxPierces = 1;

        private DamageAreaApplier _damageAreaApplier;
        private int _currentPierces;

        public void Initialize(int maxPierces)
        {
            _maxPierces = maxPierces;
        }

        private void Awake()
        {
            _damageAreaApplier = GetComponent<DamageAreaApplier>();
        }

        private void OnEnable()
        {
            if (_damageAreaApplier != null)
            {
                _damageAreaApplier.OnDamageApplied += OnDamageApplied;
            }
        }

        private void OnDisable()
        {
            if (_damageAreaApplier != null)
            {
                _damageAreaApplier.OnDamageApplied -= OnDamageApplied;
            }
        }

        private void OnDamageApplied(Health targetHit)
        {
            _currentPierces++;
            if (_currentPierces > _maxPierces)
            {
                DestroyProjectile();
            }
        }

        private void DestroyProjectile()
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}