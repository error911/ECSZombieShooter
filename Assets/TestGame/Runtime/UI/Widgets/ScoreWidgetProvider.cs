using System.Threading.Tasks;

namespace TestGame
{
    public class ScoreWidgetProvider : LocalAssetLoader
    {
        public Task<ScoreWidget> Load() => LoadInternal<ScoreWidget>("ScoreWidget");

        public void Unload() => UnloadInternal();
    }
}