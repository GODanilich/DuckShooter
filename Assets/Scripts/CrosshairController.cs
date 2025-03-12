using UnityEngine;
using UnityEngine.Rendering;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);

        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (hit.collider == null)
        {
            gameManager.Accuracy(false);
        }
        else if (hit.collider.CompareTag("Chicken"))
        {
            //audioSource.volume = Mathf.Clamp01(1f);
            audioSource.PlayOneShot(hitSound);
            Destroy(hit.collider.gameObject);
            gameManager.Accuracy(true);
            gameManager.AddScore(10);
        }
        else if (hit.collider.CompareTag("Bonus"))
        {
            Destroy(hit.collider.gameObject);
            gameManager.Accuracy(true);
            gameManager.AddScore(50);
        }
    }
}