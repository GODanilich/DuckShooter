using UnityEngine;
using System.Collections;

public class ChickenSpawner : MonoBehaviour
{
    [SerializeField] private GameObject chickenPrefab;
    [SerializeField] private float spawnInterval = 1f; // Интервал между спавнами одной волны
    [SerializeField] private int chickenCount = 3; // Начальное количество куриц в одной волне
    [SerializeField] private float speedIncrease = 0.5f; // Увеличение скорости куриц
    [SerializeField] private int countIncrease = 1; // Увеличение количества куриц в волне
    private DifficultySettings difficultySettings; // Ссылка на скрипт с настройками сложности

    private float screenWidth;
    private float screenHeight;

    // Параметры для повышения сложности
    private int initialChickenCount; // Начальное значение для сброса
    private float initialMinSpeed; // Начальная минимальная скорость
    private float initialMaxSpeed; // Начальная максимальная скорость

    private int difficultyIncreases = 0; // Счётчик увеличений сложности

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

        // Вычисляем границы экрана в мировых координатах
        screenHeight = Camera.main.orthographicSize * 2f;
        screenWidth = screenHeight * Camera.main.aspect;

        // Сохраняем начальные значения для сброса
        initialChickenCount = chickenCount;
        ChickenMovement chickenMovement = chickenPrefab.GetComponent<ChickenMovement>();
        if (chickenMovement != null)
        {
            initialMinSpeed = chickenMovement.minSpeed;
            initialMaxSpeed = chickenMovement.maxSpeed;
        }

        // Подписываемся на событие увеличения сложности из GameManager
        GameManager.OnDifficultyIncreased += IncreaseDifficulty;

        StartCoroutine(SpawnWave());
    }

    void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        GameManager.OnDifficultyIncreased -= IncreaseDifficulty;
        difficultySettings = null;
    }

    void IncreaseDifficulty()
    {
        difficultyIncreases++; // Увеличиваем счётчик

        // Увеличиваем количество куриц и скорость
        chickenCount = initialChickenCount + countIncrease * difficultyIncreases;

        ChickenMovement chickenMovement = chickenPrefab.GetComponent<ChickenMovement>();
        if (chickenMovement != null)
        {
            chickenMovement.minSpeed = initialMinSpeed + speedIncrease * difficultyIncreases;
            chickenMovement.maxSpeed = initialMaxSpeed + speedIncrease * difficultyIncreases;
        }

        Debug.Log($"Сложность увеличена: Куриц = {chickenCount}, Скорость = {chickenMovement.minSpeed}-{chickenMovement.maxSpeed}");
    }

    public void ResetChickenParameters()
    {
        // Сбрасываем параметры префаба до начальных значений
        chickenCount = initialChickenCount;
        difficultyIncreases = 0; // Сбрасываем счётчик увеличений
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
            // Спавним заданное количество куриц
            for (int i = 0; i < chickenCount; i++)
            {
                SpawnChicken();
                yield return new WaitForSeconds(spawnInterval / chickenCount); // Равномерно распределяем спавн
            }

            // Ждём перед следующей волной
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnChicken()
    {
        // Случайно выбираем сторону спавна (0 - слева, 1 - справа)
        int side = Random.Range(0, 2);
        Vector3 spawnPosition;
        int direction; // Направление движения: 1 = вправо, -1 = влево

        if (side == 0) // Слева
        {
            spawnPosition = new Vector3(-screenWidth / 2 - 1f, Random.Range(-screenHeight / 2, screenHeight / 2), 0);
            direction = 1; // Курица летит вправо
        }
        else // Справа
        {
            spawnPosition = new Vector3(screenWidth / 2 + 1f, Random.Range(-screenHeight / 2, screenHeight / 2), 0);
            direction = -1; // Курица летит влево
        }

        // Создаём курицу в выбранной позиции
        GameObject chicken = Instantiate(chickenPrefab, spawnPosition, Quaternion.identity);

        // Задаём случайный масштаб от 1 до 4
        float scale = Random.Range(1f, 4f);
        chicken.transform.localScale = new Vector3(scale, scale, 1f);

        // Устанавливаем направление в ChickenMovement
        ChickenMovement chickenMovement = chicken.GetComponent<ChickenMovement>();
        if (chickenMovement != null)
        {
            chickenMovement.SetDirection(direction);
        }
    }
}