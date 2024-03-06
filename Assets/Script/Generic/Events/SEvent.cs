using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary> Generic event stuff.  Not used </summary>
public static class SEvent {
    public static DbEvent dbEvent = new DbEvent();
}

public class DbEvent : UnityEvent<DbEventData> { }

public class DbEventData {
    public string name; public string data; public float time;
    public DbEventData( string name, string data, float time = -1) {
        this.name = name; this.data = data; this.time = time;
    }
}

/*

    // on send object
    SEvent.dbEvent.Invoke(new DbEventData("name", "data"));

void Awake() {
    // on recieve object
    SEvent.dbEvent.AddListener(DbEventReceive);
}

void DbEventReceive(DbEventData data) {
   Debug.log( "DbEventReceive name: " + data.name );
   Debug.log( "DbEventReceive data: " + data.data );
}

*/
