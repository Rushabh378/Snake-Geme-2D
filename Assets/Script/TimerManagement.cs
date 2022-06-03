using System;
using System.Collections.Generic;
using UnityEngine;

public class TimerManagement
{
    private static List<TimerManagement> activeTimerList;
    private static GameObject timerInit;

    private static void initilize()
    {
        if(timerInit == null)
        {
            timerInit = new GameObject("TimerInit");
            activeTimerList = new List<TimerManagement>();
        }
    }
    public static TimerManagement setTimer(Action action,float countDown, string timerName="Timer")
    {
        initilize();
        GameObject gameObject = new GameObject(timerName, typeof(MonoBeheviarHook));
        TimerManagement timerManagement = new TimerManagement(action, countDown, gameObject,timerName);
        gameObject.GetComponent<MonoBeheviarHook>().onUpdate = timerManagement.Timer;

        activeTimerList.Add(timerManagement);
        return timerManagement;
    }
    private static void removeTimer(TimerManagement timerManagement)
    {
        initilize();
        activeTimerList.Remove(timerManagement);
    }
    public static void cancelTimer(string timerName)
    {
        for(int select = 0;select < activeTimerList.Count; select++)
        {
            if (activeTimerList[select].timerName == timerName)
                activeTimerList[select].destroySelf();
        }
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
    private string timerName;
    private bool destroyed = false;

    private TimerManagement(Action action, float countDown,GameObject gameObject, string timerName)
    {
        this.action = action;
        this.countDown = countDown;
        this.gameObject = gameObject;
        this.timerName = timerName;
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
        removeTimer(this);
        UnityEngine.Object.Destroy(gameObject);
    }
}
