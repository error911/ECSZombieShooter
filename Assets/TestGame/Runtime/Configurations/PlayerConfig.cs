using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TestGame.ECS
{
    [CreateAssetMenu]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private AssetReferenceGameObject _prefabRef;
        [SerializeField] [Range(1f, 10f)] private float _speed = 3f;
        [SerializeField] [Range(0f, 1f)] private float _health = 1f;
        [SerializeField][Range(0.0f, 1f)] private float _damage = 0.5f;
        [SerializeField][Range(0.05f, 1f)] private float _shootRate = 0.5f;
        [SerializeField] private Texture2D _aimTexture;

        //public GameObject prefab => _prefab;
        public AssetReferenceGameObject prefabRef => _prefabRef;
        public float speed => _speed;
        public float health => _health;
        public float damage => _damage;
        public float shootRate => _shootRate;
        public Texture2D aimTexture => _aimTexture;
    }
}