using Leopotam.EcsLite;
using UnityEngine;

namespace TestGame.ECS
{
    public sealed class PlayerInputSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<InputEventComponent> _inputEventsPool;
        private Camera _camera;
        public void Init(IEcsSystems systems)
        {
            // Кешируем все необходимое для работы в Run
            _world = systems.GetWorld();
            _filter = _world
                .Filter<InputEventComponent>()
                .End();

            _inputEventsPool = _world.GetPool<InputEventComponent>();

            _camera = Camera.main;
        }

        public void Run(IEcsSystems systems)
        {
            var xPos = Input.GetAxis("Horizontal");
            var yPos = Input.GetAxis("Vertical");
            
            var lmb = Input.GetMouseButton(0);

            var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);

            foreach (int entity in _filter)
            {
                ref InputEventComponent inputEventComponent = ref _inputEventsPool.Get(entity);
                inputEventComponent.direction = new Vector2(xPos, yPos);
                inputEventComponent.aimPosition = mousePos;
                inputEventComponent.mouseButton_L = lmb;
            }
        }
    }
}