using ExtensionMethods;
using Leopotam.EcsLite;

namespace TestGame.ECS
{
    public sealed class PlayerSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<HealthComponent> _healthPool;
        private EcsPool<PlayerComponent> _playerPool;

        private PlayerHPWidget _playerHPWidget;

        public async void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world
                .Filter<PlayerComponent>()
                .Inc<HealthComponent>()
                .End();

            _healthPool = _world.GetPool<HealthComponent>();
            _playerPool = _world.GetPool<PlayerComponent>();


            _playerHPWidget = await EntryPoint.UI
                .PlayerHPWidgetProvider
                .Load();
        }

        public void Destroy(IEcsSystems systems)
        {
            EntryPoint.UI.ScoreWidgetProvider.Unload();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref PlayerComponent enemyComponent = ref _playerPool.Get(entity);
                ref HealthComponent healthComponent = ref _healthPool.Get(entity);

                _playerHPWidget?.SetHP(healthComponent.health);

                if (healthComponent.health <= 0)
                {
                    RespawnPlayer(entity);
                }
            }
        }


        private void RespawnPlayer(int entity)
        {
            ref PlayerComponent enemyComponent = ref _playerPool.Get(entity);
            ref HealthComponent healthComponent = ref _healthPool.Get(entity);

            float radius = EntryPoint.EnemyConfig.spawnRadius;
            enemyComponent.transform.position = radius.RandomOnCircle();
            healthComponent.health = healthComponent.respawnHealth;
            healthComponent.respawnTimeCounter = 0;
        }

    }
}