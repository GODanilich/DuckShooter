using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    public int score = 0;
    public float totalGameTime = 60f; // ����� ����� ����
    private float timeLeft;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI accuracy;
    public Canvas gameOverCanvas;
    public GameObject crosshair;
    private int shotsFired;
    private int shotsHit;
    private float accuracyPercentage;

    // ������� ��� ���������� ��������� (��� ������ �����)
    public static event Action OnDifficultyIncreased;

    private float increaseInterval = 20f; // �������� ��������� ��������� (20 ������)
    private float timeSinceLastIncrease; // ����� � ���������� ����������

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

        timerText.text = "�����: " + Mathf.Round(timeLeft).ToString();
        scoreText.text = "����: " + score.ToString();

        // �������� ������� ��� ���������� ���������
        if (timeSinceLastIncrease >= increaseInterval)
        {
            OnDifficultyIncreased?.Invoke(); // ���������� � ��������� ���������
            timeSinceLastIncrease -= increaseInterval; // ���������� ������ ��������
        }

        // ���������� ����
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
        accuracy.text = "��������: " + accuracyPercentage.ToString() + "%";
    }

    private void EndGame()
    {
        Cursor.visible = true;
        // ���������� ��������� ������� ����� ����� ChickenSpawner
        ChickenSpawner spawner = FindObjectOfType<ChickenSpawner>();
        if (spawner != null)
        {
            spawner.ResetChickenParameters();
        }

        Debug.Log("���� ��������! ����: " + score);
        gameOverCanvas.gameObject.SetActive(true); // ���������� ����� ��������� ����
        crosshair.SetActive(false); // �������� ������
        Time.timeScale = 0f; // ������������� ����
        // ����� ����� �������� ������ �������� � ���� ��� �����������
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // ������������ �����
        SceneManager.LoadScene("GameScene"); // ���������� �����
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // ������������ �����
        SceneManager.LoadScene("MainMenu"); // ������� � ������� ����
    }
}