using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    public int score = 0;
    public float totalGameTime = 60f; // Общее время игры
    private float timeLeft;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI accuracy;
    public Canvas gameOverCanvas;
    public GameObject crosshair;
    private int shotsFired;
    private int shotsHit;
    private float accuracyPercentage;

    // Событие для увеличения сложности (без номера волны)
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
            OnDifficultyIncreased?.Invoke(); // Уведомляем о повышении сложности
            timeSinceLastIncrease -= increaseInterval; // Сбрасываем только интервал
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
        gameOverCanvas.gameObject.SetActive(true); // Показываем экран окончания игры
        crosshair.SetActive(false); // Скрываем прицел
        Time.timeScale = 0f; // Останавливаем игру
        // Здесь можно добавить логику перехода в меню или перезапуска
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Возобновляем время
        SceneManager.LoadScene("GameScene"); // Перезапуск сцены
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // Возобновляем время
        SceneManager.LoadScene("MainMenu"); // Переход в главное меню
    }
}