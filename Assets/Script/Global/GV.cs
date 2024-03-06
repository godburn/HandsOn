using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GV : MonoBehaviour {

    [Header("OPTIONS ")]
    public bool isDebug = true;
    /// <summary> Overall speed factor </summary>
    public float rateFactor = 1f;
    /// <summary> how far away mouse interacts </summary>
    //public float distanceInteract = 2f;

    [Header("VARIABLES ")]
    
    /// <summary> rateFactor * Time.deltaTime </summary>
    public static float rate = 1f;
    /// <summary> clock speed </summary>
    public static float clock = 1f;


    //public Vector2 mousePosition;
    /// <summary> Input.mousePosition </summary>
    public static Vector3 mousePos;
    /// <summary> Input.mousePosition </summary>
    public static Vector2 mousePos2;
    /// <summary> Camera.main.ScreenToWorldPoint( Input.mousePosition )</summary>
    public static Vector3 mouseVec;
    /// <summary> Camera.main.ScreenPointToRay( Input.mousePosition )</summary>
    public static Ray mouseRay;
    public static int mouseRayLayer = 0;

    public static float mouseX = 0f;
    public static float mouseY = 0f;
    public static float keyX = 0f;
    public static float keyY = 0f;

    /// <summary> Switch off 3d mouse interactions </summary>
    public static bool is3dMouse = true;


    [Header("CONSTANTS ")]

    const string strMouseX = "Mouse X";
    const string strMouseY = "Mouse Y";
    const string strKeyX = "Horizontal";
    const string strKeyY = "Vertical";
    public const string mouseL = "Fire1";
    public const string mouseR = "Fire2";
    public const string mouseM = "Fire3";

    public void UpdateG() {
        //if ( isDebug) Db.I.Set( "mouseRayLayer", mouseRayLayer.ToString() );

        rate = rateFactor * Time.deltaTime;

        mousePos = Input.mousePosition;
        //mousePos2 = Input.mousePosition;
        mouseX = Input.GetAxis( strMouseX );
        mouseY = Input.GetAxis( strMouseY );

        keyX = Input.GetAxis( strKeyX );
        keyY = Input.GetAxis( strKeyY );

        mouseVec = Camera.main.ScreenToWorldPoint( mousePos );//Input.mousePosition );

        //rayTmp = Camera.main.ScreenPointToRay( Input.mousePosition );
        //mousePosition = new Vector2( Input.mousePosition.x, Input.mousePosition.y );

        mouseRay = Camera.main.ScreenPointToRay( mousePos );// Input.mousePosition );
    }
}










/*
public float Rate {
    get { return rate * Time.deltaTime; }
    private set { rate = value; }
} // GI.Rate
public float RateFixed {
    get { return rate * Time.fixedDeltaTime; }
    private set { rate = value; }
} // GI.Rate
*/