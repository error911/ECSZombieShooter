using Leopotam.EcsLite;
using UnityEngine;

namespace TestGame.ECS
{
    public sealed class PlayerMoveSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<MovableComponent> _movablePool;
        private EcsPool<InputEventComponent> _inputEventsPool;

        public void Init(IEcsSystems systems)
        {
            // Кешируем все необходимое для работы в Run
            _world = systems.GetWorld();
            _filter = _world
                .Filter<InputEventComponent>()
                .Inc<MovableComponent>()
                .End();

            _movablePool = _world.GetPool<MovableComponent>();
            _inputEventsPool = _world.GetPool<InputEventComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref InputEventComponent inputEventComponent = ref _inputEventsPool.Get(entity);
                ref MovableComponent movableComponent = ref _movablePool.Get(entity);

                Vector3 direction = inputEventComponent.direction
                    * movableComponent.moveSpeed
                    * Time.deltaTime;

                movableComponent.transform.position += direction;
                movableComponent.isMoving = inputEventComponent.direction.sqrMagnitude > 0;

                var aimPos = inputEventComponent.aimPosition - (Vector2)movableComponent.transform.position;

                Rotate(movableComponent.transform, aimPos);
            }

        }

        private void Rotate(Transform transform, Vector2 direction)
        {
            transform.rotation = Quaternion.identity;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, 1f, 0.0f);
            transform.rotation = Quaternion.LookRotation(transform.forward, newDirection);
#if UNITY_EDITOR
            Debug.DrawRay(transform.position, transform.up * 4f, Color.cyan);
#endif
        }
    }
}