using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI accuracy;
    [SerializeField] private TextMeshProUGUI finalScore;

    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private float totalGameTime = 60f; // Общее время игры

    private int score = 0;

    private float timeLeft;


    private int shotsFired;
    private int shotsHit;
    private float accuracyPercentage;

    // Ивент для увеличения сложности
    public static event Action OnDifficultyIncreased;

    private float increaseInterval = 20f; // Интервал повышения сложности (20 секунд)
    private float timeSinceLastIncrease; // Время с последнего увеличения

    void Start()
    {
        Cursor.visible = false;
        Time.timeScale = 1f;
        timeLeft = totalGameTime;
        timeSinceLastIncrease = 0f;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndGame();
        }

        timeLeft -= Time.deltaTime;
        timeSinceLastIncrease += Time.deltaTime;

        timerText.text = "Время: " + Mathf.Round(timeLeft).ToString();
        scoreText.text = "Очки: " + score.ToString();

        // Проверка времени для увеличения сложности
        if (timeSinceLastIncrease >= increaseInterval)
        {
            OnDifficultyIncreased?.Invoke(); // вызов ивента
            timeSinceLastIncrease -= increaseInterval; // Сброс интервала
        }

        // Завершение игры
        if (timeLeft <= 0)
        {
            timeLeft = 0;
            EndGame();
        }
    }

    public void AddScore(int points)
    {
        score += points;
    }

    public void Accuracy(bool hit)
    {
        audioSource.PlayOneShot(shootSound);
        shotsFired++;
        if (hit)
        {
            shotsHit++;
        }
        accuracyPercentage = (float)Math.Round((float)shotsHit / shotsFired * 100, 2);
        accuracy.text = "Точность: " + accuracyPercentage.ToString() + "%";
    }

    private void EndGame()
    {
        Cursor.visible = true;
        // Сбрасываем параметры префаба куриц через ChickenSpawner
        ChickenSpawner spawner = FindObjectOfType<ChickenSpawner>();
        if (spawner != null)
        {
            spawner.ResetChickenParameters();
        }

        Debug.Log("Игра окончена! Очки: " + score);
        gameOverCanvas.gameObject.SetActive(true); // экран окончания игры
        finalScore.text = "Итоговый счёт: " + Math.Round(score * accuracyPercentage / 100, 2).ToString();
        crosshair.SetActive(false); 
        Time.timeScale = 0f; 
 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}