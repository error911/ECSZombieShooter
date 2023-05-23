using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TestGame.ECS
{
    [CreateAssetMenu]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private AssetReferenceGameObject _prefabRef;
        [SerializeField][Min(0)] private int _maxCountInScene = 5;
        [SerializeField][Min(2)] private float _spawnRadius = 10;
        [SerializeField] private float _respawnTime = 5.0f;  // Время воздождения
        [SerializeField] private EnemyTypeConfig[] _enemyTypes;

        //public GameObject prefab => _prefab;
        public AssetReferenceGameObject prefabRef => _prefabRef;
        public int maxCountInScene => _maxCountInScene;
        public float spawnRadius => _spawnRadius;
        public float respawnTime => _respawnTime;
        public EnemyTypeConfig[] enemyTypes => _enemyTypes;

        public int maxCount { get; internal set; }
    }
}