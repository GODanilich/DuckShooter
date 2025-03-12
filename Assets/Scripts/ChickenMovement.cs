using UnityEngine;

public class ChickenMovement : MonoBehaviour
{
    public float minSpeed = 2f; // Минимальная скорость (доступна для изменения)
    public float maxSpeed = 5f; // Максимальная скорость (доступна для изменения)
    private float speed;
    private int direction; // 1 = вправо, -1 = влево
    public float verticalShiftChance = 0.02f; // Шанс смены высоты (2% за кадр)
    public float verticalSpeed = 1f; // Скорость вертикального движения
    private float targetY; // Целевая высота для плавного перемещения

    void Start()
    {
        // Учитываем масштаб при вычислении скорости (опционально: большие куры медленнее)
        float scale = transform.localScale.x; // Базовый масштаб уже задан в ChickenSpawner
        speed = Random.Range(minSpeed, maxSpeed);

        // Применяем направление к масштабу, сохраняя заданный размер
        transform.localScale = new Vector3(direction * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1f);

        // Устанавливаем начальную целевую высоту
        targetY = transform.position.y;
    }

    public void SetDirection(int newDirection)
    {
        direction = newDirection; // Устанавливаем направление из ChickenSpawner
    }

    void Update()
    {
        // Горизонтальное движение (влево или вправо)
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime, Space.World);

        // Случайное решение о вертикальном смещении
        if (Random.value < verticalShiftChance)
        {
            // Выбираем новую случайную высоту в пределах экрана
            float screenHeight = Camera.main.orthographicSize;
            targetY = Random.Range(-screenHeight * 0.8f, screenHeight * 0.8f);
        }

        // Плавное перемещение к целевой высоте
        float newY = Mathf.MoveTowards(transform.position.y, targetY, verticalSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Уничтожаем курицу, если она вышла за пределы экрана
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPos.x < -0.1f || viewportPos.x > 1.1f)
        {
            Destroy(gameObject);
        }
    }
}