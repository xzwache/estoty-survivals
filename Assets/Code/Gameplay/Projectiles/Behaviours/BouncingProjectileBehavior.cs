using Code.Gameplay.Lifetime.Behaviours;
using Code.Gameplay.Teams.Behaviours;
using UnityEngine;

namespace Code.Gameplay.Projectiles.Behaviours
{
    /// <summary>
    /// Component that makes projectiles bounce to another enemy after hitting.
    /// Handles finding the nearest enemy and redirecting the projectile.
    /// </summary>
    [RequireComponent(typeof(DamageAreaApplier))]
    public class BouncingProjectileBehavior : MonoBehaviour
    {
        [Header("Bounce Settings")]
        [SerializeField] private float _bounceRange = 10f;
        [SerializeField] private LayerMask _enemyLayerMask = -1;
        
        private DamageAreaApplier _damageArea;
        private Team _projectileTeam;
        private bool _hasBounced = false;

        private void Awake()
        {
            _damageArea = GetComponent<DamageAreaApplier>();
            _projectileTeam = GetComponent<Team>();
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
            if (_hasBounced)
                return;

            var nearestEnemy = FindNearestEnemy(targetHit.transform.position);
            if (nearestEnemy != null)
            {
                BounceToTarget(nearestEnemy);
                _hasBounced = true;
            }
            else
            {
                // No valid bounce target found, projectile should be destroyed
                // The destruction will be handled by other components (like DestroyOnDamageApplied)
            }
        }

        private Transform FindNearestEnemy(Vector3 fromPosition)
        {
            var colliders = Physics2D.OverlapCircleAll(fromPosition, _bounceRange, _enemyLayerMask);
            Transform nearestEnemy = null;
            float nearestDistance = float.MaxValue;

            foreach (var collider in colliders)
            {
                // Skip if it doesn't have health (not a valid target)
                if (!collider.TryGetComponent<Health>(out _))
                    continue;

                // Skip if it's on the same team as the projectile
                if (collider.TryGetComponent<Team>(out var enemyTeam) && 
                    _projectileTeam != null && 
                    enemyTeam.Type == _projectileTeam.Type)
                    continue;

                var distance = Vector3.Distance(fromPosition, collider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = collider.transform;
                }
            }

            return nearestEnemy;
        }

        private void BounceToTarget(Transform target)
        {
            // Calculate direction to target
            var direction = (target.position - transform.position).normalized;
            
            // Update movement direction if the projectile has a movement direction provider
            if (TryGetComponent<Movement.Behaviours.IMovementDirectionProvider>(out var directionProvider))
            {
                directionProvider.SetDirection(direction);
            }

            // Update rotation to face the new direction
            transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        }
    }
} 