using Code.Gameplay.Lifetime.Behaviours;
using Code.Gameplay.Movement.Behaviours;
using Code.Gameplay.Teams.Behaviours;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using UnityEngine;

namespace Code.Gameplay.Projectiles.Behaviours
{
    [RequireComponent(typeof(DamageAreaApplier), typeof(IMovementDirectionProvider))]
    public class BouncingProjectileBehaviour : MonoBehaviour
    {
        private float _bounceRange = 5f;
        private LayerMask _enemyLayerMask = 64;
        private int _maxTargetsToCheck = 20;

        private DamageAreaApplier _damageAreaApplier;
        private IMovementDirectionProvider _movementProvider;

        private Team _team;
        private Stats _stats;
        private int _bouncesCount;
        private Health _lastTargetHit;

        private Collider2D[] _nearbyColliders;

        private void Awake()
        {
            _damageAreaApplier = GetComponent<DamageAreaApplier>();
            _movementProvider = GetComponent<IMovementDirectionProvider>();
            _team = GetComponent<Team>();

            _stats = GetComponent<Stats>();
            _nearbyColliders = new Collider2D[_maxTargetsToCheck];
        }

        private void Start()
        {
            _bouncesCount = (int) _stats.GetStat(StatType.BounceCount);
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
            if (_bouncesCount <= 0)
            {
                return;
            }

            _lastTargetHit = targetHit;

            if (targetHit != null)
            {
                BounceToNearestEnemy(targetHit.transform.position);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void BounceToNearestEnemy(Vector3 hitPosition)
        {
            var colliderCount = Physics2D.OverlapCircleNonAlloc(hitPosition, _bounceRange, _nearbyColliders, _enemyLayerMask);

            if (colliderCount == 0)
            {
                return;
            }

            var closestTarget = GetClosestTarget(hitPosition, colliderCount);

            if (closestTarget != null && _movementProvider != null)
            {
                // Calculate new direction
                var newDirection = (closestTarget.position - transform.position).normalized;
                var newRotation = Quaternion.FromToRotation(Vector3.up, newDirection);
                transform.rotation = newRotation;
                _movementProvider.SetDirection(newDirection);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private Transform GetClosestTarget(Vector3 hitPosition,
                                        int colliderCount)
        {
            Transform closestTarget = null;
            var closestDistance = float.MaxValue;

            for (var i = 0; i < colliderCount; i++)
            {
                var collider = _nearbyColliders[i];

                // Safety check for null collider
                if (collider == null)
                {
                    continue;
                }

                if (!collider.TryGetComponent(out Health health) ||
                    !collider.TryGetComponent(out Team otherTeam))
                {
                    continue;
                }

                // Additional safety checks
                if (health == null || otherTeam == null)
                {
                    continue;
                }

                // Don't bounce to same team
                if (_team != null && otherTeam.Type == _team.Type)
                {
                    continue;
                }

                if (health == _lastTargetHit)
                {
                    continue;
                }

                var distance = Vector3.Distance(hitPosition, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = health.transform;
                }
            }

            return closestTarget;
        }
    }
}
