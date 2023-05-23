using Leopotam.EcsLite;
using System.Linq;
using UnityEngine;
using ExtensionMethods;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

namespace TestGame.ECS
{
    public sealed class GameInitSystem : IEcsInitSystem
    {
        private EcsWorld _world;

        private EcsPool<PlayerComponent> _poolPlayer;
        private EcsPool<EnemyComponent> _poolEnemy;
        private EcsPool<MovableComponent> _poolMovable;
        private EcsPool<InputEventComponent> _poolInputEvent;
        private EcsPool<FollowComponent> _poolFollow;
        private EcsPool<ShootComponent> _poolShoot;
        private EcsPool<HealthComponent> _poolHealth;

        public async void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _poolPlayer = _world.GetPool<PlayerComponent>();
            _poolEnemy = _world.GetPool<EnemyComponent>();
            _poolMovable = _world.GetPool<MovableComponent>();
            _poolInputEvent = _world.GetPool<InputEventComponent>();
            _poolFollow = _world.GetPool<FollowComponent>();
            _poolShoot = _world.GetPool<ShootComponent>();
            _poolHealth = _world.GetPool<HealthComponent>();

            await CreateLocationObjects();
        }

        private async Task CreateLocationObjects()
        {
            var playerConfig = EntryPoint.PlayerConfig;
            var playerHandler = playerConfig.prefabRef.LoadAssetAsync();
            var playerPref = await playerHandler.Task;

            var enemyConfig = EntryPoint.EnemyConfig;
            var enemyHandler = enemyConfig.prefabRef.LoadAssetAsync();
            var enemyPref = await enemyHandler.Task;

            var playerTarget = SpawnPlayer(playerPref);

            float radius = EntryPoint.EnemyConfig.spawnRadius;
            
            for (int i = 0; i < EntryPoint.EnemyConfig.maxCountInScene; i++)
            {
                var pos = radius.RandomOnCircle();
                SpawnEnemy(enemyPref, pos, playerTarget);
            }

            Addressables.Release(playerHandler);
            Addressables.Release(enemyHandler);
        }

        private Transform SpawnPlayer(GameObject pref)
        {
            var player = _world.NewEntity();

            ref PlayerComponent playerComponent = ref _poolPlayer.Add(player);
            ref MovableComponent playerMoovableComponent = ref _poolMovable.Add(player);
            ref InputEventComponent playerInputEventComponent = ref _poolInputEvent.Add(player);
            ref ShootComponent shootComponent = ref _poolShoot.Add(player);
            ref HealthComponent healthComponent = ref _poolHealth.Add(player);

            var playerConfig = EntryPoint.PlayerConfig;
            var spawnedPlayer = GameObject.Instantiate(pref);

            playerComponent.transform = spawnedPlayer.transform;
            playerComponent.animator = spawnedPlayer.GetComponent<Animator>();

            healthComponent.health = playerConfig.health;
            healthComponent.respawnHealth = playerConfig.health;
            healthComponent.armor = 1.0f;
            healthComponent.respawnTime = 1.0f;

            playerMoovableComponent.transform = spawnedPlayer.transform;
            playerMoovableComponent.moveSpeed = playerConfig.speed;
            
            shootComponent.damage = playerConfig.damage;
            shootComponent.shootRate = playerConfig.shootRate;
            shootComponent.aimTexture = playerConfig.aimTexture;
            shootComponent.passedTime = float.MaxValue;

            return spawnedPlayer.transform;
        }


        private void SpawnEnemy(GameObject pref, Vector3 position, Transform playerTarget)
        {
            var enemy = _world.NewEntity();

            ref EnemyComponent enemyComponent = ref _poolEnemy.Add(enemy);
            ref MovableComponent enemyMoovableComponent = ref _poolMovable.Add(enemy);
            ref FollowComponent enemyFollowComponent = ref _poolFollow.Add(enemy);
            ref HealthComponent enemyHealthComponent = ref _poolHealth.Add(enemy);

            var enemyConfig = EntryPoint.EnemyConfig;
            var enemyTypeConfig = EntryPoint.EnemyConfig.enemyTypes[Random.Range(0, EntryPoint.EnemyConfig.enemyTypes.Count())];

            var spawnedEnemy = GameObject.Instantiate(pref, position, Quaternion.identity);
            spawnedEnemy.transform.GetComponent<SpriteRenderer>().sprite = enemyTypeConfig.sprite;

            enemyComponent.transform = spawnedEnemy.transform;  // Ссылка, необходима для расчета попадания без использования физики
            enemyComponent.damage = enemyTypeConfig.damage;
            enemyComponent.animator = spawnedEnemy.GetComponent<Animator>();

            enemyHealthComponent.health = enemyTypeConfig.health;
            enemyHealthComponent.respawnHealth = enemyTypeConfig.health;
            enemyHealthComponent.armor = enemyTypeConfig.armor;
            enemyHealthComponent.respawnTime = enemyConfig.respawnTime;

            enemyMoovableComponent.transform = spawnedEnemy.transform;
            enemyMoovableComponent.moveSpeed = enemyTypeConfig.speed + Random.Range(-0.05f, 0.2f);
            
            enemyFollowComponent.target = playerTarget;
        }

    }
}