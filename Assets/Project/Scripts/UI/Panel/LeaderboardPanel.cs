using System;
using Project.Scripts.UI.View;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Scripts.UI.Panel
{
    public class LeaderboardPanel : MonoBehaviour, IView
    {
        [SerializeField] private LeaderboardYG _leaderboardYg;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private Button _backToSceneButton;
        
        public event Action OnBackToSceneButtonPressed;

        private void OnEnable()
        {
            _leaderboardYg.SetLeaderboard(YG2.saves.AcumulatedScore);
            _leaderboardYg.UpdateLB();
            
            _backToSceneButton.onClick.AddListener(MoveBackToScene);
            _leaderboardButton.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _backToSceneButton.onClick.RemoveListener(MoveBackToScene);
            _leaderboardButton.gameObject.SetActive(true);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private void MoveBackToScene()
        {
            OnBackToSceneButtonPressed?.Invoke();
        }
    }
}