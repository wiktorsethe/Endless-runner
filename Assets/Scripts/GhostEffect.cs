using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    [Header("Ustawienia efektu ducha")]
    [SerializeField] private SpriteRenderer targetSpriteRenderer;
    [SerializeField] private float spawnRate = 0.1f;
    [SerializeField] private int maxGhosts = 4;
    [SerializeField] private Color ghostColor = new Color(1, 1, 1, 0.5f);
    [SerializeField] private float ghostLifetime = 0.5f;
    [SerializeField] private bool startOnAwake = false;

    private float timer;
    private Queue<GameObject> ghostPool = new Queue<GameObject>();
    private bool isActive = false;

    private static GhostEffect _instance;

    void Awake()
    {
        if (_instance == null)
            _instance = this;

        if (startOnAwake)
        {
            StartEffect();
        }
    }

    void Start()
    {
        CreateGhostPool();
    }

    void Update()
    {
        if (!isActive || targetSpriteRenderer == null) return;

        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnGhost();
            timer = 0f;
        }
    }

    void CreateGhostPool()
    {
        int needed = maxGhosts - ghostPool.Count;

        for (int i = 0; i < needed; i++)
        {
            GameObject ghost = new GameObject("Ghost");
            SpriteRenderer sr = ghost.AddComponent<SpriteRenderer>();
            sr.sortingLayerID = targetSpriteRenderer != null ? targetSpriteRenderer.sortingLayerID : 0;
            sr.sortingOrder = targetSpriteRenderer != null ? targetSpriteRenderer.sortingOrder - 1 : 0;
            ghost.SetActive(false);
            ghostPool.Enqueue(ghost);
        }
    }

    void SpawnGhost()
    {
        if (ghostPool.Count > 0)
        {
            GameObject ghost = ghostPool.Dequeue();
            SpriteRenderer ghostSR = ghost.GetComponent<SpriteRenderer>();

            ghost.transform.position = targetSpriteRenderer.transform.position;
            ghost.transform.rotation = targetSpriteRenderer.transform.rotation;
            ghost.transform.localScale = targetSpriteRenderer.transform.localScale;
            ghostSR.sprite = targetSpriteRenderer.sprite;
            ghostSR.color = ghostColor;
            ghost.SetActive(true);

            StartCoroutine(DisableAfterTime(ghost, ghostLifetime));
        }
    }

    IEnumerator DisableAfterTime(GameObject ghost, float delay)
    {
        float elapsed = 0f;
        SpriteRenderer sr = ghost.GetComponent<SpriteRenderer>();

        while (elapsed < delay)
        {
            float alpha = Mathf.Lerp(ghostColor.a, 0f, elapsed / delay);
            sr.color = new Color(ghostColor.r, ghostColor.g, ghostColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ghost.SetActive(false);
        ghostPool.Enqueue(ghost);
    }

    // ============================
    // STATIC METHODS
    // ============================

    public static void StartEffect()
    {
        if (_instance == null)
        {
            Debug.LogWarning("GhostEffect: Brak instancji w scenie.");
            return;
        }

        _instance.timer = 0f;
        _instance.isActive = true;
        _instance.CreateGhostPool();
    }

    public static void StopEffect()
    {
        if (_instance == null) return;
        _instance.isActive = false;
    }
}
