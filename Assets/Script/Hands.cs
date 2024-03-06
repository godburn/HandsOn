using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//public enum tool { none, pipette };
public class Hands : MonoBehaviour
{


    [SerializeField] Hander handerR;
    [SerializeField] Hander handerL;
    public MouseSelector inHandR = null;
    public MouseSelector inHandL = null;

    public pickupType isHandR = pickupType.none;
    public pickupType isHandL = pickupType.none;


    [SerializeField] Animator pipette;

    public float plungerAmountStart = 0f;
    public float plungerAmount = 0f;
    public float plungerAmountAim = 0f;

    //[SerializeField] Animator animatorL;
    //[SerializeField] Animator animatorR;
    //[SerializeField] GameObject centrePos;
    // Start is called before the first frame update
    void Start()    {
        handerL.PickAmount( 0.8f );

    }
    void Update() {
        PlungerAnimate();
    }


    // distribute mouse events to left and right hand
    public void MouseAction( pickupMouse _mouseEvent, MouseSelector _mouseSelected, float _dragVal ) {

        if ( _mouseEvent != pickupMouse.drag ) {
            Debug.Log( "OnMouse " + _mouseEvent + " " + _mouseSelected.type + " " + _mouseSelected.name );// + " " + _dragVal );
        }

        if ( _mouseEvent == pickupMouse.down ) {
            switch ( _mouseSelected.type ) {
                case pickupType.pipette:

                    if ( isHandR == pickupType.none ) {
                        inHandR = _mouseSelected;
                        isHandR = _mouseSelected.type;
                        handerR.SetMouseMove( HandMoveMode.grab, _mouseSelected );
                    }
                    break;

                case pickupType.container:

                    if ( isHandR == pickupType.pipette ) {
                        inHandL = _mouseSelected;
                        isHandL = _mouseSelected.type;
                        handerR.SetMouseMove( HandMoveMode.centrePipette, inHandR, 0.7f );
                        handerL.SetMouseMove( HandMoveMode.pick, _mouseSelected );
                    }
                    break;


                case pickupType.bin:

                    if ( isHandR == pickupType.pipette ) {
                        handerR.SetMouseMove( HandMoveMode.tipbin, _mouseSelected, 1f );
                    }
                    break;

                case pickupType.tip:

                    if ( isHandR == pickupType.pipette ) {
                        handerR.SetMouseMove( HandMoveMode.tipget, _mouseSelected, 1f );
                    }
                    break;

                case pickupType.pipetteRack:

                    if ( isHandR == pickupType.pipette ) {
                        handerR.SetMouseMove( HandMoveMode.drop, inHandR, 1f );
                        inHandR = null;
                        isHandR = pickupType.none;
                    }

                    break;
            }




        } else if ( _mouseEvent == pickupMouse.up ) {


            if ( isHandR == pickupType.pipette ) {
                PlungerReset();
                switch ( _mouseSelected.type ) {

                    case pickupType.container:

                        if ( !handerL.isHolding ) {
                            handerL.SetMouseMoveBack( HandMoveMode.rest, _mouseSelected );
                        } else {
                            handerL.SetMouseMoveBack( HandMoveMode.pick, _mouseSelected );
                        }
                        inHandL = null;
                        isHandL = pickupType.none;
                        handerR.SetMouseMoveBack( HandMoveMode.restPipette, inHandR );
                        break;
                }
            }
        } else if ( _mouseEvent == pickupMouse.drag ) {

            if ( isHandR == pickupType.pipette ) {
                plungerAmountAim = _dragVal;
            }
        }


    }

    //handerR.RestPipette();
    //handerL.Drop( _mouseSelected );

    public void PlungerAnimate() {
        plungerAmount = plungerAmount + ((plungerAmountAim - plungerAmount) * 0.2f);
        PlungerSet( plungerAmount );
    }

    public void PlungerSet( float _drag ) {
        handerR.PipettePressAmount( _drag );
        pipette.SetFloat( "press", _drag );
    }

    public void PlungerReset( ) {
        plungerAmountAim = 0f;
    }



 }




