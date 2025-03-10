using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip hitSound;

    void Update()
    {
        // Перемещаем прицел за курсором мыши
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;

        // Стрельба по нажатию левой кнопки мыши
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        audioSource.PlayOneShot(shootSound);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Chicken"))
            {
                audioSource.PlayOneShot(hitSound);
                Destroy(hit.collider.gameObject);
                GameObject.Find("GameManager").GetComponent<GameManager>().AddScore(10);
            }
            else if (hit.collider.CompareTag("Bonus"))
            {
                Destroy(hit.collider.gameObject);
                GameObject.Find("GameManager").GetComponent<GameManager>().AddScore(50);
            }
        }
    }
}