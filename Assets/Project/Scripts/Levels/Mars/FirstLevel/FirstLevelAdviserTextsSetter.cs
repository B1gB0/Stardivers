using Project.Scripts.UI.Panel;

namespace Project.Scripts.Levels.Mars.FirstLevel
{
    public class FirstLevelAdviserTextsSetter
    {
        private AdviserMessagePanel _adviserMessagePanel;
        
        private const string WelcomeMarsText =
            "Марс встречает тебя, десантник! Мы получили сигнал бедствия с планеты." +
            " Поэтому действуем быстро! Тебе нужно проверить ближайший радиопередатчик " +
            "около поста колонистов.";

        private const string EnemySpawnTriggerText = "Десантник, зафиксированы неопознанные биосигналы." +
                                                     " Они всё ближе к тебе! Приготовься!";

        private const string EndAttackText = "Атака закончилась! Пора отправиться в ближайший аванпост" +
                                             " и перевести дух!";

        public void GetAdviserPanel(AdviserMessagePanel adviserMessagePanel)
        {
            _adviserMessagePanel = adviserMessagePanel;
        }

        public void SetAndShowWelcomePlanetText()
        {
            _adviserMessagePanel.SetText(WelcomeMarsText);
            _adviserMessagePanel.Show();
        }

        public void SetAndShowEnemySpawnTriggerText()
        {
            _adviserMessagePanel.SetText(EnemySpawnTriggerText);
            _adviserMessagePanel.Show();
        }

        public void SetAndShowEndAttackText()
        {
            _adviserMessagePanel.SetText(EndAttackText);
            _adviserMessagePanel.Show();
        }
    }
}