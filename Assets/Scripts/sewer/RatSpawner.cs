using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject swarmerPrefab;

    [SerializeField]
    private int totalSwarmerEnemies = 10;
    [SerializeField]
    private float swarmerInterval = 2.5f;

    private int swarmerCount = 0;

    private void Start()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        while (swarmerCount < totalSwarmerEnemies)
        {
            if (swarmerCount < totalSwarmerEnemies)
            {
                SpawnEnemy(swarmerPrefab);
                swarmerCount++;
            }

            yield return new WaitForSeconds(swarmerInterval);
        }

        // Wait for a delay before starting the next wave.
        yield return new WaitForSeconds(2.0f); // Adjust this time to control the gap between waves.

        // Start the next wave.
        StartCoroutine(SpawnWave());
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition;
        bool isPositionValid = false;

        // Define a maximum number of attempts to prevent an infinite loop
        int maxAttempts = 50;
        int attempts = 0;

        do
        {
            // Generate a random spawn position
            spawnPosition = new Vector3(Random.Range(22f, 32f), Random.Range(-4f,15f), 0);

            // Check if the spawn position is valid (not inside wall colliders)
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(spawnPosition, 0.5f); // Adjust the radius as needed
            isPositionValid = true;

            foreach (Collider2D collider in hitColliders)
            {
                if (collider.CompareTag("walls"))
                {
                    isPositionValid = false;
                    break;
                }
            }

            attempts++;
        } while (!isPositionValid && attempts < maxAttempts);

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("Unable to find a valid spawn position after multiple attempts.");
        }

        return spawnPosition;
    }
}