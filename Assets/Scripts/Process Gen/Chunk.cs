using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] GameObject fencePrefab;
    [SerializeField] GameObject applePrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] float appleSpawnChance = 0.12f;
    [SerializeField] float coinSpawnChance = 0.5f;
    [SerializeField] float[] lanes = {-3f, 0f, 3f};
    [SerializeField] float coinSeparationLength = 2f;

    LevelGenerator levelGenerator;
    ScoreManager scoreManager;
    List<int> availableLanes = new List<int>{0, 1, 2};
    void Start()
    {
        SpawnFences();
        SpawnApples();
        SpawnCoins();
    }

    public void Init(LevelGenerator levelGenerator, ScoreManager scoreManager)
    {
        this.levelGenerator = levelGenerator;
        this.scoreManager = scoreManager;
    }

    void SpawnFences()
    {
        int fencesToSpawn = Random.Range(0, lanes.Length);
   
        for (int i = 0; i < fencesToSpawn; i++)
        {
            if (availableLanes.Count <= 0) break;
            
            int selectedLane = SelectedLane();

            Vector3 fencePosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
            Instantiate(fencePrefab, fencePosition, Quaternion.identity, transform);
        }
    }

    void SpawnApples()
    {
        if (Random.value < appleSpawnChance || availableLanes.Count <= 0) return;
        int selectedLane = SelectedLane();
        Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
        Apple newApple = Instantiate(applePrefab, spawnPosition, Quaternion.identity, transform).GetComponent<Apple>();
        newApple.Init(levelGenerator);
    }

    void SpawnCoins()
    {
        if (Random.value < coinSpawnChance || availableLanes.Count <= 0) return;

        int selectedLane = SelectedLane();

        int maxCoinsToSpawn = 6;
        int coinsToSpawn = Random.Range(0, maxCoinsToSpawn);
        float topOfChunkZPosition = transform.position.z + coinSeparationLength * 2f;
        
        for (int i = 0; i < coinsToSpawn; i++)
        {
            float spawnPositionZ = topOfChunkZPosition - (i * coinSeparationLength);
            Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, spawnPositionZ);
            Coin newCoin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity, transform).GetComponent<Coin>();
            newCoin.Init(scoreManager);
        }
    }
    
    int SelectedLane()
    {
        int randomLaneIndex = Random.Range(0, availableLanes.Count);
        int selectedLane = availableLanes[randomLaneIndex];
        availableLanes.RemoveAt(randomLaneIndex);
        return selectedLane;
    }
}
