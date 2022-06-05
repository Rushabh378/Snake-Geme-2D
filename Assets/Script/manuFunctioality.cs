using UnityEngine;


public class manuFunctioality : MonoBehaviour
{
    public void onExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
