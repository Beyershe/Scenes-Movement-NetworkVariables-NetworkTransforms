using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Application.isEditor){
        }
        else
        {
            //Turn off stack trace for debug.log
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
        }
    }

    private void OnGUI()
    {
        NetworkHelper.GUILayoutNetworkControls();
    }

}
