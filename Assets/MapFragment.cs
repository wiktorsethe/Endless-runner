using UnityEngine;

public class MapFragment : MonoBehaviour
{
    private Vector2 _position;
    private MapGenerator mapGenerator;

    private void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
    }

    public void SetPosition(Vector2 position)
    {
        _position = position;
    }
    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player").transform.position.x > _position.x + 80f)
        {
            mapGenerator.ReturnToPool(this.transform);
        }
    }
}