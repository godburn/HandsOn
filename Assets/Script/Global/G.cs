
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class G : MonoBehaviour {
    private static G instance;
    /// <summary>Global Manager Static Instance</summary>
    public static G I {
        get { return instance; }
        private set { instance = value; }
    }
    private void Awake() { CheckStaticInstance(); }
    void CheckStaticInstance() {
        if ( instance == null ) {
            instance = this;
            DontDestroyOnLoad( gameObject );
            RunOnce();
        } else {
            Destroy( gameObject );
        }
    }

    [SerializeField] Hands hands;
    //--------------------------------------------------------------------
    [SerializeField] List<GameObject> toActivate;

    //[SerializeField] UiMenuCore uiMenu;
    //[SerializeField] UiLabelCore uiLabel;
    //[SerializeField] UiInfoCore uiInfo;
    //[SerializeField] UiQuestionCore uiQuestion;
    //[SerializeField] public UiGateway ui;
    [SerializeField] public User user;
    //public CamDirector tour;

    public static float camZoomTime = 0.75f;
    //public static GData data;
    public static GStatus status;

    public static GcScreen screen;
    public static GcScenes scene;
    public static GcCursor cursor;
    public static GcMouse mouse;
    public static GV V;

    public static GMode mode = GMode.none;
    public static GModeOverlay modeOverlay = GModeOverlay.none;

    private static bool isPause = true;
    //public static bool isMenu = false;
    float camFadeTime = 1f;
    GModeOverlay modeOverlayLast = GModeOverlay.menu;

    void RunOnce() {
        //Debug.Log( "*** G singleton RunOnce ***" );

        // setup dotween
        DOTween.Init( false, true, LogBehaviour.ErrorsOnly );

        //Shader.EnableKeyword( "UNITY_UI_CLIP_RECT" );

        foreach ( var item_ in toActivate )
            item_.SetActive( true );

        //data = new GData();
        status = new GStatus();

        V = GetComponent<GV>();
        cursor = GetComponent<GcCursor>();
        screen = GetComponent<GcScreen>();
        scene = GetComponent<GcScenes>();
        mouse = GetComponent<GcMouse>();

        Invoke( "InteractionDelay", 0.1f );

    }
    void InteractionDelay() {
        isPause = false;
       //ModeChange( GMode.tour );
        //ModeChangeOverlay( GModeOverlay.menu, true );
        //ui.MenuOpen();
    }


    public void MouseAction( pickupMouse _mouseEvent, MouseSelector _mouseSelected, float _dragVal = 0f ) {

        hands.MouseAction( _mouseEvent, _mouseSelected, _dragVal );

    }
    ///////////////////////////////////////////////////////////////

    void Update() {

        // temp for video ending
        if ( Input.GetKeyDown( KeyCode.Home ) )
            SceneManager.LoadScene( 0 );


        if ( !isPause ) {
            //Db.I.Set( "G mode", mode.ToString() );
            //Db.I.Set( "G modeOverlay", modeOverlay.ToString() );
            //Db.I.Set( "G modeOverlayLast", modeOverlayLast.ToString() );
            //Db.I.Set( "GV is3dMouse", GV.is3dMouse.ToString() );

            G.V.UpdateG();
            //mouse.UpdateG( GV.rate );

            if ( modeOverlay == GModeOverlay.none ) {
                switch ( mode ) {
                    case GMode.play:
                        //user.UpdateG( GV.rate );
                        break;
                    case GMode.tour:
                        //if ( Input.GetButtonDown( GV.mouseR ) ) tour.CamAnimEnd();
                        //if ( Input.GetButtonDown( fire1 ) ) SetMenu( true );
                        //user.SwitchCamFocusMode( CamFocusMode.last );
                        break;
                }
            }
            CheckEsc();
        }
    }

    void CheckEsc() {
        if ( Input.GetKeyDown( KeyCode.Escape ) ) {
            //ui.MenuToggle();
        }
    }















    // OVERLAYS ////////////////////////////////////////


    public void ModeChangeOverlay( GModeOverlay _modeOverlay, bool _dir ) {

        if ( _dir ) {
            // switch on
            user.SwitchOverlayMode( true );
            GV.is3dMouse = false;

            cursor.SetCursorDefault( CursorType.arrow, CursorStatus.norm );
            cursor.SwitchCursorDefault();
            modeOverlayLast = modeOverlay;
            modeOverlay = _modeOverlay;

        } else {
            // switch off
            Invoke( "ModeChangeOverlayFalseDelay", camZoomTime );
            //_modeOverlay = modeOverlayLast;
        }

    }


    void ModeChangeOverlayFalseDelay() {

        user.SwitchOverlayMode( false );
        GV.is3dMouse = true;
        modeOverlay = modeOverlayLast;

    }




    // TOUR and TRANSITION ////////////////////////////////////////



    public void ModeChange( GMode _mode ) {

        switch ( _mode ) {
            case GMode.play:
                
                ResetPlayerStartFade(); 
                modeOverlayLast = GModeOverlay.none;
                break;
            case GMode.tour:
                //tour.CamSetAllOff();
                //tour.CamRandom();
                break;
        }
        mode = _mode;
    }


    /*
    public void TourFade( bool _dir ) {
        Debug.Log( "G TourFade: " + _dir );
        if ( _dir ) {
            ui.uiMenu.faderBehind.SetFade( 1f, camFadeTime );
        } else {
            ui.uiMenu.faderBehind.SetFade( 0f, camFadeTime );
        }
    }
    */




    void ResetPlayerStartFade() {
        //ui.uiMenu.faderBehind.SetFade( 0f, camFadeTime );
        //ui.uiMenu.faderFront.SetFade( 1f, camFadeTime );
        this.Invoke( () => ResetPlayerDelayedFadeOver(), camFadeTime + 0.2f );
    }

    void ResetPlayerDelayedFadeOver() {
        //camBrain.enabled = false;  // bug work around, cut blending not working
        modeOverlay = GModeOverlay.none;
        //isMenu = false;
        //tour.CamSetAllOff();
        user.ResetPlayer();
        this.Invoke( () => ResetPlayerDelayedStartFadeIn(), 1.5f );

        //ui.uiMenu.RemoveEnterButton();
    }

    void ResetPlayerDelayedStartFadeIn() {
        //G.I.ui.MenuClose();
        // bug work around, cut blending not working
        //ui.uiMenu.faderFront.SetFade( 0f, camFadeTime );
    }


}











/*



    public void SetMenu( bool dir ) {
        if ( dir == true ) {
            ModeChangeOverlay( GModeOverlay.menu );
        } else {
            ModeChangeOverlay( GModeOverlay.none );
        }
        //isMenu = dir;
    }


public void ModeChangeOverlay( bool isLocked = false ) {
    if ( isLocked ) {
        //isMenu = true;
        user.SwitchOverlayMode( true );
        GV.is3dMouse = false;
        cursor.SetCursorDefault( CursorType.arrow, CursorStatus.norm );
        cursor.SwitchCursorDefault();
    } else {
        user.SwitchOverlayMode( false );
        GV.is3dMouse = true;
        //isMenu = false;
    }
}
*/