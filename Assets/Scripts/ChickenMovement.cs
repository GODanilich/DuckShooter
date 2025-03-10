using UnityEngine;

public class ChickenMovement : MonoBehaviour
{
    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxSpeed = 5f;
    private float speed;

    void Start()
    {
        // ����� ��������� ��������
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        // ������� ������ ������
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}