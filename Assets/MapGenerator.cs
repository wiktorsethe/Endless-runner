using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Transform LevelPart_Start;
    [SerializeField] private List<Transform> LevelPartList;
    [SerializeField] private int poolSize = 5;
    [SerializeField] private float player_distance_spawn_level_part = 20f;

    private Vector3 lastEndPosition;
    private Queue<Transform> levelPartPool = new Queue<Transform>();
    private List<Transform> activeLevelParts = new List<Transform>();

    private void Awake()
    {
        lastEndPosition = LevelPart_Start.Find("EndPosition").position;
        Debug.Log(lastEndPosition);
        InitializePool();
        SpawnLevelPart();
    }

    private void Update()
    {
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, lastEndPosition) < player_distance_spawn_level_part)
        {
            SpawnLevelPart();
        }
    }

    private void InitializePool()
    {
        foreach (var levelPartPrefab in LevelPartList)
        {
            for (int i = 0; i < poolSize; i++)
            {
                Transform obj = Instantiate(levelPartPrefab);
                obj.gameObject.SetActive(false);
                levelPartPool.Enqueue(obj);
            }
        }
    }

    private void SpawnLevelPart()
    {
        if (levelPartPool.Count > 0)
        {
            Transform levelPartTransform = levelPartPool.Dequeue();
            levelPartTransform.position = lastEndPosition;
            levelPartTransform.GetComponent<MapFragment>().SetPosition(lastEndPosition);
            levelPartTransform.gameObject.SetActive(true);
            activeLevelParts.Add(levelPartTransform);
            lastEndPosition = levelPartTransform.Find("EndPosition").position;
        }
    }

    public void ReturnToPool(Transform levelPart)
    {
        levelPart.gameObject.SetActive(false);
        levelPartPool.Enqueue(levelPart);
        activeLevelParts.Remove(levelPart);
    }
}