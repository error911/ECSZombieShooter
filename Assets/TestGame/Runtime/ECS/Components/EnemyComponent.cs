using UnityEngine;

namespace TestGame.ECS
{
    public struct EnemyComponent
    {
        public Animator animator;
        public Transform transform;
        public float damage;
    }
}