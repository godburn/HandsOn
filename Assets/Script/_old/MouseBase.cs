
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {

    [Header("MouseBase info")]
    public string id = "MouseBase";
    public bool isTooltip = false;
    public Vector3 tooltipOffset = Vector3.zero;
    public float tooltipTime = 2.5f;
    bool isTooltipOn = false;
    //[TextArea(minLines:1, maxLines:4)]
    //public string description = "none";
    //public bool isDesc = false;

    [Header("MouseBase cursor ")]
    [SerializeField] bool isCursorChange = false;
    [SerializeField] CursorType cursorType = CursorType.arrow;

    [Header("MouseBase connection to Base core")]
    public Base coreTo;

    [Header("MouseBase public runtime")]
    public bool isOn = true;
    public bool isOver = false;
    public bool isHold = false;

    public float timeHold = 0f;
    public float timeOver = 0f;
    public Vector2 pos;
    public Vector2 posPressed;
    public Vector2 posReleased;

    [Header("MouseBase button addon (auto) ")]
    public bool hasButton = false;
    public UiButton button;

    enum TriggerType { point, ray }
    TriggerType triggerType = TriggerType.ray;

    //public bool isDebug = false;

    public void Start() {
        button = gameObject.GetComponent<UiButton>();
        if ( button != null ) {
            triggerType = TriggerType.point;
            hasButton = true;
        }
        StartPass();
        //Debug.Log($"{gameObject.name} : {button?.gameObject.name}");
    }
    public virtual void StartPass() { }

    void Update() {
        if ( isOver ) {
            timeOver += GV.rate;
            if ( isTooltip && !isTooltipOn ) {
                if ( timeOver > tooltipTime ) {
                    isTooltipOn = true;
                    //G.I.uiLabel.Createtooltip( id, gameObject, id, tooltipOffset, 0 );
                   // G.I.ui.TooltipCreate( id, gameObject, id, tooltipOffset, 0 );
                }
            }
        }
        if ( isHold ) {
            timeHold += GV.rate;
            if ( !Input.GetButton( GV.mouseL ) ) ReleaseOutsideMe();
        }
        UpdatePass( GV.rate );
    }
    public virtual void UpdatePass( float _rate ) { }


    void ResetTooltip() {

        isTooltipOn = false;

    }

    /// <summary> Filter mouse rays based on machine state </summary>
    public virtual RayResponse IsRayable() { return RayResponse.trigger; }
    public virtual void Action( string action, float _n = 1f ) { }

    public void SetOn( bool dir = true ) { isOn = dir; ResetMe(); }
    public void ResetMe() { isOver = false; isHold = false; timeOver = 0f; timeHold = 0f; }


    // 2D events, unity generated 

    public void OnPointerDown( PointerEventData eventData ) { if ( isOn && hasButton ) PressMe(); }
    public void OnPointerUp( PointerEventData eventData ) { if ( isOn && hasButton ) ReleaseMe(); }
    public void OnPointerEnter( PointerEventData eventData ) { if ( isOn && hasButton ) EnterMe(); }
    public void OnPointerExit( PointerEventData eventData ) { if ( isOn && hasButton ) ExitMe(); }


    // Basic mouseBase settings, called via 2d unity events and from 3d rays in GcMouse 

    public void PressMe() {
        if ( !isOn ) return;
        isHold = true; timeHold = 0f;
        posPressed = Input.mousePosition;
        sendMouses( MouseEvent.press );
        PressMePass();
        ResetTooltip();
    }
    public void ReleaseMe() {
        if ( !isOn ) return;
        isHold = false; timeHold = 0f;
        posReleased = Input.mousePosition;
        sendMouses( MouseEvent.release );
        ReleaseMePass();
        ResetTooltip();
    }
    public void ReleaseOutsideMe() {
        if ( !isOn ) return;
        isHold = false; timeHold = 0f;
        posReleased = Input.mousePosition;
        sendMouses( MouseEvent.releaseOut );
        ReleaseOutsideMePass();
        ResetTooltip();
    }
    public void EnterMe() {
        if ( !isOn ) return;
        isOver = true;
        sendMouses( MouseEvent.enter );
        timeOver = 0f;
        EnterMePass();
        //G.I.ui.TooltipDestroy( id );
    }
    public void ExitMe() {
        if ( !isOn ) return;
        isOver = false;
        sendMouses( MouseEvent.exit );
        timeOver = 0f;
        ExitMePass();
        ResetTooltip();
    }

    // pass to front

    public virtual void PressMePass() { }
    public virtual void ReleaseMePass() { }
    public virtual void ReleaseOutsideMePass() { }
    public virtual void ExitMePass() { }
    public virtual void EnterMePass() { }

    // called from front
    public virtual void sendMouses( MouseEvent mouseEvent ) {
        if ( isOn && hasButton ) button.SwitchState( mouseEvent );
        if ( isCursorChange ) SwitchMyCursor( mouseEvent, cursorType );
    }

    // BUTTON add on /////////////////////////////////////////////////////

    public void SelectMe() {
        if ( hasButton ) button.SetButton( ButtonStatus.selected );
        SetOn( false );
    }
    public void DisableMe() {
        if ( hasButton ) button.SetButton( ButtonStatus.disabled );
        SetOn( false );
    }
    public void UnLockMe() {
        if ( hasButton ) button.SetButton( ButtonStatus.norm );
        SetOn( true );
    }
    public void LockMe() {
        //if ( hasButton ) button.SetButton( ButtonStatus.norm );
        SetOn( false );
    }



    /// <summary> CURSOR; called from front, which stores the individuated cursor </summary>
    public void SwitchMyCursor( MouseEvent mouseEvent, CursorType _cursorType ) {
        switch ( mouseEvent ) {
            case MouseEvent.enter:
                G.cursor.SwitchCursorEnter( _cursorType, CursorStatus.over );
                break;
            case MouseEvent.exit:
                G.cursor.SwitchCursorDefault();
                break;
            case MouseEvent.press:
                G.cursor.SwitchCursorPress( _cursorType, CursorStatus.down );
                break;
            case MouseEvent.release:
                G.cursor.SwitchCursorRelease( _cursorType, CursorStatus.over );
                break;
            case MouseEvent.releaseOut:
                G.cursor.SwitchCursorReleaseOutside();
                break;
        }
    }

}


