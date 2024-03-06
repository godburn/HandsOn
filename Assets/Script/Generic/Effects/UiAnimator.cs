using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using DG.Tweening.Core.Easing;

public class UiAnimator : MonoBehaviour {

    [SerializeField] bool start = true;
    [SerializeField] bool isAnim = true;
    [SerializeField] float animTime = 0.15f;
    [SerializeField] bool isFade = true;
    [SerializeField] float fadeTime = 0.05f;


    [SerializeField] float delayAddIn = 0;
    [SerializeField] float delayAddOut = 0;
    [SerializeField] Ease easeIn = Ease.InCubic;
    [SerializeField] Ease easeOut = Ease.OutCubic;

    [Header("If part of a set")]

    [SerializeField] int delayPos = 0;
    [SerializeField] int delaySet = 0;
    [SerializeField] float delayTime = 0.1f;



    Image image;
    public void Start() {
        if ( isFade ) {
            image = gameObject.GetComponent<Image>();
            if ( image == null ) isFade = false;
        }
            if ( start ) SetIn();
    }

    public void SetIn() {
        gameObject.SetActive( true );
        if ( isAnim ) transform.DOScale( 0f, 0f ).SetEase( easeIn );
        if ( isFade ) image.DOFade( 0f, 0f );
        Invoke( "SetInDelay", (delayTime * delayPos ) + delayAddIn);
    }

    public void SetInDelay() {
        if ( isAnim ) transform.DOScale( 1f, animTime );
        if ( isFade ) image.DOFade( 1f, fadeTime );
    }

    public void SetOut() {
        //transform.DOScale( 0f, 0f );
        Invoke( "SetOutDelay", (delayTime * (delaySet - delayPos)) + delayAddOut );
    }
    public void SetOutDelay() {
        if ( isAnim ) transform.DOScale( 0f, animTime ).SetEase( easeOut ); ;
        if ( isFade ) image.DOFade( 0f, fadeTime );
        Invoke( "SetOutDelay", animTime );
    }

    public void SetInactive() {

        gameObject.SetActive( false );
    }
}