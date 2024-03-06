using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventReceiveTest : MonoBehaviour {
    void Awake() {
        SEvent.dbEvent.AddListener( DbEventReceive );
    }
    void DbEventReceive( DbEventData data ) {
        Debug.Log( "------------------" );
        Debug.Log( "DbEventReceive name: " + data.name );
        Debug.Log( "DbEventReceive data: " + data.data );
    }
}
