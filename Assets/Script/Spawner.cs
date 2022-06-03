using UnityEngine;

public class Spawner : MonoBehaviour
{
    private static BoxCollider2D spawnArea;
    [SerializeField]
    private GameObject food;
    [SerializeField]
    private GameObject poisen;
    [SerializeField]
    private GameObject shield;
    [SerializeField]
    private GameObject doubler;

    private void Start()
    {
        spawnArea = GetComponent<BoxCollider2D>();
        RandomizeFood();
        RandomizePoisen();
        TimerManagement.setTimer(RandomizeShield, 12f,"shieldTimer");
        TimerManagement.setTimer(RandomizeDoubler, 15f,"doublerTimer");
    }
    public Vector2 Randomizer()
    {
        Bounds bounds = spawnArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(x, y);
    }
    private void RandomizeFood()
    {
        food.transform.position = Randomizer();
        TimerManagement.setTimer(RandomizeFood, 5f,"foodTimer");
    }
    public void RandomizePoisen()
    {
        Destroy(GameObject.FindGameObjectWithTag("poisen"));
        Instantiate(poisen);
        poisen.transform.position = Randomizer();
        TimerManagement.setTimer(RandomizePoisen, 6f,"poisenTimer");
    }
    public void RandomizeShield()
    {
        Destroy(GameObject.FindGameObjectWithTag("Shield"));
        Instantiate(shield);
        shield.transform.position = Randomizer();
        TimerManagement.setTimer(RandomizeShield, 12f,"shieldTimer");
    }
    public void RandomizeDoubler()
    {
        Destroy(GameObject.FindGameObjectWithTag("Doubler"));
        Instantiate(doubler);
        doubler.transform.position = Randomizer();
        TimerManagement.setTimer(RandomizeDoubler, 4f, "doublerTimer");
    }
}
