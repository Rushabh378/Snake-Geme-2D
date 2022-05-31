using UnityEngine;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TimerManagement.setTimer(testingAction, 3f);
    }

    private void testingAction()
    {
        Debug.Log("testing");
    }
}