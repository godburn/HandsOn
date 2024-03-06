using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class Val {
    public string value;
    public float timer;
    public Val( string l_val = "-", float l_timer = -1 ) {
        value = l_val;
        timer = l_timer;
    }
}

public class Db : MonoBehaviour {

    Dictionary<string, Val> dic = new Dictionary<string, Val>();
    string asStringKey = "-";
    string asStringVal = "-";
    int dbCount = 0;
    public float fpsAv;

    public bool showDebugger = true;
    [Range(0.1f, 2f)]
    public float updateTime = 0.1f;

    [Header("--- FPS ---")]
    public bool showFPS = true;
    public int targetFPS = 60;

    [Header("--- Size & position ---")]
    [Range(1, 600)]
    public int widthKey = 100;
    [Range(1, 600)]
    public int widthVal = 120;
    [Range(1, 40)]
    public int lineHeight = 18;
    public Vector2 position = new Vector2( 10, 10 );

    [Header("--- Style ---")]
    public Color backgroundCol; // Color.black;
    public GUIStyle debugStyle = new GUIStyle();

    // Rect rectK = new Rect( 0, 0, 0, 0 );
    // Rect rectV = new Rect( 0, 0, 0, 0 );


    //private string[] debugList = new string[10];
    //private int[] debugTimer = new int[10];

    /*private float updateCount = 0;
    private float fixedUpdateCount = 0;
    private float updateUpdateCountPerSecond;
    private float updateFixedUpdateCountPerSecond;
    */
    private static Db _instance;
    public static Db I { get { return _instance; } }
    private void Awake() {
        if ( _instance != null && _instance != this ) {
            Destroy( this.gameObject );
        } else {
            _instance = this;
        }
    }

    private void Start() {
        // backgroundCol.a = 0.5f;
        //dic.Add( "aaaaaaaaaa", new Val( "nnnnnnnnn", -1f ) );
        //dic.Add( "fdhdjhd", new Val( "fdgdhdh", -1f ) );
        //dic.Add( "535735", new Val( "46467487", -1f ) );
        //dic.Add( "temp", new Val( "goooooo", 20f ) );
        //InvokeRepeating( "WriteIR", updateTime, updateTime ); //1s delay, repeat every 1s
        fpsAv = GetFPS( Time.deltaTime );
        StartCoroutine( WriteIR() );
    }

    IEnumerator WriteIR() {
        yield return new WaitForSeconds( updateTime );
        while ( true ) {
            WriteIR2();
            yield return new WaitForSeconds( updateTime );
        }
    }

    void WriteIR2() {
        dbCount = dic.Count;
        asStringKey = ""; // string.Join( Environment.NewLine, dic.Keys );
        asStringVal = ""; // string.Join( Environment.NewLine, dic.Values.value );
        foreach ( var item in dic ) {
            if ( item.Value.timer == -1 ) {
                asStringKey += item.Key + "\n";
                asStringVal += item.Value.value + "\n";
            }
        }

        asStringKey += "\n";
        asStringVal += "\n";

        List<string> removals = new List<string>();
        foreach ( var item in dic ) {
            if ( item.Value.timer > -1 ) {
                item.Value.timer -= updateTime;
                if ( item.Value.timer < 0 ) {
                    removals.Add( item.Key );
                } else {
                    asStringKey += item.Key + "\n";
                    asStringVal += item.Value.value + "\n";
                }
            }
        }

        foreach ( var item in removals ) {
            dic.Remove( item );
        }
    }

    void Update() {
        if ( showFPS || showDebugger ) {
            float fps = GetFPS( Time.deltaTime );
                if ( fpsAv < fps ) fpsAv += 0.5f;
                if ( fpsAv > fps ) fpsAv -= 0.5f;
                Set( "FPS", ((int)fps).ToString() + " / " + ((int)fpsAv).ToString(), -1f );
            
        }


        /*
        for ( int i = 0; i < dbTime.Count; i++ ) {
            if ( dbTime[i] > 0 ) {
                dbTime[i]--;

                if ( dbTime[i] == 0 ) {

                }
            }
        }
        */
    }

    void OnGUI() {
        if ( showFPS || showDebugger ) {
            GUI.backgroundColor = backgroundCol;
            //GUI.backgroundColor = (0f, 0f, 0f, 0.5f);

            float ht = (lineHeight * (dbCount+1));
            Rect rect = new Rect( position.x, position.y, widthKey,  ht);
            GUI.Label( rect, asStringKey, debugStyle );
            rect = new Rect( position.x + widthKey + 5, position.y, widthVal, ht );
            GUI.Label( rect, asStringVal, debugStyle );
            //debugString = "";
            //for ( int i = 0; i < debugList.Length; i++ ) {
            //   debugString += i + ": " + debugList[i] + "\n";
            //}
            //var asString = string.Join(Environment.NewLine, dic);
            //GUI.Label( new Rect( 10, 10, 180, 16 * dic.Count ), asString, debugStyle );
            //set(debugList.Length.ToString(), 0);
        }
    }

    //--------------------------------------------
    /*
    public void set( string txt = "", int pos = 0, int timer = 0 ) {
        dbTxt[pos] = txt;
        if ( timer > 0 ) {
            dbTime[pos] = timer;
        }
    }*/

    public void Set( string l_key, string l_val, float l_timer = -1f ) {
        if ( l_key == "FPS" && showFPS || showDebugger ) {
            if ( dic.ContainsKey( l_key ) ) {
                Val val = dic[ l_key ];
                dic[ l_key ].value = l_val;
                dic[ l_key ].timer = l_timer;
            } else {
                dic.Add( l_key, new Val( l_val, l_timer ) );
            }
        }
    }

    public float GetFPS(float dt) {
        return 1f / dt;
    }

    public int GetFPSint(float dt) {
        return (int)(1f / dt);
    }

    public float GetFPSFactor( float dt ) {
        float expectedTime = 1 / targetFPS;
        float fps = dt * targetFPS;
        return 1 / dt;
    }

}


