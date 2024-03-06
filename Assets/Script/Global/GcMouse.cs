//using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

    public class Presser {
    public InputMouse type; public string label = "";
    public bool isPressed = false; public bool isReleased = false; public bool isHold = false;
    public bool isTimer = false; public float timeHold = 0f;
    public Vector2 pos; public Vector2 posPressed; public Vector2 posReleased; // public Vector3 PosClick3D;

    public Presser( InputMouse _type, string _label ) {
        type = _type; label = _label;
    }

    public void frameStart( float dt ) {
        isHold = Input.GetButton( label );
        isPressed = Input.GetButtonDown( label );
        isReleased = Input.GetButtonUp( label );
        if ( isPressed ) { isTimer = true; timeHold = 0f; posPressed = Input.mousePosition; };
        if ( isReleased ) { isTimer = false; timeHold = 0f; posReleased = Input.mousePosition; };
        if ( isTimer ) timeHold += dt;
        if ( isHold ) pos = Input.mousePosition;
    }

    public void frameEnd() {
        isPressed = false;
        isReleased = false;
    }
    // public void frameKey( float dt ) {    //if ( Input.GetKeyDown( KeyCode.F ) ) { }
}

public class GcMouse : MonoBehaviour {

    Camera cam;
    public bool isOn = true;

    [SerializeField] User user;
    public float raycastDistance = 2f;

    /// <summary> for each mouse button </summary>
    public Dictionary<InputMouse, Presser> inputMouseDict = new Dictionary<InputMouse, Presser>();

    /// <summary> layers for raycast </summary>
    [SerializeField] List<LayerMask> masks3D;

    /// <summary> for each layer.  quick look up dictionary.  to avoid getcomponent </summary>
    List<Dictionary<GameObject, MouseBase> > layers3D = new List<Dictionary<GameObject, MouseBase>>();

    MouseBase this3d;
    MouseBase last3d;


    void Start() {
        cam = Camera.main;
        // set up each mouse button
        inputMouseDict.Add( InputMouse.left, new Presser( InputMouse.left, GV.mouseL ) );
        inputMouseDict.Add( InputMouse.right, new Presser( InputMouse.right, GV.mouseR ) );
        inputMouseDict.Add( InputMouse.middle, new Presser( InputMouse.middle, GV.mouseM ) );

        foreach ( LayerMask lm in masks3D )
            layers3D.Add( new Dictionary<GameObject, MouseBase>() );
    }

    /// <summary> called from G singleton </summary>
    public void UpdateG( float dtr ) {

        //update each mouse button state
        inputMouseDict[ InputMouse.left ].frameStart( dtr );
        inputMouseDict[ InputMouse.right ].frameStart( dtr );
        inputMouseDict[ InputMouse.middle ].frameStart( dtr );

        if ( isOn ) {

            // send user mouse events
            user.InputPresser( InputMouse.left, inputMouseDict[ InputMouse.left ] );
            if ( !inputMouseDict[ InputMouse.left ].isHold ) {
                user.InputPresser( InputMouse.right, inputMouseDict[ InputMouse.right ] );
                user.InputPresser( InputMouse.middle, inputMouseDict[ InputMouse.middle ] );
            }

            // used for overlay mode to shut out other interactions
            if ( GV.is3dMouse ) {
                // check current layer ffor ray collisions and send events to MouseBase
                if ( GV.mouseRayLayer >= 0 && GV.mouseRayLayer < masks3D.Count ) {
                    // get what's under the mouse on the chosen 3d layer
                    this3d = Get3dMouseBaseHit( GV.mouseRayLayer );

                    Db.I.Set( "layer3dOn", GV.mouseRayLayer.ToString() );
                    Db.I.Set( "this3d", this3d?.name );
                    //send mouse events
                    MouseObjectOverChecks( last3d, this3d );
                    MouseObjectPressChecks( last3d, this3d, inputMouseDict[ InputMouse.left ] );
                    last3d = this3d;
                }
            }
        }
        // reset each mouse button state
        inputMouseDict[ InputMouse.left ].frameEnd();
        inputMouseDict[ InputMouse.right ].frameEnd();
        inputMouseDict[ InputMouse.middle ].frameEnd();
    }

    public void SetMousePosition( Vector2 pos ) {
        Mouse.current.WarpCursorPosition( pos );
    }

    // 3D mouse overs and presses

    /// <summary> Avoids GetComponent by compiling per layer dict </summary>
    private MouseBase MouseObjectDictCheck( Dictionary<GameObject, MouseBase> _dict, GameObject _key ) {
        if ( _dict.ContainsKey( _key ) ) return _dict[ _key ];
        else _dict.Add( _key, _key.GetComponent<MouseBase>() );
        return _dict[ _key ];
    }


    private void MouseObjectOverChecks( MouseBase _last, MouseBase _this ) {
        if ( _last != null && _this != _last ) _last.ExitMe();
        if ( (_last == null || _this != _last) && _this != null ) _this.EnterMe();
        if ( _last != null && _this == null ) G.cursor.SwitchCursorDefault();

    }

    private void MouseObjectPressChecks( MouseBase _last, MouseBase _this, Presser _presser ) {
        if ( _this != null ) {
            if ( _presser.isPressed ) _this.PressMe();
            if ( _presser.isReleased ) _this.ReleaseMe();
        }
    }

    Ray rayTmp;
    private MouseBase Get3dMouseBaseHit( int index ) {
        RaycastHit[] hits3d;
        hits3d = Physics.RaycastAll( GV.mouseRay, raycastDistance, masks3D[ index ] );
        Db.I.Set( "3D hit count", hits3d.Length.ToString() );
        if ( hits3d.Length == 0 ) return null;
        //if ( hits3d.Length == 1 ) return hits3d[ 0 ].transform.gameObject.GetComponent<MouseBase>();

        //return Get3dMouseBaseClosestHit( hits3d ).GetComponent<MouseBase>();
        GameObject gameObj = Get3dMouseBaseClosestHit( hits3d, index );
        if ( gameObj == null ) return null;
        return MouseObjectDictCheck( layers3D[ index ], gameObj );
    }

    private GameObject Get3dMouseBaseClosestHit( RaycastHit[] hits3d, int index ) {
        float lowdist = 10000000;
        GameObject closest = null;
        MouseBase mouseBase;
        RayResponse rayResponse;
        RayResponse rayResponseClosest = RayResponse.trigger;
        foreach ( RaycastHit obj in hits3d ) {
            if ( obj.distance < lowdist ) {
                mouseBase = MouseObjectDictCheck( layers3D[ index ], obj.transform.gameObject );
                rayResponse = mouseBase.IsRayable();

                if ( !mouseBase.isOn ) rayResponse = RayResponse.pass;

                if ( rayResponse != RayResponse.pass ) {
                    lowdist = obj.distance;
                    closest = obj.transform.gameObject;
                    rayResponseClosest = rayResponse;
                }
            }
        }
        if ( rayResponseClosest == RayResponse.block ) return null;
        return closest;
    }
}






















    //RayerEvent rayEvent = new RayerEvent();
/*

public class RayerEvent {
    RayResponse rayResponse;
    MouseBase mouseBase;
    GameObject gameObj;
}

*/


// <summary> which 2D layer to look at </summary>
//int layer2dOn = 0;
//[SerializeField] List<LayerMask> masks2D;
//List<Dictionary<GameObject, MouseBase> > layers2D = new List<Dictionary<GameObject, MouseBase>>();
//ContactFilter2D filter2d = new ContactFilter2D();

//Dictionary<InputKey, Presser> inputKeyDict = new Dictionary<InputKey, Presser>();


//MouseBase this2d;
//MouseBase last2d;

// <summary> which 2D layer to look at </summary>
//public void Set2d( int i = -1 ) { layer2dOn = i; ResetRays(); }
// <summary> which 3D layer to look at </summary>
//public void Set3d( int i = -1 ) { layer3dOn = i; ResetRays(); }
//public void ResetRays() {
/*if ( last2d != null ) last2d.MouseExit();
if ( last3d != null ) last3d.MouseExit();
if ( this2d != null ) this2d.MouseExit();
if ( this3d != null ) this3d.MouseExit();*/
//}





/*

public class MouseObject {
    public MouseBase mouseBase;
    public bool isOver;
    public MouseObject( MouseBase _mouseBase, bool _over = true ) {
        mouseBase = _mouseBase;
        isOver = _over;
    }
}
*/
/*
public class LayerSet {
    public TypeDimension type;
    public List<LayerMask> maskList;

    public LayerSet( TypeDimension _type, List<LayerMask> _maskList ) {
        type = _type;
        maskList = _maskList;
    }
}*/


/*

//Ray rayTmp;
//RaycastHit[] hits3d = new RaycastHit[10];
private MouseBase Get3dMouseBaseHitAlloc( int index ) {
    int hitCount = Physics.RaycastNonAlloc( GV.mouseRay, hits3d, raycastDistance, masks3D[index] );
    Db.I.Set( "3D hit count", hitCount.ToString() );
    if ( hitCount == 0 ) return null;
    return Get3dMouseBaseClosestHit().GetComponent<MouseBase>();
    // return MouseObjectDictCheck( layers3D[ index ], Get3dMouseBaseClosestHit() );
}
*/









/*
// serialised layers for quick look up dictionary
foreach ( LayerMask lm in masks2D )
    layers2D.Add( new Dictionary<GameObject, MouseBase>() );*/
/*
// interaction with just left, may need to expand for other mouse buttons
Presser presser = inputMouseDict[InputMouse.left];

if ( layer2dOn != -1 ) {
    // get what's under the mouse on the chosen 2d layer
    //this2d = Get2dMouseBaseHit( layer2dOn );
    Db.I.Set( "this2d", this2d?.name );
    Db.I.Set( "last2d", last2d?.name );
    //Debug.Log( this2d?.name );
    //send mouse events
    MouseObjectOverChecks( last2d, this2d );
    MouseObjectPressChecks( last2d, this2d, presser );
    last2d = this2d;
}

if ( layer3dOn != -1 ) {
    // get what's under the mouse on the chosen 3d layer
    this3d = Get3dMouseBaseHit( layer3dOn );
    Db.I.Set( "this3d", this3d?.name );
    Db.I.Set( "last3d", last3d?.name );
    //send mouse events
    MouseObjectOverChecks( last3d, this3d );
    MouseObjectPressChecks( last3d, this3d, presser );
    last3d = this3d;
}


*/

/*

// raycasts



RaycastHit2D hit2d;
private MouseBase Get2dMouseBaseHitb( int index ) {

    filter2d.layerMask = masks2D[index];

    hit2d = Physics2D.Raycast( GV.mouseVec, Vector2.zero, Mathf.Infinity, masks2D[index] );
    //hit2d = Physics2D.Raycast( cam.ScreenToWorldPoint( Input.mousePosition ), Vector2.zero );
    //hit2d = Physics2D.GetRayIntersection( cam.ScreenPointToRay( Input.mousePosition ) );
    //Db.I.Set( "hit2d", hit2d.collider?.gameObject.name.ToString() );
    //Db.I.Set( "hit2d", hit2d.collider?.ToString() );
    if ( hit2d.collider == null ) return null;
    return MouseObjectDictCheck( layers2D[index], hit2d.collider.gameObject );
}

List<RaycastResult> overAll2DHits = new List<RaycastResult>{ };
private void Get2dMouseBaseHit() {
    PointerEventData eventData = new PointerEventData(EventSystem.current);
    eventData.position = Input.mousePosition;
    overAll2DHits = new List<RaycastResult>();
    EventSystem.current.RaycastAll( eventData, overAll2DHits );
    //return raycastResults.Any( x => x.gameObject == gameObject );
}




}

*/









/*
private MouseBase OverAll3DB( int index ) {
    rayTmp = cam.ScreenPointToRay( Input.mousePosition );
    hits3d = Physics.RaycastAll( rayTmp, 5f, masks3D[index] );
    //Db.I.Set( "hits3d", hits3d.Length.ToString() );
    if ( hits3d.Length == 0 ) return null;
    return MouseObjectDictCheck( layers3D[index], Get3dMouseBaseClosestHit() );
}*}


/*public void MouseObjectDictReset( Dictionary<GameObject, MouseBase> _dict ) {
 //foreach ( var item in _dict ) item.Value.isOver = false;
}*/


// public string id = "GcMouse";
//List<RaycastHit[]> resultList2D;
//List<List<RaycastResult>> resultList3D;
//public LayerMask mask2D;
//public LayerMask mask3D;
//Dictionary<GameObject, MouseBase> go2Dic = new Dictionary<GameObject, MouseBase>();
// Dictionary<GameObject, MouseBase> go3Dic = new Dictionary<GameObject, MouseBase>();
// = new RaycastHit[0];
//List<RaycastResult> overAll2DHits = new List<RaycastResult>{ };



//if ( inputMouseDict[InputMouse.left] != null ) { }

//Get3dMouseBaseHit();
//Get2dMouseBaseHit();

//Db.I.Set( "3DHits", hits3d.Length.ToString() );
//Db.I.Set( "2DHits", overAll2DHits.Count.ToString() );

/*if ( overAll2DHits.Count > 0 ) {
    foreach ( RaycastResult obj in overAll2DHits )
        Debug.Log( obj.ToString() );
}
if ( hits3d.Length > 0 ) {
    foreach ( RaycastHit obj in hits3d )
        Debug.Log( obj.transform.name.ToString() );
}*/



/*
public bool OverAll3DCheckIn( GameObject go ) {
    if ( hits3d.Length < 1 ) return false;
    foreach ( RaycastHit obj in hits3d )
        if ( obj.transform.gameObject == go ) return true;
    return false;
}
public bool Get3dMouseBaseClosestHit( GameObject go ) {
    if ( hits3d.Length < 1 ) return false;
    float lowdist = 10000000;
    GameObject closest = null;
    foreach ( RaycastHit obj in hits3d )
        if ( obj.distance < lowdist ) closest = obj.transform.gameObject;
    if ( closest == go ) return true;
    return false;
}*/

// 2D //////////////////////////////////////////////////////////

/*
private void OverAll2Db( LayerMask lm ) {
    PointerEventData eventData = new PointerEventData(EventSystem.current);
    eventData.position = Input.mousePosition;
    overAll2DHits = new List<RaycastResult>();
    EventSystem.current.RaycastAll( eventData, lm );
    //return raycastResults.Any( x => x.gameObject == gameObject );
}
public bool OverAll2DCheckIn( GameObject go ) {
    if ( overAll2DHits.Count < 1 ) return false;
    foreach ( RaycastResult obj in overAll2DHits )
        if ( obj.gameObject == go ) return true;
    return false;
}
public bool OverAll2DCheckFirst( GameObject go ) {
    if ( overAll2DHits.Count < 1 ) return false;
    if ( overAll2DHits[0].gameObject == go ) return true;
    return false;
}
*/




/*
public void Over3D() {

    Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit2D hit = Physics2D.Raycast(v, Vector2.zero);
    if ( hit ) {
        //Debug.Log( "card clicked: " + hit.collider.GetComponent<CardPrefabScript>().index );
    }
}

List<RaycastResult> results;
bool Over3D() {
    bool ret = false;
    PointerEventData m_PointerEventData = new PointerEventData( m_EventSystem );
    m_PointerEventData.position = Input.mousePosition;
    results = new List<RaycastResult>();
    m_Raycaster.Raycast( m_PointerEventData, results );
    foreach ( RaycastResult result in results ) {
        if ( result.gameObject == target ) {                //Debug.Log( "Hit " + result.gameObject.name );
            ret = true;
            break;
        }
    }
    return ret;


// from object
private void IsOver3D() {
    retTmp = false;
    rayTmp = cam.ScreenPointToRay( Input.mousePosition );
    if ( coll.Raycast( rayTmp, out RaycastHit hit, 100f ) ) retTmp = true;
    //if ( coll.Raycast( G.I.gVar.rayTmp, out RaycastHit hit, 100f ) ) ret = true;
    return retTmp;
}

private bool IsOver2Dx() {
    retTmp = false;
    rayTmp = Camera.main.ScreenPointToRay( Input.mousePosition );
    RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll (rayTmp, Mathf.Infinity);
    foreach ( var hit in hits ) {
        if ( hit.collider.name == name ) { retTmp = true; }

    }
    return retTmp;
}

}*/

/*
private bool IsMouseOver3D() {
    retTmp = false;
    rayTmp = cam.ScreenPointToRay( Input.mousePosition );
    if ( coll.Raycast( rayTmp, out RaycastHit hit, 100f ) ) retTmp = true;
    //if ( coll.Raycast( G.gVar.rayTmp, out RaycastHit hit, 100f ) ) ret = true;
    return retTmp;
}
private bool IsMouseOver2Dx() {
    retTmp = false;
    rayTmp = Camera.main.ScreenPointToRay (Input.mousePosition);
    RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll (rayTmp, Mathf.Infinity);
    foreach ( var hit in hits ) {
        if ( hit.collider.name == name ) { retTmp = true; }

    }
    return retTmp;
}

public static bool IsMouseOver2D( GameObject gameObject ) {
    PointerEventData eventData = new PointerEventData(EventSystem.current);
    eventData.position = Input.mousePosition;
    List<RaycastResult> raycastResults = new List<RaycastResult>();
    EventSystem.current.RaycastAll( eventData, raycastResults );
    return raycastResults.Any( x => x.gameObject == gameObject );
}
*/









/*

//Returns 'true' if we touched or hovering on Unity UI element.
public bool IsPointerOverUIElement() {
    return IsPointerOverUIElement( GetEventSystemRaycastResults() );
}


//Returns 'true' if we touched or hovering on Unity UI element.
private bool IsPointerOverUIElement( List<RaycastResult> eventSystemRaysastResults ) {
    for ( int index = 0; index < eventSystemRaysastResults.Count; index++ ) {
        RaycastResult curRaysastResult = eventSystemRaysastResults[index];
        if ( curRaysastResult.gameObject.layer == UILayer )
            return true;
    }
    return false;
}


//Gets all event system raycast results of current mouse or touch position.
static List<RaycastResult> GetEventSystemRaycastResults() {
    PointerEventData eventData = new PointerEventData(EventSystem.current);
    eventData.position = Input.mousePosition;
    List<RaycastResult> raysastResults = new List<RaycastResult>();
    EventSystem.current.RaycastAll( eventData, raysastResults );
    return raysastResults;
}
*/


/*
 * 


private bool IsMouseOver() {
ret = false;
Ray rayTmp = cam.ScreenPointToRay( Input.mousePosition );
RaycastHit hit;
if ( Physics.Raycast( rayTmp, out hit, 100f, layerMask ) ) {
    if ( targetObject == hit.collider.gameObject ) ret = true;
}
return ret;
}




GraphicRaycaster m_Raycaster;
PointerEventData m_PointerEventData;
m_Raycaster = GetComponent<GraphicRaycaster>();
    m_EventSystem = GetComponent<EventSystem>();
List<RaycastResult> results;
bool IsMouseOver2() {
    bool ret = false;
    m_PointerEventData = new PointerEventData( m_EventSystem );
    m_PointerEventData.position = Input.mousePosition;
    results = new List<RaycastResult>();
    m_Raycaster.Raycast( m_PointerEventData, results );
    foreach ( RaycastResult result in results ) {
        if ( result.gameObject == target ) {                //Debug.Log( "Hit " + result.gameObject.name );
            ret = true;
            break;
        }
    }
    return ret;
}

enum raycastType {
    physicsRaycast,
    graphicRaycaster
}*/

