using Project.Scripts.Experience;
using TMPro;
using UnityEngine;

namespace Project.Scripts.Score
{
    public class ScoreView : MonoBehaviour, IView
    {
        [SerializeField] private TMP_Text _scoreText;
        
        private ExperiencePoints experiencePoints;
        
        public void Construct(ExperiencePoints experiencePoints)
        {
            this.experiencePoints = experiencePoints;
        }

        private void OnEnable()
        {
            //experiencePoints.ValueIsChanged += ValueIsChanged;
        }

        private void OnDisable()
        {
            //experiencePoints.ValueIsChanged -= ValueIsChanged;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private void ValueIsChanged(int value, int maxValue)
        {
            _scoreText.text = value.ToString();
        }
    }
}