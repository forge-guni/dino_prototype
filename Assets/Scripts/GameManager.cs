using System;
using System.Collections;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public GameObject startScreen;
        public GameObject gameScreen;
        public GameObject gameOverScreen;
        public DinoController dinoController;
        public RectTransform highScorePanel;
        public GameObject highScoreTemplate;
        public TMP_Text scoreText;
        public Obstacle templateObstacle;
        
        public Button startButton;
        public Button replayButton;
        
        private string _savePath => Path.Combine(Application.persistentDataPath, "highscores.json");

        private void Awake()
        {
            startScreen.SetActive(true);
            gameOverScreen.SetActive(false);
            gameScreen.SetActive(false);
            startButton.onClick.AddListener(StartGame);
            replayButton.onClick.AddListener(StartGame);
            highScoreTemplate.SetActive(false);

            dinoController.DinoIsDead += GameOver;
            dinoController.ScoreUpdated += UpdateScore;
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveAllListeners();
            replayButton.onClick.RemoveAllListeners();

            dinoController.DinoIsDead -= GameOver;
            dinoController.ScoreUpdated -= UpdateScore;
        }

        //object pooling is better, time is running out :D
        private IEnumerator InstantiateObstacle()
        {
            
            yield return null;
        }

        private void GameOver()
        {
            gameScreen.SetActive(false);
            gameOverScreen.SetActive(true);
            SaveScore();
        }
        
        private void UpdateScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        private void StartGame()
        {
            startScreen.SetActive(false);
            ListHighScores();
            gameOverScreen.SetActive(false);
            gameScreen.SetActive(true);
            dinoController.StartDinoing();
        }

        private void ListHighScores()
        {
            if (File.Exists(_savePath))
            {
                var content = File.ReadAllText(_savePath);
                var scoreData = JsonConvert.DeserializeObject<ScoreData>(content);
                var highScore = Instantiate(highScoreTemplate, highScorePanel);
                highScore.GetComponentInChildren<TMP_Text>().text = $"High: {scoreData.score} at {scoreData.date}";
                highScore.gameObject.SetActive(true);
            }
        }

        private void SaveScore()
        {
            var score = dinoController.score;
            var scoreData = new ScoreData()
            {
                score = score,
                date = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
            };
            var json = JsonConvert.SerializeObject(scoreData);
            File.WriteAllText(_savePath, json);
        }
    }

    [Serializable]
    public class ScoreData
    {
        public int score;
        public string date;
    }
}