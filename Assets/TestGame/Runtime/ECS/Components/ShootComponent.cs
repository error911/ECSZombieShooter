using UnityEngine;

namespace TestGame.ECS
{
    public struct ShootComponent
    {
        public float damage;
        public Texture2D aimTexture;
        public float shootRate;
        public float passedTime;
    }
}