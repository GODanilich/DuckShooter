using UnityEngine;

public class ChickenMovement : MonoBehaviour
{
    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxSpeed = 5f;
    private float speed;

    void Start()
    {
        // Задаём случайную скорость
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        // Двигаем курицу вправо
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}