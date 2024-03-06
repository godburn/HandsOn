using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSendTest : MonoBehaviour {
    void Start() {
        SEvent.dbEvent.Invoke( new DbEventData( "name", "data" ) );
    }
}
