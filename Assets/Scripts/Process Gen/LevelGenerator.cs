using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject[] chunkPrefab;
    [SerializeField] GameObject checkpointChunkPrefab;
    [SerializeField] Transform chunkParent;
    [SerializeField] ScoreManager scoreManager;
    
    [Header("Level Settings")]
    [SerializeField] int startingChunkAmount = 12;
    [SerializeField] int checkpointChunkInterval = 8;
    [SerializeField] float chunkLength = 10f;
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float minMoveSpeed = 2f;
    [SerializeField] float maxMoveSpeed = 20f;
    [SerializeField] float minGravityZ = -22f;
    [SerializeField] float maxGravityZ = -2;

    List<GameObject> chunks = new List<GameObject>();

    int chunksSpawned = 0;
    void Start()
    {
        SpawnStartingChunks();
    }

    void Update()
    {
        MoveChunks();
    }

    public void ChangeChunkMoveSpeed(float speedAmount)
    {
        float newMoveSpeed = moveSpeed + speedAmount;
        newMoveSpeed = Mathf.Clamp(newMoveSpeed, minMoveSpeed, maxMoveSpeed);
        if (newMoveSpeed != moveSpeed)
        {
            moveSpeed = newMoveSpeed;
            
            float newGravityZ = Physics.gravity.z - speedAmount;
            newGravityZ = Mathf.Clamp(newGravityZ, minGravityZ, maxGravityZ);
            Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, newGravityZ);
            
            cameraController.ChangeCameraFOV(speedAmount);
        }
    }

    void SpawnStartingChunks()
    {
        for (int i = 0; i < startingChunkAmount; i++)
        {
            SpawnChunk();
        }
    }
    void SpawnChunk()
    {
        float spawnPositionZ = CalculateSpawnPosition();
        Vector3 chunkSpawnPosition = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);
        GameObject chunkToSpawn = ChooseChunkToSpawn();
        GameObject newChunkGO = Instantiate(chunkToSpawn, chunkSpawnPosition, Quaternion.identity, chunkParent);
        chunks.Add(newChunkGO);
        Chunk newChunk = newChunkGO.GetComponent<Chunk>();
        newChunk.Init(this, scoreManager);
        chunksSpawned++;
    }

    private GameObject ChooseChunkToSpawn()
    {
        GameObject chunkToSpawn;
        if (chunksSpawned % checkpointChunkInterval == 0 && chunksSpawned != 0)
        {
            chunkToSpawn = checkpointChunkPrefab;
        }
        else
        {
            chunkToSpawn = chunkPrefab[Random.Range(0, chunkPrefab.Length)];   
        }

        return chunkToSpawn;
    }

    float CalculateSpawnPosition()
    {
        float spawnPositionZ;
        if (chunks.Count == 0)
        {
            spawnPositionZ = transform.position.z;
        }
        else
        {
            spawnPositionZ = chunks[chunks.Count - 1].transform.position.z + chunkLength;
        }

        return spawnPositionZ;
    }

    void MoveChunks()
    {
        for (int i = 0; i < chunks.Count; i++)
        {
            GameObject chunk = chunks[i];
            chunk.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
            
            if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)
            {
                chunks.Remove(chunk);
                Destroy(chunk);
                SpawnChunk();
            }
        }
    }
}