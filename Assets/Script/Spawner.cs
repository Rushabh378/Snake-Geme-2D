using UnityEngine;

public class Spawner : MonoBehaviour
{
    private static BoxCollider2D spawnArea;
    [SerializeField]
    private GameObject food;

    private void Start()
    {
        spawnArea = GetComponent<BoxCollider2D>();
        randomizeFood();
    }
    public static Vector2 randomizer()
    {
        Bounds bounds = spawnArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(x, y);
    }
    private void randomizeFood()
    {
        food.transform.position = randomizer();
        TimerManagement.setTimer(randomizeFood, 5f);
    }
}
