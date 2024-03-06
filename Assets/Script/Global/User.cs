
using UnityEngine;
using DG.Tweening;
using Cinemachine;

/// <summary> main user movement / look script </summary>

public class User : MonoBehaviour {
    // public string id = "User";

    [Header("CAMERA ")]
    [SerializeField] CinemachineVirtualCamera vcam;
    [SerializeField] GameObject camProxyH;
    [SerializeField] GameObject camProxyV;
    [SerializeField] Ease camZoomEase = Ease.InOutQuad;// .InOutExpo;
    [SerializeField] float camRotEase = 0.1f;
    NumRange camZoomSteps = new NumRange(60f, 60f, 20f, 5f);
    Vector3 camLastPos;
    Quaternion camLastRot;

    /// <summary> machine being interacted with </summary>
    Base machine;
    CinemachineVirtualCamera machineCam;


    [Header("HEIGHT ")]
    [SerializeField] GameObject heightFollow;
    [SerializeField] float height = 1.5f;
    [SerializeField] float heightLow = 1.2f;
    [SerializeField] float heightOffset;
    bool isDuck = false;

    [Header("MOVEMENT ")]
    [SerializeField] Rigidbody moveRigidBody;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float moveForce = 1500f;
    [SerializeField] float moveLerp = 0.1f;
    Vector3 startPos;

    [Header("MOUSE ")]
    [SerializeField] float mouseSensitivity = 500.0f;
    [SerializeField] float mouseLimitHigh = -10f;
    [SerializeField] float mouseLimitLow = 45f;
    float mouseRotateVertical = 0.0f;

    [Header("OTHER ")]
    //[SerializeField] UiFader vignetteImage;
    public UserStatus state = UserStatus.none;


    void Start() {
        vcam.transform.localPosition = Vector3.zero;
        heightOffset = moveRigidBody.transform.position.y;
        startPos = this.transform.position;
        heightFollow.transform.localPosition = new Vector3( 0, height - heightOffset, 0 );// new Vector3( moveRigidBody.transform.position.x, height, moveRigidBody.transform.position.x );


        transform.position = heightFollow.transform.position;
        camProxyV.transform.rotation = vcam.transform.rotation;
        camProxyH.transform.rotation = transform.rotation;
        vcam.m_Lens.FieldOfView = camZoomSteps.val;

        //G.cursor.SwitchCursor( CursorType.arrow, CursorStatus.norm );
        //SCursor.SetCursorMode( true );
        //cam.fieldOfView = camZoomer.val;
        Invoke( "StartDelayed", 0.5f );
    }

    void StartDelayed() {
        SwitchState( UserStatus.none );
    }

    public void UpdateG( float dtr ) {

        Db.I.Set( "User state", state.ToString() );
        if ( state == UserStatus.move || state == UserStatus.point ) {
            CheckDuckKey( dtr );
            UpdateKeyMove( dtr );
        }
        UpdateCamFoV();
    }
    //UserStatus lastState;
    //bool isOverlay
    public void SwitchOverlayMode( bool isOn = false ) {


        if ( isOn ) {
            //lastState = state;
            //state = UserStatus.overlay;
            //SwitchState( UserStatus.machineView );
        } else {
            //state = lastState;
            //state = UserStatus.point;
            //SwitchState( UserStatus.point );
        }

    }
    // INTERACTION //////////////////////////////////

    public void ExitMachineInfo() {
        if ( machine != null ) machine.Actions( SActions.unview );
        //G.I.ui.SubtextDestroy();
    }



    // from gcmouse, all mouse button presses
    public void InputPresser( InputMouse inmouse, Presser presser ) {
        if ( state == UserStatus.zoom ) return;
        if ( !GV.is3dMouse ) return;

        switch ( inmouse ) {
            case InputMouse.right:

                if ( state == UserStatus.move || state == UserStatus.point ) {

                    if ( presser.isPressed ) {
                        state = UserStatus.move;
                        G.cursor.SetCursorOff(); // SCursor.SetCursorMode( false );
                    }
                    if ( presser.isReleased ) {
                        state = UserStatus.point;
                        //SCursor.SetCursorMode( true ); 
                        G.cursor.SetCursorOn( presser.posPressed );
                        //G.cursor.SetCursorOnDelay();
                        //G.mouse.SetMousePosition( presser.posPressed );
                    }

                    if ( state == UserStatus.move ) {
                        if ( presser.isHold ) {
                            float mouseX = GV.mouseX * mouseSensitivity * GV.rate;
                            float mouseY = -GV.mouseY * mouseSensitivity * GV.rate;
                            UpdateRotate( GV.rate, mouseX, mouseY );
                        }
                    } else {
                        UpdateRotate( GV.rate );
                    }

                } else if ( state == UserStatus.machineView ) {
                    if ( presser.isPressed ) {
                        ExitMachineInfo();
                    }
                }
                break;


            case InputMouse.middle:

                if ( state == UserStatus.move || state == UserStatus.point ) {
                    if ( presser.isPressed ) {
                        state = UserStatus.move;
                        G.cursor.SetCursorOff();
                    }
                    if ( presser.isReleased ) {
                        state = UserStatus.point;
                        G.cursor.SetCursorOn( presser.posPressed );
                    }
                    if ( presser.isHold ) {
                        Vector3 forw = transform.forward * GV.rate * -GV.mouseY * moveSpeed * moveForce * 0.5f;
                        Vector3 side = transform.right * GV.rate * -GV.mouseX * moveSpeed * moveForce * 0.5f;
                        UpdateMove( GV.rate, forw, side );
                    } else {
                        UpdateMove( GV.rate, Vector3.zero, Vector3.zero );
                    }
                }
                break;
        }
    }

    void UpdateCamFoV() {
        //if ( state != UserStatus.machineView && GcScreen.IsMouseOverGameWindow ) 

        camZoomSteps.StepOne( -Input.mouseScrollDelta.y );
        //cam.fieldOfView = camZoomSteps.GetLerp( 0.1f );
        vcam.m_Lens.FieldOfView = camZoomSteps.GetLerp( 0.05f );
        if ( machineCam != null ) machineCam.m_Lens.FieldOfView = camZoomSteps.GetLerp( 0.05f );
    }

    void CheckDuckKey( float dtr ) {
        if ( Input.GetKeyDown( KeyCode.F ) ) {
            if ( !isDuck ) {
                isDuck = true;
                heightFollow.transform.DOLocalMoveY( heightLow - heightOffset, 0.4f );
            } else if ( isDuck ) {
                isDuck = false;
                heightFollow.transform.DOLocalMoveY( height - heightOffset, 0.4f );
            }
        }
    }

    void UpdateKeyMove( float dtr ) {
        float fact = 1f;
        //if ( Input.GetKey( KeyCode.LeftShift ) || Input.GetKey( KeyCode.RightShift ) ) fact = 1.5f;
        Vector3 forw = transform.forward * dtr * GV.keyY * moveSpeed * moveForce * fact;
        Vector3 side = transform.right * dtr * GV.keyX * moveSpeed * moveForce * fact;

        UpdateMove( dtr, forw, side );
    }

    void UpdateMove( float dtr, Vector3 forw, Vector3 side ) {
        moveRigidBody.AddForce( forw + side, ForceMode.Force );
        transform.position = Vector3.Lerp( transform.position, heightFollow.transform.position, moveLerp );
    }

    void UpdateRotate( float dtr = 1f, float mouseX = 0f, float mouseY = 0f ) {
        mouseRotateVertical = Mathf.Clamp( mouseRotateVertical += mouseY, mouseLimitHigh, mouseLimitLow );
        camProxyH.transform.Rotate( Vector3.up * mouseX );
        camProxyV.transform.localRotation = Quaternion.Euler( mouseRotateVertical, 0.0f, 0.0f );
        transform.localRotation = Quaternion.Lerp( transform.localRotation, camProxyH.transform.localRotation, camRotEase );
        vcam.transform.localRotation = Quaternion.Lerp( vcam.transform.localRotation, camProxyV.transform.localRotation, camRotEase );
    }

    // STATES ////////////////////////////////////////////////////////////////////////////////////////////////////

    public void ResetPlayer() {
        SwitchState( UserStatus.point );
        vcam.Priority = 20;
        vcam.m_Lens.FieldOfView = camZoomSteps.val;
        vcam.transform.localRotation = camProxyV.transform.localRotation;
        transform.localRotation = camProxyH.transform.localRotation;
    }

    public void SwitchState( UserStatus mode ) {

        switch ( mode ) {
            case UserStatus.move:
                //SCursor.SetCursorMode( false );
                break;
            case UserStatus.point:
                //G.cam.SwitchCamFocusMode( CamFocusMode.none );
                //if ( machine != null ) machine.Actions( SActions.reset );
                break;
            case UserStatus.machineView:
                //if ( machine != null ) machine.Actions( SActions.reset );
                break;
            case UserStatus.zoom:
                //G.cursor.SetCursorOff();
                break;

            default:
                break;
        }
        state = mode;
    }

    //public void SetMachineFocus( Base mach, CinemachineVirtualCamera camTo, CamFocusMode camFocusMode, bool first = false ) { }
    /// <summary> Zoom into preset machine cam positions </summary>
    public void SetMachineFocus( Base _mach, CinemachineVirtualCamera camTo, bool _first = false ) {
        machine = _mach;

        //camZoomSteps.val = camZoomSteps.max;
        if ( machineCam != null ) machineCam.Priority = -200;
        machineCam = camTo;
        machineCam.Priority = 200;

        if ( _first ) {
            camLastPos = Camera.main.transform.position;
            camLastRot = Camera.main.transform.rotation;
        }

        SwitchState( UserStatus.zoom );

        G.cursor.SetCursorOff();
        this.Invoke( () => SwitchCursorOn(), G.camZoomTime + 0.1f );
        this.Invoke( () => SwitchState( UserStatus.machineView ), G.camZoomTime + 0.1f );
    }

    /// <summary> Return to user camera </summary>
    public void SetMachineUnfocus() {

        //camZoomSteps.val = camZoomSteps.max;
        machineCam.Priority = -200;
        machineCam = null;

        SwitchState( UserStatus.zoom );

        G.cursor.SetCursorOff();
        this.Invoke( () => SwitchCursorOn(), G.camZoomTime + 0.1f );
        this.Invoke( () => SwitchState( UserStatus.point ), G.camZoomTime + 0.1f );
    }

    public void SwitchCursorOn() {
        G.cursor.SetCursorOn();

    }

}