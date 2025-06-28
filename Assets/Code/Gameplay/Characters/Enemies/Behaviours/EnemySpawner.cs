using Code.Common.Extensions;
using Code.Gameplay.Cameras.Services;
using Code.Gameplay.Characters.Enemies.Services;
using Code.Gameplay.Characters.Heroes.Services;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Code.Gameplay.Characters.Enemies.Behaviours
{
    public class EnemySpawner : MonoBehaviour
    {
        private ICameraProvider _cameraProvider;
        private IHeroProvider _heroProvider;
        private IEnemyFactory _enemyFactory;

        private float _timer;

        private const float SpawnInterval = 1.6f;
        private const float SpawnDistanceGap = 0.5f;

        [Inject]
        private void Construct(ICameraProvider cameraProvider, IHeroProvider heroProvider, IEnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
            _heroProvider = heroProvider;
            _cameraProvider = cameraProvider;

            _timer = SpawnInterval * 0.9f;
        }

        private void Update()
        {
            Spawning();
        }

        private void Spawning()
        {
            var hero = _heroProvider.Hero;

            if (hero == null)
                return;

            _timer += Time.deltaTime;

            if (_timer >= SpawnInterval)
            {
                var randomSpawnPosition = RandomSpawnPosition(hero.transform.position);
                var enemyType = GetRandomEnemyType();
                _enemyFactory.CreateEnemy(enemyType, randomSpawnPosition, Quaternion.identity);
                _timer = 0;
            }
        }

        private EnemyId GetRandomEnemyType()
        {
            // Weighted spawn rates: Walker (70%), Skeleton (25%), Boss (5%)
            var randomValue = Random.value;

            if (randomValue < 0.70f)
                return EnemyId.Walker;
            if (randomValue < 0.95f)
                return EnemyId.Skeleton;
            return EnemyId.Boss;
        }

        private Vector2 RandomSpawnPosition(Vector2 heroWorldPosition)
        {
            var startWithHorizontal = Random.Range(0, 2) == 0;

            return startWithHorizontal
                ? HorizontalSpawnPosition(heroWorldPosition)
                : VerticalSpawnPosition(heroWorldPosition);
        }

        private Vector2 HorizontalSpawnPosition(Vector2 heroWorldPosition)
        {
            Vector2[] horizontalDirections = { Vector2.left, Vector2.right };
            var primaryDirection = horizontalDirections.PickRandom();

            var horizontalOffsetDistance = _cameraProvider.WorldScreenWidth / 2 + SpawnDistanceGap;
            var verticalRandomOffset = Random.Range(-_cameraProvider.WorldScreenHeight / 2,
                _cameraProvider.WorldScreenHeight / 2);

            return heroWorldPosition + primaryDirection * horizontalOffsetDistance + Vector2.up * verticalRandomOffset;
        }

        private Vector2 VerticalSpawnPosition(Vector2 heroWorldPosition)
        {
            Vector2[] verticalDirections = { Vector2.up, Vector2.down };
            var primaryDirection = verticalDirections.PickRandom();

            var verticalOffsetDistance = _cameraProvider.WorldScreenHeight / 2 + SpawnDistanceGap;
            var horizontalRandomOffset =
                Random.Range(-_cameraProvider.WorldScreenWidth / 2, _cameraProvider.WorldScreenWidth / 2);

            return heroWorldPosition + primaryDirection * verticalOffsetDistance +
                   Vector2.right * horizontalRandomOffset;
        }
    }
}