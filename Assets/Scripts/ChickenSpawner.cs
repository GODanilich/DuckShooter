using UnityEngine;
using System.Collections;

public class ChickenSpawner : MonoBehaviour
{
    [SerializeField] private GameObject chickenPrefab;
    [SerializeField] private float spawnInterval = 1f; // �������� ����� �������� ����� �����
    [SerializeField] private int chickenCount = 3; // ��������� ���������� ����� � ����� �����
    [SerializeField] private float speedIncrease = 0.5f; // ���������� �������� �����
    [SerializeField] private int countIncrease = 1; // ���������� ���������� ����� � �����
    private DifficultySettings difficultySettings; // ������ �� ������ � ����������� ���������

    private float screenWidth;
    private float screenHeight;

    // ��������� ��� ��������� ���������
    private int initialChickenCount; // ��������� �������� ��� ������
    private float initialMinSpeed; // ��������� ����������� ��������
    private float initialMaxSpeed; // ��������� ������������ ��������

    private int difficultyIncreases = 0; // ������� ���������� ���������

    void Start()
    {

        difficultySettings = FindObjectOfType<DifficultySettings>();
        if (difficultySettings != null)
        {
            switch (difficultySettings.currentDifficulty)
            {
                case DifficultySettings.DifficultyLevel.Easy:
                    spawnInterval = 1.5f;
                    chickenCount = 2;
                    countIncrease = 2;
                    break;
                case DifficultySettings.DifficultyLevel.Medium:
                    spawnInterval = 1.5f;
                    chickenCount = 3;
                    countIncrease = 3;
                    break;
                case DifficultySettings.DifficultyLevel.Hard:
                    spawnInterval = 0.5f;
                    chickenCount = 3;
                    countIncrease = 4;
                    break;
            }
        }

        // ��������� ������� ������ � ������� �����������
        screenHeight = Camera.main.orthographicSize * 2f;
        screenWidth = screenHeight * Camera.main.aspect;

        // ��������� ��������� �������� ��� ������
        initialChickenCount = chickenCount;
        ChickenMovement chickenMovement = chickenPrefab.GetComponent<ChickenMovement>();
        if (chickenMovement != null)
        {
            initialMinSpeed = chickenMovement.minSpeed;
            initialMaxSpeed = chickenMovement.maxSpeed;
        }

        // ������������� �� ������� ���������� ��������� �� GameManager
        GameManager.OnDifficultyIncreased += IncreaseDifficulty;

        StartCoroutine(SpawnWave());
    }

    void OnDestroy()
    {
        // ������������ �� ������� ��� ����������� �������
        GameManager.OnDifficultyIncreased -= IncreaseDifficulty;
        difficultySettings = null;
    }

    void IncreaseDifficulty()
    {
        difficultyIncreases++; // ����������� �������

        // ����������� ���������� ����� � ��������
        chickenCount = initialChickenCount + countIncrease * difficultyIncreases;

        ChickenMovement chickenMovement = chickenPrefab.GetComponent<ChickenMovement>();
        if (chickenMovement != null)
        {
            chickenMovement.minSpeed = initialMinSpeed + speedIncrease * difficultyIncreases;
            chickenMovement.maxSpeed = initialMaxSpeed + speedIncrease * difficultyIncreases;
        }

        Debug.Log($"��������� ���������: ����� = {chickenCount}, �������� = {chickenMovement.minSpeed}-{chickenMovement.maxSpeed}");
    }

    public void ResetChickenParameters()
    {
        // ���������� ��������� ������� �� ��������� ��������
        chickenCount = initialChickenCount;
        difficultyIncreases = 0; // ���������� ������� ����������
        ChickenMovement chickenMovement = chickenPrefab.GetComponent<ChickenMovement>();
        if (chickenMovement != null)
        {
            chickenMovement.minSpeed = initialMinSpeed;
            chickenMovement.maxSpeed = initialMaxSpeed;
        }
    }

    IEnumerator SpawnWave()
    {
        while (true)
        {
            // ������� �������� ���������� �����
            for (int i = 0; i < chickenCount; i++)
            {
                SpawnChicken();
                yield return new WaitForSeconds(spawnInterval / chickenCount); // ���������� ������������ �����
            }

            // ��� ����� ��������� ������
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnChicken()
    {
        // �������� �������� ������� ������ (0 - �����, 1 - ������)
        int side = Random.Range(0, 2);
        Vector3 spawnPosition;
        int direction; // ����������� ��������: 1 = ������, -1 = �����

        if (side == 0) // �����
        {
            spawnPosition = new Vector3(-screenWidth / 2 - 1f, Random.Range(-screenHeight / 2, screenHeight / 2), 0);
            direction = 1; // ������ ����� ������
        }
        else // ������
        {
            spawnPosition = new Vector3(screenWidth / 2 + 1f, Random.Range(-screenHeight / 2, screenHeight / 2), 0);
            direction = -1; // ������ ����� �����
        }

        // ������ ������ � ��������� �������
        GameObject chicken = Instantiate(chickenPrefab, spawnPosition, Quaternion.identity);

        // ����� ��������� ������� �� 1 �� 4
        float scale = Random.Range(1f, 4f);
        chicken.transform.localScale = new Vector3(scale, scale, 1f);

        // ������������� ����������� � ChickenMovement
        ChickenMovement chickenMovement = chicken.GetComponent<ChickenMovement>();
        if (chickenMovement != null)
        {
            chickenMovement.SetDirection(direction);
        }
    }
}