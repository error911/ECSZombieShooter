using ExtensionMethods;
using Leopotam.EcsLite;
using UnityEngine;

namespace TestGame.ECS
{
    public sealed class EnemySystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<HealthComponent> _healthPool;
        private EcsPool<EnemyComponent> _enemyPool;

        private ScoreWidget _scoreWidget;

        public async void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world
                .Filter<EnemyComponent>()
                .Inc<HealthComponent>()
                .End();

            _healthPool = _world.GetPool<HealthComponent>();
            _enemyPool = _world.GetPool<EnemyComponent>();

            _scoreWidget = await EntryPoint.UI
                .ScoreWidgetProvider
                .Load();
        }

        public void Destroy(IEcsSystems systems)
        {
            EntryPoint.UI.ScoreWidgetProvider.Unload();
        }

        public void Run(IEcsSystems systems)
        {
            float hp = 0;
            int count = 0;
            foreach (int entity in _filter)
            {
                ref EnemyComponent enemyComponent = ref _enemyPool.Get(entity);
                ref HealthComponent healthComponent = ref _healthPool.Get(entity);

                if (healthComponent.health <= 0)
                {
                    if (healthComponent.respawnTimeCounter == 0) _scoreWidget?.SetKill();
                    enemyComponent.transform.gameObject.SetActive(false);
                    healthComponent.respawnTimeCounter += Time.deltaTime;

                    if (healthComponent.respawnTimeCounter > healthComponent.respawnTime)
                        RespawnEnemy(entity);
                }
                else
                {
                    count++;
                    hp += healthComponent.health;
                }
                
            }
            _scoreWidget?.SetAllHP(hp);
            _scoreWidget?.SetCount(count);
        }

        private void RespawnEnemy(int entity)
        {
            ref EnemyComponent enemyComponent = ref _enemyPool.Get(entity);
            ref HealthComponent healthComponent = ref _healthPool.Get(entity);

            float radius = EntryPoint.EnemyConfig.spawnRadius;
            enemyComponent.transform.position = radius.RandomOnCircle();
            enemyComponent.transform.gameObject.SetActive(true);
            healthComponent.health = healthComponent.respawnHealth;
            healthComponent.respawnTimeCounter = 0;
        }

    }
}