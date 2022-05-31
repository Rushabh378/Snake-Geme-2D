using System;
using UnityEngine;

public class TimerManagement
{
    public static TimerManagement setTimer(Action action,float countDown)
    {
        GameObject gameObject = new GameObject("Timer", typeof(MonoBeheviarHook));
        TimerManagement timerManagement = new TimerManagement(action, countDown, gameObject);
        gameObject.GetComponent<MonoBeheviarHook>().onUpdate = timerManagement.Timer;

        return timerManagement;
    }
    //accessing MonoBehaviour
    private class MonoBeheviarHook : MonoBehaviour
    {
        public Action onUpdate;
        private void Update()
        {
            onUpdate?.Invoke();
        }
    }

    private Action action;
    private float countDown;
    private GameObject gameObject;
    private bool destroyed = false;

    private TimerManagement(Action action, float countDown,GameObject gameObject)
    {
        this.action = action;
        this.countDown = countDown;
        this.gameObject = gameObject;
    } 
    private void Timer()
    {
        if(!destroyed)
        {
            //Debug.Log("count down: " + countDown);
            countDown -= Time.deltaTime;
            if (countDown < 0)
            {
                action();
                destroySelf();
            }
        }   
    }
    private void destroySelf()
    {
        destroyed = true;
        UnityEngine.Object.Destroy(gameObject);
    }
}
