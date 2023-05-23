using Leopotam.EcsLite;
using System.IO;
using TestGame.ECS;
using TestGame.UIs;
using UnityEngine;

namespace TestGame
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private EnemyConfig _enemyConfig;

        public static PlayerConfig PlayerConfig => _instance._playerConfig;
        public static EnemyConfig EnemyConfig => _instance._enemyConfig;
        public static UIController UI => _instance._ui;

        private static EntryPoint _instance;
        private EcsWorld _world;
        private EcsSystems _systems;

        private UIController _ui;

        void Awake()
        {
            Application.targetFrameRate = 1000;
            _instance = this;
        }

        private void Start()
        {
            //LoadConfig();
            _ui = new UIController();
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            AddSystems();
        }

        private void AddSystems()
        {
            _systems
                .Add(new GameInitSystem())
                .Add(new PlayerInputSystem())
                .Add(new PlayerMoveSystem())
                .Add(new FollowSystem())
                .Add(new EnemySystem())
                .Add(new PlayerSystem())
                .Add(new PlayerShootSystem());

            _systems.Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            _systems?.Destroy();
            _world?.Destroy();
        }

        private void LoadConfig()
        {
            /*
            var dataEnemy = JsonUtility.ToJson(_instance._enemyConfig);
            File.WriteAllText(Application.dataPath + "/EnemyConfig.json", dataEnemy);

            var dataPlayer = JsonUtility.ToJson(_instance._playerConfig);
            File.WriteAllText(Application.dataPath + "/PlayerConfig.json", dataPlayer);
            */

            //Debug.Log(Application.streamingAssetsPath); //D:/Projects/Test_Work/TroubleShot_Test/TroubleShot_Test/Assets/StreamingAssets
            //Debug.Log(Application.dataPath);    //D:/Projects/Test_Work/TroubleShot_Test/TroubleShot_Test/Assets
            //Debug.Log(Application.persistentDataPath);  //C:/Users/Roman/AppData/LocalLow/DefaultCompany/TroubleShot_Test
            //Debug.Log(Application.consoleLogPath);  //C:/Users/Roman/AppData/LocalLow/DefaultCompany/TroubleShot_Test


            //var eFile = Application.persistentDataPath + "/EnemyConfig.json";
            //var pFile = Application.persistentDataPath + "/PlayerConfig.json";

            var eFile = "../EnemyConfig.json";
            var pFile = "../PlayerConfig.json";

/*
            var dataEnemy = JsonUtility.ToJson(_instance._enemyConfig);
            var dataPlayer = JsonUtility.ToJson(_instance._playerConfig);
            File.WriteAllText(eFile, dataEnemy);
            File.WriteAllText(pFile, dataPlayer);
*/


            if (File.Exists(eFile))
            {
                try
                {
                    var eText = File.ReadAllText(eFile);
                    JsonUtility.FromJsonOverwrite(eText, _instance._enemyConfig);
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex);
                    throw;
                }
                
            }

            if (File.Exists(pFile))
            {
                try
                {
                    var pText = File.ReadAllText(pFile);
                    JsonUtility.FromJsonOverwrite(pText, _instance._playerConfig);
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex);
                    throw;
                }
            }

        }

    }
}