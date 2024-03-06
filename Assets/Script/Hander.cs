using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class Hander : MonoBehaviour {

    //[SerializeField] GameObject grabPos;
    [SerializeField] GameObject centrePosA;
    [SerializeField] GameObject centrePosB;
    [SerializeField] GameObject dropParent;
    [SerializeField] GameObject restPos;
    
    private GameObject sourceObj;
    public AnimationCurve curve;
    private Curver curver;
    private MoveRange3d move;
    private Animator handAnimator;

    //public float xdir = 1;
    //public float ydir = 1;
    public bool isMoving = false;
    public bool isHolding = false;
    public bool isCentre = false;

    private HandMoveMode mode;
    private HandMoveStageManager handMoveStageManager;
    //public List<HandMoveStage> handMoveStageList = new List<HandMoveStage>();


    // Start is called before the first frame update
    void Start() {
        handMoveStageManager = new HandMoveStageManager( this );
        mode = HandMoveMode.wait;
        sourceObj = this.gameObject;
        handAnimator = GetComponent<Animator>();
        curver = new Curver( curve );
        move = new MoveRange3d();
    }

    //public void SetDestination( MouseSelector destination ) {
        //destinObj = destination;
    //}

    void Update() {
        if ( isMoving ) {
            float factor = handMoveStageManager.Frame();
            if ( isMoving ) AnimSet( curver.GetY( factor ) );
        }
    }


    public void AnimSet( float fact ) {
        move.rot = move.FactorRot( fact );
        move.pos = move.FactorPos( fact, fact, fact );
        //sourceObj.transform.position = Vector3.Lerp( sourceObj.transform.position, move.pos, moveLerp ) + (Vector3.up * yAdd);
        transform.position = move.pos;// + sourceObj.transform.position; // + (Vector3.left * xAdd) + (Vector3.up * yAdd) + (Vector3.forward * zAdd);

        transform.rotation = move.rot;
    }



    // from stage manager ////////////////////////////////
    public void LiftThisUp( GameObject _go ) {

        isHolding = true;
         _go.transform.SetParent( sourceObj.transform );
    }
    public void DropThisOff( GameObject _go ) {
        isHolding = false;
        _go.transform.SetParent( dropParent.transform ); 
    }
    public void SetAnimator( string _anim ) {
        //handAnimator.SetBool( _anim, true ); //!ani.GetBool( hms.aniSwitchName ) );
        handAnimator.SetTrigger( _anim );
    }

    public void MoveSet( Vector3 _destPos, Quaternion _destRot) {
        isMoving = true;
        move.ResetPos( sourceObj.transform.position, _destPos, sourceObj.transform.rotation, _destRot );
    }

    public void MoveStop( ) {
        isMoving = false;
    }

    /////////////////////////////////////////
    

    public void SetMouseMove( HandMoveMode _type, MouseSelector _ms, float _speedFactor = 1f ) {
        HandMoveStage hms;
        //HandMoveMode _type;


        switch ( _type ) {
            case HandMoveMode.drop:
                hms = new HandMoveStage( HandMoveMode.drop );
                hms.Move( _ms.myParent.transform.position, _ms.myParent.transform.rotation, 0.1f );
                hms.Anim( "open", 0.75f );
                hms.Lift( HandLiftMode.drop, _ms.gameObject );
                handMoveStageManager.Add( hms );

                hms = new HandMoveStage( HandMoveMode.pick );
                hms.Move( restPos.transform.position, restPos.transform.rotation, 0.1f );
                handMoveStageManager.Add( hms );

                handMoveStageManager.Play();
                break;

            case HandMoveMode.grab:

                mode = HandMoveMode.grab;

                hms = new HandMoveStage( HandMoveMode.grab);
                hms.Move( _ms.pickup.transform.position, _ms.pickup.transform.rotation, 0.1f );
                hms.Anim( "grab", 0.8f );
                hms.Lift( HandLiftMode.lift, _ms.gameObject );
                handMoveStageManager.Add( hms );

                

                hms = new HandMoveStage( HandMoveMode.pick );
                hms.Move( restPos.transform.position, restPos.transform.rotation, 0.1f );

                // hms.Move( centrePos.transform.position, centrePos.transform.rotation, 0.1f );
                //hms.Anim( "pick", 0.8f, _ms.gameObject );
                handMoveStageManager.Add( hms );

                handMoveStageManager.Play();

                break;
            case HandMoveMode.centre:

                hms = new HandMoveStage( HandMoveMode.pick );
                hms.Move( centrePosA.transform.position, centrePosA.transform.rotation, 0.1f * _speedFactor );
                //hms.Anim( "pick", 0.8f, _ms.gameObject );
                handMoveStageManager.Add( hms );

                hms = new HandMoveStage( HandMoveMode.pick );
                hms.Move( centrePosB.transform.position, centrePosB.transform.rotation, 0.1f * _speedFactor );
                //hms.Anim( "pick", 0.8f, _ms.gameObject );
                handMoveStageManager.Add( hms );


                handMoveStageManager.Play();

                break;
            case HandMoveMode.centrePipette:

                hms = new HandMoveStage( HandMoveMode.pick );
                hms.Move( centrePosA.transform.position, centrePosA.transform.rotation, 0.1f * _speedFactor );
                //hms.Anim( "pick", 0.8f, _ms.gameObject );
                handMoveStageManager.Add( hms );

                hms = new HandMoveStage( HandMoveMode.pick );
                hms.Move( centrePosB.transform.position, centrePosB.transform.rotation, 0.1f * _speedFactor );

                handMoveStageManager.Add( hms );


                handMoveStageManager.Play();

                break;

            case HandMoveMode.centrePipetteDrop:



                break;

            case HandMoveMode.tipbin:

                hms = new HandMoveStage( HandMoveMode.tipbin );
                hms.Move( _ms.inGuideA.transform.position, _ms.inGuideA.transform.rotation, 0.1f );
                handMoveStageManager.Add( hms );

                hms = new HandMoveStage( HandMoveMode.tipbin );
                hms.Move( _ms.inGuideB.transform.position, _ms.inGuideB.transform.rotation, 0.1f );
                hms.Anim( "tip_off", 0.2f );

                handMoveStageManager.Add( hms );

                hms = new HandMoveStage( HandMoveMode.tipbin );
                hms.Move( _ms.inGuideB.transform.position, _ms.inGuideB.transform.rotation, 0.05f );

                handMoveStageManager.Add( hms );

                hms = new HandMoveStage( HandMoveMode.tipbin );
                hms.Move( _ms.inGuideA.transform.position, _ms.inGuideA.transform.rotation, 0.1f );

                handMoveStageManager.Add( hms );


                hms = new HandMoveStage( HandMoveMode.rest );
                hms.Move( restPos.transform.position, restPos.transform.rotation, 0.1f );
                handMoveStageManager.Add( hms );


                handMoveStageManager.Play();

                break;

            case HandMoveMode.tipget:

                hms = new HandMoveStage( HandMoveMode.tipget );
                hms.Move( _ms.inGuideA.transform.position, _ms.inGuideA.transform.rotation, 0.1f );
                handMoveStageManager.Add( hms );

                hms = new HandMoveStage( HandMoveMode.tipget );
                hms.Move( _ms.inGuideB.transform.position, _ms.inGuideB.transform.rotation, 0.3f );
                handMoveStageManager.Add( hms );

                hms = new HandMoveStage( HandMoveMode.tipget );
                hms.Move( _ms.inGuideA.transform.position, _ms.inGuideA.transform.rotation, 0.2f );
                handMoveStageManager.Add( hms );

                hms = new HandMoveStage( HandMoveMode.rest );
                hms.Move( restPos.transform.position, restPos.transform.rotation, 0.1f );
                handMoveStageManager.Add( hms );

                handMoveStageManager.Play();
                break;

            case HandMoveMode.pick:

                mode = HandMoveMode.pick;

                hms = new HandMoveStage( HandMoveMode.pick);
                hms.Move( _ms.pickup.transform.position, _ms.pickup.transform.rotation, 0.1f );
                hms.Anim( "pick", 0.6f);
                hms.Lift( HandLiftMode.lift, _ms.gameObject );
                handMoveStageManager.Add( hms );

                hms = new HandMoveStage( HandMoveMode.pick );
                hms.Move( centrePosA.transform.position, centrePosA.transform.rotation, 0.1f );
                //hms.Anim( "pick", 0.8f, _ms.gameObject );
                handMoveStageManager.Add( hms );

                hms = new HandMoveStage( HandMoveMode.pick );
                hms.Move( centrePosB.transform.position, centrePosB.transform.rotation, 0.1f );
                //hms.Anim( "pick", 0.8f, _ms.gameObject );
                handMoveStageManager.Add( hms );

                handMoveStageManager.Play();

                break;

        }
    }

    public void SetMouseMoveBack( HandMoveMode _type, MouseSelector _ms ) {
        HandMoveStage hms;
        handMoveStageManager.Clear();
        ResetAnimTriggers();

        switch ( _type ) {
            case HandMoveMode.grab:

                break;
            case HandMoveMode.pick:


                hms = new HandMoveStage( HandMoveMode.drop );
                hms.Move( centrePosA.transform.position, centrePosA.transform.rotation, 0.1f );
                handMoveStageManager.Add( hms );


                hms = new HandMoveStage( HandMoveMode.drop );
                hms.Move( _ms.myParent.transform.position, _ms.myParent.transform.rotation, 0.1f );
                hms.Anim( "open", 0.75f );
                hms.Lift( HandLiftMode.drop, _ms.gameObject );
                handMoveStageManager.Add( hms );

                hms = new HandMoveStage( HandMoveMode.rest );
                hms.Move( restPos.transform.position, restPos.transform.rotation, 0.1f );
                handMoveStageManager.Add( hms );

                handMoveStageManager.Play();
                break;

            case HandMoveMode.rest:

                hms = new HandMoveStage( HandMoveMode.rest );
                hms.Move( restPos.transform.position, restPos.transform.rotation, 0.1f );
                hms.Anim( "open", 0.75f );
                handMoveStageManager.Add( hms );
                handMoveStageManager.Play();

                break;
            case HandMoveMode.restPipette:

                hms = new HandMoveStage( HandMoveMode.restPipette );
                hms.Move( centrePosA.transform.position, centrePosA.transform.rotation, 0.1f );
                handMoveStageManager.Add( hms );

                hms = new HandMoveStage( HandMoveMode.restPipette );
                hms.Move( restPos.transform.position, restPos.transform.rotation, 0.1f );
                handMoveStageManager.Add( hms );
                handMoveStageManager.Play();

                break;

        }

    }


    public void ResetAnimTriggers() {

        handAnimator.ResetTrigger( "pick" );
        handAnimator.ResetTrigger( "grab" );
        handAnimator.ResetTrigger( "open" );
        handAnimator.SetTrigger( "tip_off" );
    }



    public void PickAmount( float f ) {

        //float f = slider.value;
        //Debug.Log( f );
        handAnimator.SetFloat( "pick_size", f );
        //pipette.SetFloat( "press", f );
    }
    public void PipettePressAmount( float f ) {
        //float f = slider.value;
        //Debug.Log( f );
        handAnimator.SetFloat( "pipette_close", f );
    }

    public void PipetteTip() {
        //Debug.Log( "grab" );
        //hand.SetBool( "grab", !hand.GetBool( "grab" ) );
        handAnimator.SetTrigger( "tip_off" );
        //pipette.SetTrigger( "tip_off" );
    }


    public void LidFlip() {
        //Debug.Log( "grab" );
        //tube.SetBool( "open", !tube.GetBool( "open" ) );
    }

}




/*



            moveCount += hms.aniSpeed;
            if ( moveCount > 1 ) {
                if ( hms.pickup != null ) {
                    hms.pickup.gameObject.transform.SetParent( sourceObj.transform );
                }
                AnimStop();
            }
//if ( mode == mode_.centre ) {
//return;
//}

//if ( moveCount > aniHalfSwitch && !isAniHalfSwitch ) {
//    AnimHalf();
//}

if ( moveCount > hms.aniTrigger && !hms.isTrigger ) {
    hms.isTrigger = true;
    if ( hms.aniSwitchName != "none" ) {
        handAnimator.SetBool( hms.aniSwitchName, true ); //!ani.GetBool( hms.aniSwitchName ) );
    }
}

//isHoldingSwitch = true;
//if ( !isHolding ) {
//    destinObj.gameObject.transform.SetParent( sourceObj.transform );
//} else {
//    destinObj.gameObject.transform.SetParent( dropParent.transform );
// }
//isHolding = !isHolding;







public void AnimHalf() {

        //isAniHalfSwitch = true;
        switch ( mode ) {
            case mode_.grab:
                handAnimator.SetBool( "grab", !handAnimator.GetBool( "grab" ) );
                break;
            case mode_.pick:
                handAnimator.SetBool( "pick", !handAnimator.GetBool( "pick" ) );
                break;
            //case mode_.centre:
               // break;

        }
    }


     public void PipetteGrab( MouseSelector pupr ) {
        //Debug.Log( "grab" );
        //aniHalfSwitch = 0.35f;
        //curver.curve = curve;
        //move = moveObj;
        //aniSpeed = 0.01f;

        //move.ResetPos( sourceObj.transform.position, destinObj.pickup.transform.position, grabPos.transform.rotation, destinObj.pickup.transform.rotation );

        //handAnimator.SetBool( "grab", !handAnimator.GetBool( "grab" ) );


        //handMoveStageList.Add( new HandMoveStage( centrePos.transform.position, centrePos.transform.rotation, "none", 0.02f ) );

    }


public void Drop( MouseSelector pupr ) {
    handMoveStageList.Add( new HandMoveStage( pupr.pickup.transform.position, pupr.pickup.transform.rotation, "pick", 0.02f, 0.8f, pupr.gameObject ) );


    handMoveStageList.Add( new HandMoveStage( centrePos.transform.position, centrePos.transform.rotation, "none", 0.02f ) );

    AnimStart();


}
public void RestPipette( ) {
    handMoveStageList.Add( new HandMoveStage( restPos.transform.position, restPos.transform.rotation, "none", 0.02f ) );

    AnimStart();

}
*/
