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
        List<Transform> tempPool = new List<Transform>();
        foreach (var levelPartPrefab in LevelPartList)
        {
            for (int i = 0; i < poolSize; i++)
            {
                Transform obj = Instantiate(levelPartPrefab);
                obj.gameObject.SetActive(false);
                tempPool.Add(obj);
            }
        }
        ShuffleList(tempPool);
        foreach (var obj in tempPool)
        {
            levelPartPool.Enqueue(obj);
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

    private void ShuffleList(List<Transform> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    public void ReturnToPool(Transform levelPart)
    {
        levelPart.gameObject.SetActive(false);
        levelPartPool.Enqueue(levelPart);
        activeLevelParts.Remove(levelPart);
    }
    
    public void ResetLevel()
    {
        foreach (var levelPart in activeLevelParts)
        {
            levelPart.gameObject.SetActive(false);
            levelPartPool.Enqueue(levelPart);
        }
    
        activeLevelParts.Clear();
        lastEndPosition = LevelPart_Start.Find("EndPosition").position;
    }
}
