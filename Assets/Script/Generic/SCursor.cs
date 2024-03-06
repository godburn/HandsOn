using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
public enum MouseStatus2 {
    normal, over, leave, down, up, dragIn, dragOut, dragInUp, hold, click
}*/

/// <summary> Generic cursor stuff </summary>
public static class SCursor {

    //public Texture2D cursorTexture;
    //public static CursorMode cursorMode = CursorMode.ForceSoftware .Auto;
    public static Vector2 hotSpot = new Vector2(20,20);
    public static bool isLocked = false;
    public static void SetCursorImage( Texture2D cursorTexture ) {
        Cursor.SetCursor( cursorTexture, hotSpot, CursorMode.ForceSoftware );
    }

    public static void ResetCursorImage() {
        Cursor.SetCursor( null, Vector2.zero, CursorMode.Auto );
    }

    public static void SetCursorMode( bool _state ) {
        if ( _state ) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isLocked = false;
        } else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            isLocked = true;
        }
    }
}
