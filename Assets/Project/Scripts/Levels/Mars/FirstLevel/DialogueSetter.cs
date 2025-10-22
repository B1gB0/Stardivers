using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Levels.Mars.FirstLevel
{
    public class DialogueSetter
    {
        private readonly DialoguePanel _dialoguePanel;
        private readonly ILevelTextService _levelTextService;

        public DialogueSetter(DialoguePanel dialoguePanel, ILevelTextService levelTextService)
        {
            _dialoguePanel = dialoguePanel;
            _levelTextService = levelTextService;
        }

        private void SetText(LevelTextsType type)
        {
            _dialoguePanel.SetText(_levelTextService.GetLevelTextData(SceneManager.GetActiveScene().name, type));
            _dialoguePanel.Show();
        }

        public void OnWelcomePlanet()
        {
            SetText(LevelTextsType.WelcomeText);
        }

        public void OnEndAttack()
        {
            SetText(LevelTextsType.EndAttackText);
        }

        public void OnEnemySpawnTriggerWithEffect()
        {
            SetText(LevelTextsType.EnemySpawnTriggerText);
        }
    }
}