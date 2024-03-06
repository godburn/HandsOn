using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UiButton : MonoBehaviour {
    public string id = "UiButton";

    [SerializeField] SoButton button;
    [SerializeField] float alphaThreshold = 0.5f;

    bool isOn = true;
    public bool IsOn { get { return isOn; } set { isOn = value; } }

    ButtonStatus state = ButtonStatus.norm;

    Image overlayImage;
    Image symbolImage;
    Image buttonImage;
    GameObject textObject;
    TextMeshProUGUI textLabel;
    Dictionary<ButtonStatus, Sprite> buttonDict = new Dictionary<ButtonStatus, Sprite>();


    // ----------------------------------------------------------

    void Start() {
        textObject = transform.GetChild( 0 ).gameObject;
        textLabel = textObject.GetComponent<TextMeshProUGUI>();
        if ( textLabel != null ) textLabel.raycastTarget = false;

        buttonImage = GetComponent<Image>();
        buttonImage.raycastTarget = true;
        buttonImage.alphaHitTestMinimumThreshold = alphaThreshold;
        buttonDict.Add( ButtonStatus.norm, button.norm );
        buttonDict.Add( ButtonStatus.over, button.over );
        buttonDict.Add( ButtonStatus.down, button.down );
        buttonDict.Add( ButtonStatus.selected, button.selected );
        buttonDict.Add( ButtonStatus.disabled, button.disabled );

        SetButton( ButtonStatus.norm );
        if ( button.symbols.Count > 0 ) {
            symbolImage = GetNewImage( "symbol" );
            symbolImage.color = buttonImage.color;
            symbolImage.raycastTarget = false;
        }
        if ( button.overlays.Count > 0 ) {
            overlayImage = GetNewImage( "overlay" );
            overlayImage.raycastTarget = false;
        }
        if ( button.labels.Count > 0 ) SetText( 0 );
        overlayImage.DOFade( 0f, 0f );
        SetSymbol( 0 );

    }

    void SetOn( bool on ) {
        //isIn = false;
        //isClick = false;
        isOn = on;
    }

    // ----------------------------------------------------------
    Image GetNewImage( string label ) {
        GameObject childOb = new GameObject( label );
        Image img = childOb.AddComponent<Image>();
        childOb.transform.SetParent( transform, true );

        childOb.transform.localPosition = Vector3.zero;
        childOb.transform.localRotation = Quaternion.identity;
        childOb.transform.localScale = Vector3.one;
        RectTransform rtf = this.gameObject.GetComponent<RectTransform>();
        RectTransform rtt = childOb.GetComponent<RectTransform>();
        rtt.sizeDelta = new Vector2( rtf.sizeDelta.x, rtf.sizeDelta.y );
        Utility.CopyGameObjectProps( this.gameObject, childOb );
        return img;
    }

    public void SetButton( ButtonStatus bs ) {
        buttonImage.sprite = buttonDict[ bs ];
        state = bs;
    }

    public void SetOverlay( int i, bool dir, bool press = false ) {
        //Debug.Log( "SetOverlay " + dir );
        if ( i < button.overlays.Count ) {
            overlayImage.sprite = button.overlays[ i ];
            if ( dir ) {
                overlayImage.DOFade( 0.75f, 0.2f );
            } else {
                if ( press ) overlayImage.DOFade( 0.75f, 0f );
                overlayImage.DOFade( 0f, 0.4f );
            }
        }
    }

    public void SetSymbol( int i ) {
        if ( i < button.symbols.Count ) {
            symbolImage.sprite = button.symbols[ i ];
        }
    }

    public void SetText( int i ) {
        if ( i < button.labels.Count ) {
            textLabel.text = button.labels[ i ];
        } else {
            textObject.SetActive( false );
        }
    }

    public void SwitchState( MouseEvent mouseEvent ) {
        switch ( mouseEvent ) {
            case MouseEvent.enter:                //if ( buttonChange ) 
                SetButton( ButtonStatus.over );
                break;
            case MouseEvent.exit:                //if ( buttonChange )
                SetButton( ButtonStatus.norm );
                break;
            case MouseEvent.press:                //if ( buttonChange ) 
                SetButton( ButtonStatus.down );
                break;
            case MouseEvent.release:                //if ( buttonChange ) 
                SetButton( ButtonStatus.over );
                break;
        }
    }
}


