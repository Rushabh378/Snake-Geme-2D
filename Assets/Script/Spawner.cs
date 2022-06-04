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

    private GameObject poisenClone;
    private GameObject shieldClone;
    private GameObject doublerClone;

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
        if(poisenClone != null)
            Destroy(poisenClone);
        poisenClone = Instantiate(poisen);
        poisenClone.transform.position = Randomizer();
        TimerManagement.setTimer(RandomizePoisen, 6f,"poisenTimer");
    }
    public void RandomizeShield()
    {
        if(shieldClone != null)
            Destroy(shieldClone);
        shieldClone = Instantiate(shield);
        shieldClone.transform.position = Randomizer();
        TimerManagement.setTimer(RandomizeShield, 12f,"shieldTimer");
    }
    public void RandomizeDoubler()
    {
        if(doublerClone != null)
            Destroy(doublerClone);
        doublerClone = Instantiate(doubler);
        doublerClone.transform.position = Randomizer();
        TimerManagement.setTimer(RandomizeDoubler, 4f, "doublerTimer");
    }
}
