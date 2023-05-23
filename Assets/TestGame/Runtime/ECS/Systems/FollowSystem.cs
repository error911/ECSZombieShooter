using Leopotam.EcsLite;
using UnityEngine;

namespace TestGame.ECS
{
    public sealed class FollowSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<MovableComponent> _movablePool;
        private EcsPool<FollowComponent> _followPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world
                .Filter<FollowComponent>()
                .Inc<MovableComponent>()
                .End();

            _movablePool = _world.GetPool<MovableComponent>();
            _followPool = _world.GetPool<FollowComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref FollowComponent followComponent = ref _followPool.Get(entity);
                if (!followComponent.target) continue;

                ref MovableComponent movableComponent = ref _movablePool.Get(entity);

                var direction = (followComponent.target.position - movableComponent.transform.position).normalized
                    * movableComponent.moveSpeed
                    * Time.deltaTime;

                movableComponent.transform.position += direction;
                movableComponent.isMoving = direction.sqrMagnitude > 0;
            }
        }
    }
}