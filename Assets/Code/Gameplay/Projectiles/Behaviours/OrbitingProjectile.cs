using System.Collections;
using Code.Gameplay.Lifetime.Behaviours;
using UnityEngine;

namespace Code.Gameplay.Projectiles.Behaviours
{
    public class OrbitingProjectile : MonoBehaviour
    {
        [Header("Orbit Settings")]
        [SerializeField]
        private float _orbitRadius = 2f;

        [SerializeField]
        private float _orbitDuration = 2f; // Time to complete one full orbit

        [Header("Disappear/Reappear Settings")]
        [SerializeField]
        private float _reappearDelay = 2f; // Time before projectile reappears after hitting enemy

        private Transform _player;
        private DamageAreaApplier _damageAreaApplier;
        private readonly bool _isActive = true;
        private bool _isVisible = true;
        private float _rotationSpeed;
        private float _currentAngle;

        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider;

        public void Initialize(Transform player, float angleOffset)
        {
            _player = player;
            _damageAreaApplier = GetComponent<DamageAreaApplier>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();

            _currentAngle = angleOffset;
            _rotationSpeed = 360f / _orbitDuration;

            UpdatePosition();
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

        private void Update()
        {
            if (!_isActive || _player == null)
            {
                return;
            }

            // Always continue orbiting, even when invisible
            _currentAngle += _rotationSpeed * Time.deltaTime;

            // Keep angle in 0-360 range
            if (_currentAngle >= 360f)
            {
                _currentAngle -= 360f;
            }

            // Update position relative to player's current position
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            // Calculate position relative to player's current position
            var offset = new Vector3(
                Mathf.Cos(_currentAngle * Mathf.Deg2Rad) * _orbitRadius,
                Mathf.Sin(_currentAngle * Mathf.Deg2Rad) * _orbitRadius,
                0
            );

            transform.position = _player.position + offset;
        }

        private void OnDamageApplied(Health targetHit)
        {
            if (!_isActive || !_isVisible)
            {
                return;
            }

            SetVisibility(false);
            StartCoroutine(ReappearAfterDelay());
        }

        private void SetVisibility(bool visible)
        {
            _isVisible = visible;

            if (_spriteRenderer != null)
            {
                _spriteRenderer.enabled = visible;
            }

            if (_collider != null)
            {
                _collider.enabled = visible;
            }
        }

        private IEnumerator ReappearAfterDelay()
        {
            yield return new WaitForSeconds(_reappearDelay);

            SetVisibility(true);
        }
    }
}
