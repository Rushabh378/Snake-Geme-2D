using UnityEngine;
using UnityEngine.UI;

public class manuFunctioality : MonoBehaviour
{
    public void onExit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
