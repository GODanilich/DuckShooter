using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int score = 0;
    [SerializeField] private float timeLeft = 60f;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;

    void Update()
    {
        timeLeft -= Time.deltaTime;
        timerText.text = "�����: " + Mathf.Round(timeLeft).ToString();
        scoreText.text = "����: " + score.ToString();

        if (timeLeft <= 0)
        {
            // ���� �������� (����� �������� ������ ����������)
            timeLeft = 0;
        }
    }

    public void AddScore(int points)
    {
        score += points;
    }
}