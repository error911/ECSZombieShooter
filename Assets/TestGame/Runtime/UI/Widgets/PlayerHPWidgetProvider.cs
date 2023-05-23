using System.Threading.Tasks;

namespace TestGame
{
    public class PlayerHPWidgetProvider : LocalAssetLoader
    {
        public Task<PlayerHPWidget> Load() => LoadInternal<PlayerHPWidget>("PlayerHPWidget");

        public void Unload() => UnloadInternal();
    }
}