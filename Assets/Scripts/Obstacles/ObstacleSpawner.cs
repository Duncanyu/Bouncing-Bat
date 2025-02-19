using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] obstaclePrefabs;

    [Header("Spawn Configurations")]
    [Range(0.5f, 15.0f)]
    public float initialSpawnInterval = 2f;
    public float spawnX = 10f;
    public float spawnYMin = -3f;
    public float spawnYMax = 3f;
    public float obstacleLifetime = 10f;

    [Header("Obstacle Speeds")]
    public float initialSpeed = 5f;
    public float speedIncreaseRate = 0.1f;

    [Header("Dynamic Spawn Interval")]
    public float spawnIntervalDecreaseRate = 0.05f;
    public float minSpawnInterval = 0.5f;

    private float currentSpeed;
    private float currentSpawnInterval;
    private float timeStart;

    void Start()
    {
        currentSpeed = initialSpeed;
        currentSpawnInterval = initialSpawnInterval;
        StartCoroutine(SpawnObstacles());
        timeStart = Time.time;
    }

    void Update()
    {
        currentSpeed = initialSpeed + ((Time.time - timeStart) * speedIncreaseRate);

        currentSpawnInterval = Mathf.Clamp(
            initialSpawnInterval - ((Time.time - timeStart) * spawnIntervalDecreaseRate), minSpawnInterval, initialSpawnInterval);

        if(PlayerControl.isDead){
            currentSpeed = initialSpeed;
            currentSpawnInterval = initialSpawnInterval;
            timeStart = Time.time;
        }
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            SpawnObstacle();
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    void SpawnObstacle()
    {
        int index = Random.Range(0, obstaclePrefabs.Length);
        GameObject prefabToSpawn = obstaclePrefabs[index];

        Vector3 spawnPosition = new Vector3(spawnX, Random.Range(spawnYMin, spawnYMax), 0f);
        GameObject obstacle = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        ObstaclesMovement movement = obstacle.GetComponent<ObstaclesMovement>();
        if (movement == null)
        {
            movement = obstacle.AddComponent<ObstaclesMovement>();
        }
        movement.speed = currentSpeed;

        Destroy(obstacle, obstacleLifetime);
    }
}
