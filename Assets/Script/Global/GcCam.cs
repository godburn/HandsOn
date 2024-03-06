using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GcCam : MonoBehaviour {

    [SerializeField] Volume globalVolume;
    public float camZoomTime = 0.75f;
    private DepthOfField ppDepthOfField;
    private CamFocusMode camFocusModeLast = CamFocusMode.none;
    private CamFocusMode camFocusMode = CamFocusMode.none;

    void Start() {

        if ( globalVolume ) 
            if ( globalVolume.sharedProfile.TryGet<DepthOfField>( out ppDepthOfField ) )
                SwitchCamFocusMode( CamFocusMode.none );
    }

    public void SwitchCamFocusMode( CamFocusMode _camFocusMode, float t = 1f ) {

        if ( _camFocusMode == CamFocusMode.last ) {
            SwitchCamFocusMode( camFocusModeLast );
        } else {
            camFocusModeLast = camFocusMode;
            camFocusMode = _camFocusMode;
            float _toFocusDistance = 0f;
            float _toFocalLength = 0f;
            float _toApertureLength = 3f;
            switch ( _camFocusMode ) {

                case CamFocusMode.none:
                    _toFocusDistance = 1f;
                    _toFocalLength = 15f;
                    break;
                case CamFocusMode.machine:
                    _toFocusDistance = 0.35f;
                    _toFocalLength = 18f;
                    break;
                case CamFocusMode.mid:
                    _toFocusDistance = 0.35f;
                    _toFocalLength = 18f;
                    break;
                case CamFocusMode.menu:
                    _toFocusDistance = 0.1f;
                    _toFocalLength = 13f;
                    break;
                case CamFocusMode.close:
                    _toFocusDistance = 0.16f;
                    _toFocalLength = 18f;
                    break;
                case CamFocusMode.dynamic:
                    break;
                case CamFocusMode.tour:
                    _toFocusDistance = 2.5f;
                    _toFocalLength = 45f;
                    break;
            }

            DOTween.To( () => ppDepthOfField.focusDistance.value, x => ppDepthOfField.focusDistance.value = x, _toFocusDistance, 0.5f );//.OnComplete( SetCamFocusValues );
            DOTween.To( () => ppDepthOfField.focalLength.value, x => ppDepthOfField.focalLength.value = x, _toFocalLength, 0.5f );
            DOTween.To( () => ppDepthOfField.aperture.value, x => ppDepthOfField.aperture.value = x, _toApertureLength, 0.5f );
        }
    }

    public void SetFocus( float toFocusDistance, float toFocalLength, float toAperture) {
        ppDepthOfField.focusDistance.value = toFocusDistance;
        ppDepthOfField.focalLength.value = toFocalLength;
        ppDepthOfField.aperture.value = toAperture;
    }

}
