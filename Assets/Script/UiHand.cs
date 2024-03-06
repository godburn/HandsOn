using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class UiHand : MonoBehaviour {
    // public string id = "UiMenuSubAudio";

    [SerializeField] Slider sliderUIPress;
    [SerializeField] Slider sliderUIPick;
    //[SerializeField] Animator handL;
    //[SerializeField] Animator handR;
    [SerializeField] Animator pipette;
    [SerializeField] Animator tube;

    [SerializeField] Hander handerL;
    [SerializeField] Hander handerR;

    void Start() {
        sliderUIPress.wholeNumbers = false; sliderUIPress.minValue = 0f; sliderUIPress.maxValue = 1f;
        sliderUIPick.wholeNumbers = false; sliderUIPick.minValue = 0f; sliderUIPick.maxValue = 1f;
        sliderUIPress.onValueChanged.AddListener( delegate { SliderPress( sliderUIPress ); } );
        sliderUIPick.onValueChanged.AddListener( delegate { SliderPick( sliderUIPick ); } );


    }
    /// set mixer based on UI slider
    public void SliderPress( Slider slider ) {

        float f = slider.value;
        //Debug.Log( f );
        //handR.SetFloat( "pipette_close", f );

        handerR.PipettePressAmount( f );

        pipette.SetFloat( "press", f );
    }

    public void SliderPick( Slider slider ) {

        float f = slider.value;
        //Debug.Log( f );
        //handL.SetFloat( "pick_size", f );
        //pipette.SetFloat( "press", f );
        handerL.PickAmount( f );
    }

    public void ButtonGrab() {
        //Debug.Log( "grab" );
        //handR.SetBool( "grab", !handR.GetBool( "grab" ));

        //handerR.PipetteGrab();
    }
    public void ButtonTip() {
        //Debug.Log( "grab" );
        //hand.SetBool( "grab", !hand.GetBool( "grab" ) );
        //handR.SetTrigger( "tip_off" );
        handerR.PipetteTip();


        pipette.SetTrigger( "tip_off" );
    }


    public void PositionCentre() {
        //handerR.PositionCentre();
        //handerL.PositionCentre();

    }


    public void ButtonPick() {
        //Debug.Log( "grab" );
        //handL.SetBool( "pick", !handL.GetBool( "pick" ) );
       //handerL.Pick();

    }


    public void ButtonLid() {
        //Debug.Log( "grab" );
        tube.SetBool( "open", !tube.GetBool( "open" ) );
    }

}
