using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary> mostly for LCD elements </summary>

public class ScreenElement : MonoBehaviour {

    public TextMeshProUGUI txt;
    public Image img;
    public int pad = 2;
    public char padChar = '0';

    bool isOn = true;
    bool isFlash = false;
    public int flashCount = 20;

    float fadeTime = 0.2f;
    float flashTime = 0.4f;

    void Start() {
        if ( txt == null ) txt = gameObject.GetComponent<TextMeshProUGUI>();
        if ( img == null ) img = gameObject.GetComponent<Image>();
    }

    public void SetText( string text ) {
        if ( txt != null ) txt.text = text.PadLeft( pad, padChar );
    }
    public void SetFloatToInt( float num ) {
        if ( txt != null ) txt.text = Mathf.Floor( num ).ToString(); ;
    }
    public void SetTime( float secs ) {
        if ( txt != null ) {
            //string str = 
            //SetText( str.PadLeft( pad, ' ' ) );
            int mins = (int)Mathf.Floor(secs / 60);
            secs = (int)Mathf.Floor( secs % 60 );
            //txt.text = mins.ToString() + " " + secs.ToString();
            SetText( mins.ToString() + " " + secs.ToString() );
        }
    }
    public void SetTimeSec( float secs ) {
        if ( txt != null ) {
            secs = (int)Mathf.Floor( secs % 60 );
            SetText( secs.ToString() );
            //txt.text = secs.ToString();
        }
    }
    public void SetTimeMin( float mins ) {
        if ( txt != null ) {
            mins = (int)Mathf.Floor( mins / 60 );
            SetText( mins.ToString() );
            txt.text = mins.ToString();
        }
    }
    public void SetOn( bool dir ) {
        if ( txt != null ) {
            if ( !dir ) {
                txt.DOFade( 0f, fadeTime );
            } else {
                txt.DOFade( 1f, fadeTime );
            }
        }

        if ( img != null ) {
            if ( !dir ) {
                img.DOFade( 0f, fadeTime );
            } else {
                img.DOFade( 1f, fadeTime );
            }
        }
        isOn = dir;
    }

    Coroutine cr_flash;
    public void SetFlash( bool dir ) {
        if ( !dir ) {
            if ( isFlash ) {
                StopCoroutine( cr_flash );
            }
            SetOn( true );
        } else {
            flashCount = 20000;
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