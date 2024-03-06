using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandMoveMode { wait, none, pick, drop, grab, centre, centrePipette, centrePipetteDrop, rest, restPipette, tipget, tipbin };
public enum HandLiftMode { none, lift, drop };
public class HandMoveStage {
    public Vector3 destPos ;
    public Quaternion destRot ;
    public GameObject pickup ;
    public string aniSwitchName = "none";
    public float aniSpeed = 0.05f;
    public float aniTrigger = 0.75f;
    public bool isTrigger = false;
    public HandMoveMode mode = HandMoveMode.wait;
    public HandLiftMode liftMode = HandLiftMode.none;
    public HandMoveStage( HandMoveMode _mode, Vector3 _destPos = new Vector3(), Quaternion _destRot = new Quaternion(), float _aniSpeed = 0.05f ) {
        Move( _destPos, _destRot, _aniSpeed );
    }

    public void Move( Vector3 _destPos = new Vector3(), Quaternion _destRot = new Quaternion(), float _aniSpeed = 0.05f ) {
        destPos = _destPos;
        destRot = _destRot;
        aniSpeed = _aniSpeed;
    }
    public void Anim( string _aniSwitchName = "", float _aniTrigger = 0.75f ) {
        aniSwitchName = _aniSwitchName;
        aniTrigger = _aniTrigger;
    }
    public void Lift( HandLiftMode _liftMode, GameObject _pickup = null ) {
        liftMode = _liftMode;
        pickup = _pickup;
    }

}
public class HandMoveStageManager {

    public List<HandMoveStage> handMoveStageList = new List<HandMoveStage>();
    public float moveCount = 0f;
    public float moveFactor = 15f;
    public HandMoveStage hms;
    public Hander hand;

    public HandMoveStageManager( Hander _hand ) {
        hand = _hand;
    }

    public void Add( HandMoveStage _hms ) {
        handMoveStageList.Add( _hms );
    }

    public float Frame() {
        moveCount += (hms.aniSpeed * Time.deltaTime * moveFactor);

        // check animation trigger
        if ( moveCount > hms.aniTrigger && !hms.isTrigger ) {
            hms.isTrigger = true;
            if ( hms.aniSwitchName != "none" ) {
                hand.SetAnimator( hms.aniSwitchName );
            }
        }

        // check end of movement
        if ( moveCount > 1 ) {
            if ( hms.pickup != null ) {
                if ( hms.liftMode == HandLiftMode.lift ) {
                    hand.LiftThisUp( hms.pickup );
                } else if ( hms.liftMode == HandLiftMode.drop ) {
                    hand.DropThisOff( hms.pickup );
                }
            }
            Next();
        }
        return moveCount;
    }

    private void Next() {
        handMoveStageList.RemoveAt( 0 );
        hand.isMoving = false;
        Play();
    }

    public void Play() {
        moveCount = 0f;
        if ( handMoveStageList.Count > 0 ) {
            hms = handMoveStageList[ 0 ];
            hand.MoveSet( hms.destPos, hms.destRot );
            return;
        }
        hand.MoveStop();
    }
    public void Clear() {
        handMoveStageList.Clear();

    }
}