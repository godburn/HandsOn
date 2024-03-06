using DG.Tweening;
using DG.Tweening.Core.Easing;
//using System;
using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using UnityEngine;
//using Unity.Mathematics;

public class Colourise : MonoBehaviour {
    [SerializeField] List<Color> colourList;
    [SerializeField] List<Renderer> fadeRenderers;
    [SerializeField] bool setAlphaOnStart = true;
    [SerializeField] bool setColourOnStart = false;
    public int startColour = -1;
    public int randomColour = 0;

    public float alphaOn = 0.5f;

    bool isOn = true;
    bool isFlash = false;
    public int flashCount = 1000;

    //float fadeTime = 0.2f;
    float flashTime = 0.3f;

    bool isFlashFade = false;
    float flashFadeTime = 0.3f;
    float flashFadeTopAlpha = 0.5f;
    float flashFadeLowAlpha = 0.2f;

    void Start() {

        //InitTextures();
        if ( setColourOnStart ) {
            if ( startColour == -1 ) startColour = Random.Range( 0, colourList.Count );
            SetColour( colourList[ startColour ] );
        }
        if ( setAlphaOnStart ) SetAlpha( alphaOn );

    }

    public void SetVisible( bool on = true ) {
        foreach ( Renderer rndr in fadeRenderers )
            rndr.enabled = on;
    }

    public void InitTextures() {
        MeshRenderer[] meshRenderers = this.gameObject.GetComponentsInChildren<MeshRenderer>( );
        foreach ( MeshRenderer meshRenderer in meshRenderers ) {
            //List<Material> myMaterials = GetComponent<Renderer>().materials.ToList();
            // foreach ( Material myMaterial in myMaterials ) {
        }
    }

    void SetColour( Color c ) {
        foreach ( Renderer rndr in fadeRenderers ) {
            rndr.materials[ 0 ].color = c;
        }
    }

    void SetAlpha( float a = 1f ) {
        foreach ( Renderer rndr in fadeRenderers ) {
            Color color = rndr.materials[0].color;
            color.a = a;
            rndr.materials[ 0 ].color = color;
        }
    }

    public void SetFadeAlpha( float alpha_, float time_ ) {
        //Debug.Log( "SetFadeAlpha" + fadeRenderers.Count );

        foreach ( Renderer rndr in fadeRenderers )
            rndr.materials[ 0 ].DOFade( alpha_, time_ );
    }





    public void SetOn( bool dir ) {
        if ( !dir ) {
            //txt.DOFade( 0f, fadeTime );
            SetVisible( false );
        } else {
            SetVisible( true );
            //txt.DOFade( 1f, fadeTime );
        }

        isOn = dir;
    }







    // FLASH

    public void SetFlashFadeAlpha( bool dir ) {
        isFlashFade = dir;
        if ( !dir ) {
            KillAllFlashFades();
            SetAlpha( alphaOn );
        } else SetFlashFadeAlphaDown();
    }

    void SetFlashFadeAlphaDown() {
        if ( !isFlashFade ) return;
        SetFadeAlpha( flashFadeLowAlpha, flashFadeTime );
        this.Invoke( () => SetFlashFadeAlphaAlternate( true ), flashFadeTime + 0.05f );
    }

    void SetFlashFadeAlphaUp() {
        if ( !isFlashFade ) return;
        SetFadeAlpha( flashFadeLowAlpha, flashFadeTime );
        this.Invoke( () => SetFlashFadeAlphaAlternate( false ), flashFadeTime + 0.05f );
    }

    public void SetFlashFadeAlphaAlternate( bool dir ) {
        if ( !isFlashFade ) return;
        if ( !dir ) SetFlashFadeAlphaDown();
        else SetFlashFadeAlphaUp();
    }


    void KillAllFlashFades() {
        foreach ( Renderer rndr in fadeRenderers )
            rndr.materials[ 0 ].DOKill();
    }





    Coroutine cr_flash;
    public void SetFlash( bool dir ) {
        if ( !dir ) {
            if ( isFlash ) {
                StopCoroutine( cr_flash );
            }
            SetOn( true );
        } else {
            cr_flash = StartCoroutine( Flasher() );
            SetOn( true );
        }
        isFlash = dir;
    }

    IEnumerator Flasher() {
        for ( int t = flashCount; t >= 0; t -= 1 ) {
            SetOn( !isOn );
            yield return new WaitForSeconds( flashTime );
        }
        isFlash = false;
        SetOn( true );
    }


}
