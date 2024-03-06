using System.Collections.Generic;
using UnityEngine;


public enum CursorType {
    arrow, finger, grab, eye, clock, 
    settings, lid_open, lid_close, vial, pick, 
    spanner, crosshairs, empty, grabup, turn, 
    loader, grabver, grabhor, eyehor, dial, 
    bung, comb, gel
}


public enum CursorStatus {
    norm, over, down, dont
}


/// <summary>
/// Handles all cursor settings and changes
/// </summary>
public class GcCursor : MonoBehaviour {
    // public string id = "GcCursor";

    [SerializeField] public Vector2 hotSpot = new Vector2(20,20); // Vector2.zero; 
    //MouseEvent mouseEvent;
    Dictionary<int, SoCursor> cursorDict = new Dictionary<int, SoCursor>();

    [SerializeField] public List<SoCursor> cursorList;

    public bool lockCursor = false;
    CursorMode cursorMode = CursorMode.ForceSoftware; // Auto;


    CursorType cursorType;
    CursorType storedCursorType;
    //int cursorTypeInt;
    //int storedCursorTypeInt;
    CursorStatus cursorStatus;
    CursorStatus storedCursorStatus;




    private void Start() {
        foreach ( SoCursor cursor in cursorList ) cursorDict.Add( (int)cursor.type, cursor );
        storedCursorType = CursorType.arrow;
        storedCursorStatus = CursorStatus.norm;
        cursorType = CursorType.arrow;
        cursorStatus = CursorStatus.norm;
        SwitchCursorState();
    }

    private void Update() {
        return;
        Db.I.Set( "lockCursor", lockCursor.ToString() );
        Db.I.Set( "storedCursorType", storedCursorType.ToString() );
        Db.I.Set( "storedCursorStatus", storedCursorStatus.ToString() );
        Db.I.Set( "cursorType", cursorType.ToString() );
        Db.I.Set( "cursorStatus", cursorStatus.ToString() );


    }


    // ON OFF SWITCHING

    void SetCursorOnDelay() {
        SCursor.SetCursorMode( true );
        Cursor.SetCursor( cursorDict[ (int)CursorType.empty ].norm, hotSpot, cursorMode );
        Invoke( "SetCursorOnDelayEnd", 0.1f );
    }
    void SetCursorOnDelayEnd() {
        cursorStatus = CursorStatus.norm;
        SwitchCursorState();
    }


    Vector2 mousePosForOff;
    public void SetCursorOff() { SetCursorOff(GV.mousePos2); }
    public void SetCursorOff( Vector2 mousePos ) {
        mousePosForOff = mousePos;
        SCursor.SetCursorMode( false );
    }

    public void SetCursorOn() { SetCursorOn( mousePosForOff ); }
    public void SetCursorOn( Vector2 mousePos ) {
        //G.mouse.SetMousePosition( mousePosForOff );
        //SCursor.SetCursorMode( true );
        SetCursorOnDelay();
        G.mouse.SetMousePosition( mousePos );
    }

    

    //public void MenuLockMode(bool _isOn = true ) {

    //}

    // CURSOR IMAGE



    public void SetCursorDefault( CursorType type_, CursorStatus status_ ) {
        //lockCursor = false;
        cursorType = type_;
        cursorStatus = status_;
        storedCursorType = type_;
        storedCursorStatus = status_;
        SwitchCursorState();
    }

    public void SwitchCursorDefault() {
        cursorType = storedCursorType;
        cursorStatus = storedCursorStatus;
        if ( lockCursor ) return;
        SwitchCursorState( );
    }





    public void SwitchCursorPress( CursorStatus status_ ) { 
        SwitchCursorPress( cursorType, status_ ); 
    }
    public void SwitchCursorPress( CursorType type_, CursorStatus status_ ) {//, bool lock_ = false) {
        lockCursor = true;
        cursorType = type_;
        cursorStatus = status_;
        SwitchCursorState( );
    }



    public void SwitchCursorRelease( CursorStatus status_ ) { 
        SwitchCursorRelease( cursorType, status_ ); 
    }
    public void SwitchCursorRelease( CursorType type_, CursorStatus status_ ) {
        lockCursor = false;
        cursorType = type_;
        cursorStatus = status_;
        SwitchCursorState( );
    }



    public void SwitchCursorReleaseOutside() {
        lockCursor = false;
        SwitchCursorState( );
    }

    //public void SwitchCursorExit() {
    //    if ( lockCursor ) return;
    //}



    public void SwitchCursorEnter( CursorStatus status_ ) { 
        SwitchCursorEnter( cursorType, status_ ); 
    }
    public void SwitchCursorEnter( CursorType type_, CursorStatus status_ ) {//, bool reset_ = false

        //Debug.Log( type_ + " : " + status_ );
        cursorType = type_;
        cursorStatus = status_;
        if ( lockCursor ) return;
        SwitchCursorState( );
    }


    void SwitchCursorState(  ) {



        switch ( cursorStatus ) {
            case CursorStatus.norm:
                Cursor.SetCursor( cursorDict[ (int)cursorType ].norm, hotSpot, cursorMode );
                break;
            case CursorStatus.over:
                Cursor.SetCursor( cursorDict[ (int)cursorType ].over, hotSpot, cursorMode );
                break;
            case CursorStatus.down:
                Cursor.SetCursor( cursorDict[ (int)cursorType ].down, hotSpot, cursorMode );
                break;
            case CursorStatus.dont:
                Cursor.SetCursor( cursorDict[ (int)cursorType ].norm, hotSpot, cursorMode );
                break;
            default:
                break;
        }
    }
}




/*
void SetCursorImage( Texture2D cursorTexture ) {
    Cursor.SetCursor( cursorTexture, hotSpot, CursorMode.ForceSoftware );
}
public void ResetCursorImage() {
    Cursor.SetCursor( null, Vector2.zero, CursorMode.Auto );
}
*/