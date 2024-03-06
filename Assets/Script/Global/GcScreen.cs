using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GcScreen : MonoBehaviour {
    [SerializeField] int vSync = 0;
    [SerializeField] int targetFPS = 60;


    void Start() {
        //GI = G.I;
    }
    void Awake() {
        QualitySettings.vSyncCount = vSync;
        Application.targetFrameRate = targetFPS;
        //QualitySettings.antiAliasing = 2;
        //if ( Screen.fullScreen )
        //Screen.fullScreen = true;
    }
    /*
    void Update() {
        if (Application.targetFrameRate != targetFPS)
            Application.targetFrameRate = targetFPS;
    }
    */

    public float GetFPS() {
        return 1f / Time.deltaTime;
    }
    public int GetFPSint() {
        return (int)(1f / Time.deltaTime);
    }
    public float GetFPSFactor() {
        float expectedTime = 1 / targetFPS;
        float fps = Time.deltaTime * targetFPS;
        return 1 / Time.deltaTime;
    }

    public static bool IsMouseOverGameWindow { 
        get { return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); } 
    }


    /*
    public void SetCursor( bool cursorState ) {
        if ( cursorState ) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if ( !cursorState ) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }*/
}

