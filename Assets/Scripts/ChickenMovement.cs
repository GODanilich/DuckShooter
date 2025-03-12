using UnityEngine;

public class ChickenMovement : MonoBehaviour
{
    public float minSpeed = 2f; // ����������� �������� (�������� ��� ���������)
    public float maxSpeed = 5f; // ������������ �������� (�������� ��� ���������)
    private float speed;
    private int direction; // 1 = ������, -1 = �����
    public float verticalShiftChance = 0.02f; // ���� ����� ������ (2% �� ����)
    public float verticalSpeed = 1f; // �������� ������������� ��������
    private float targetY; // ������� ������ ��� �������� �����������

    void Start()
    {
        // ��������� ������� ��� ���������� �������� (�����������: ������� ���� ���������)
        float scale = transform.localScale.x; // ������� ������� ��� ����� � ChickenSpawner
        speed = Random.Range(minSpeed, maxSpeed);

        // ��������� ����������� � ��������, �������� �������� ������
        transform.localScale = new Vector3(direction * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1f);

        // ������������� ��������� ������� ������
        targetY = transform.position.y;
    }

    public void SetDirection(int newDirection)
    {
        direction = newDirection; // ������������� ����������� �� ChickenSpawner
    }

    void Update()
    {
        // �������������� �������� (����� ��� ������)
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime, Space.World);

        // ��������� ������� � ������������ ��������
        if (Random.value < verticalShiftChance)
        {
            // �������� ����� ��������� ������ � �������� ������
            float screenHeight = Camera.main.orthographicSize;
            targetY = Random.Range(-screenHeight * 0.8f, screenHeight * 0.8f);
        }

        // ������� ����������� � ������� ������
        float newY = Mathf.MoveTowards(transform.position.y, targetY, verticalSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // ���������� ������, ���� ��� ����� �� ������� ������
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPos.x < -0.1f || viewportPos.x > 1.1f)
        {
            Destroy(gameObject);
        }
    }
}