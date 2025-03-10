using UnityEngine;
using System.Collections;

public class ChickenSpawner : MonoBehaviour
{
    public GameObject chickenPrefab;
    public float spawnInterval = 1f;

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        while (true)
        {
            Instantiate(chickenPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}