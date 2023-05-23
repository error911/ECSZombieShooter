using UnityEngine;

namespace TestGame.ECS
{
    public struct InputEventComponent
    {
        public Vector2 direction;
        public Vector2 aimPosition;
        public bool mouseButton_L;
    }
}