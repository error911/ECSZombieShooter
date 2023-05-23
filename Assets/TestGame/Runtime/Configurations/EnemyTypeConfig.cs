using UnityEngine;

namespace TestGame.ECS
{
    [CreateAssetMenu]
    public class EnemyTypeConfig : ScriptableObject
    {
        [SerializeField] [Range(0.05f, 1f)] private float _speedMin = 0.5f;   // Скорость
        [SerializeField] [Range(0.05f, 1f)] private float _speedMax = 0.5f;   // Скорость
        [SerializeField] [Range(0.0f, 1f)] private float _health = 0.5f; // Здоровье
        [SerializeField] [Range(0.0f, 1f)] private float _damage = 0.5f; // Урон
        [SerializeField] [Range(0.0f, 1f)] private float _armor = 0.5f;  // Защита
        [SerializeField] private Sprite _sprite;

        public float speed => Random.Range(_speedMin, _speedMax);
        public float health => _health;
        public float damage => _damage;
        public float armor => _armor;
        public Sprite sprite => _sprite;
    }
}