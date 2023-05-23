namespace TestGame.UIs
{
    // Тут ссылки на UI провайдеры, для загрузки/выгрузки в реалтайме
    public class UIController
    {
        private ScoreWidgetProvider _scoreWidgetProvider = new ScoreWidgetProvider();
        private PlayerHPWidgetProvider _playerHPWidgetProvider = new PlayerHPWidgetProvider();
        
        public ScoreWidgetProvider ScoreWidgetProvider => _scoreWidgetProvider;
        public PlayerHPWidgetProvider PlayerHPWidgetProvider => _playerHPWidgetProvider;

    }
}