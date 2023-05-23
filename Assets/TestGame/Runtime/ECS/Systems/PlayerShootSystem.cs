using Leopotam.EcsLite;
using UnityEngine;

namespace TestGame.ECS
{
    public sealed class PlayerShootSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsFilter _filterMonsters;

        private EcsPool<InputEventComponent> _inputEventsPool;
        private EcsPool<ShootComponent> _shootPool;
        private EcsPool<PlayerComponent> _playerPool;
        private EcsPool<EnemyComponent> _enemyPool;
        private EcsPool<HealthComponent> _healthPool;

        private static readonly int shotAnimationId = Animator.StringToHash("player_shot");
        private static readonly int damageAnimationId = Animator.StringToHash("enemy_hit");

        public void Init(IEcsSystems systems)
        {
            // Кешируем все необходимое для работы в Run
            _world = systems.GetWorld();
            _filter = _world
                .Filter<PlayerComponent>()
                .Inc<InputEventComponent>()
                .Inc<ShootComponent>()
                .End();

            _filterMonsters = _world
                .Filter<EnemyComponent>()
                .Inc<HealthComponent>()
                .End();

            _inputEventsPool = _world.GetPool<InputEventComponent>();
            _shootPool = _world.GetPool<ShootComponent>();
            _playerPool = _world.GetPool<PlayerComponent>();
            _enemyPool = _world.GetPool<EnemyComponent>();
            _healthPool = _world.GetPool<HealthComponent>();

            SetAim();
        }

        public void Destroy(IEcsSystems systems)
        {
            UnsetAim();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref InputEventComponent inputEventComponent = ref _inputEventsPool.Get(entity);
                ref ShootComponent shootComponent = ref _shootPool.Get(entity);
                ref PlayerComponent playerComponent = ref _playerPool.Get(entity);
                
                shootComponent.passedTime += Time.deltaTime;
                if (!inputEventComponent.mouseButton_L) continue;
                if (shootComponent.passedTime < shootComponent.shootRate) continue;
                shootComponent.passedTime = 0;

                var damage = shootComponent.damage;
                var direction = playerComponent.transform.up;

                Shot(playerComponent.transform.position, direction, damage, playerComponent.animator);
            }
        }


        private void Shot(Vector2 position, Vector2 direction, float damage, Animator playeraAnimator)
        {
            int maxShootingEnemy = 15;   // Сколько за раз противников можно прошить одной пулей
            foreach (int entity in _filterMonsters)
            {
                ref EnemyComponent enemyComponent = ref _enemyPool.Get(entity);
                ref HealthComponent healthComponent = ref _healthPool.Get(entity);

                var hitCheck = HitCheck(position, direction, enemyComponent.transform.position);
                if (!hitCheck) continue;    // Не попали

#if UNITY_EDITOR
                Debug.DrawRay(position, direction * 7f, Color.red, 0.1f);
#endif
                if (healthComponent.health <= 0) continue;   // Обезопасимся и не будем убивать мертвого

                //здоровье = здоровье - урон * защита
                healthComponent.health -= damage * (1f-healthComponent.armor);
                enemyComponent.animator.Play(damageAnimationId);
                maxShootingEnemy--;
                if (maxShootingEnemy < 0) break;
            }
            playeraAnimator.Play(shotAnimationId);
        }

        // TODO Возможно, вынести в отдельный поток
        private bool HitCheck(Vector2 hitPoint, Vector2 hitDirection, Vector2 targetPosition)
        {
            float rayLength = 8f;
            float radius = 0.4f;

            var p2 = hitPoint;
            var p1 = hitPoint + hitDirection * rayLength;

#if UNITY_EDITOR
            Debug.DrawLine(p1, p2, Color.magenta, 0.1f);
#endif
            var center = targetPosition;

            float x01 = p1.x - center.x;
            float y01 = p1.y - center.y;
            float x02 = p2.x - center.x;
            float y02 = p2.y - center.y;
            float dx = x02 - x01;
            float dy = y02 - y01;
            float a = dx * dx + dy * dy;
            float b = 2.0f * (x01 * dx + y01 * dy);
            float c = x01 * x01 + y01 * y01 - radius * radius;
            if (-b < 0) return (c < 0);
            if (-b < (2.0f * a)) return (4.0f * a * c - b * b < 0);
            return (a + b + c < 0);
        }


        private void SetAim()
        {
            foreach (int entity in _filter)
            {
                ref ShootComponent shootComponent = ref _shootPool.Get(entity);
                Cursor.SetCursor(shootComponent.aimTexture, Vector2.zero, CursorMode.Auto);
            }
        }

        private void UnsetAim()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}