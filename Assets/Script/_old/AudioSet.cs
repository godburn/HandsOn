using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AudioSet : MonoBehaviour {

    [SerializeField] bool loop = false;
    [SerializeField] public List<AudioClip> clipList;
    Dictionary<string, AudioClip> clipDict = new Dictionary<string, AudioClip>();

    [ Header("Volume")]
    [SerializeField] float volFactor = 1f;
    [SerializeField] float volStart = 1f;
    [SerializeField] float volMin = 0.1f;
    [SerializeField] float volMax = 1f;

    [ Header("Pitch")]
    [SerializeField] float pitchStart = 1f;
    [SerializeField] float pitchMin = 0.3f;
    [SerializeField] float pitchMax = 2f;

    AudioSource audioOut;
    NumRange rVol = new NumRange( 1f , 1f, 0.5f, 0.1f);
    NumRange rPitch = new NumRange( 1f , 2f, 0.3f, 0.01f);



    void Start() {
        audioOut = gameObject.GetComponent<AudioSource>();
        foreach ( AudioClip clip_ in clipList ) clipDict.Add( clip_.name, clip_ );
        audioOut.loop = loop;
        if ( clipList.Count > 0 )
            audioOut.clip = clipList[ 0 ];

        rVol.val = volStart;
        rPitch.val = pitchStart;
        RangeVol( volMin, volMax );
        RangePitch( pitchMin, pitchMax );

        audioOut.volume = rVol.val;
        audioOut.pitch = rPitch.val;
    }

    public void Stop() {
        audioOut.Stop();
    }

        public void Play() {
        audioOut.volume = rVol.val;
        audioOut.pitch = rPitch.val;
        audioOut.Play();

    }

    public void Play( float _vol, float _pitch ) {
        rVol.val = _vol * volFactor;
        rPitch.val = _pitch;
        Play();
    }

    public void Play( float _vol ) {
        rVol.val = _vol * volFactor;
        Play();
    }


    public void Play( AudioClip _ac, float _vol = 1f ) {
        if ( !loop ) {
            audioOut.PlayOneShot( _ac, _vol * volFactor );
        } else {
            audioOut.clip = _ac;
            Play( _vol );
        }
    }

    public void Play( string _ac, float _vol = 1f ) {
        if ( clipDict[ _ac ] != null ) {
            Play( clipDict[ _ac ], _vol * volFactor );
        } else {
            Debug.LogWarning( $"AudioClip missing : {_ac} : {gameObject.name}" );
        }
    }

    public void Clip( AudioClip _ac ) {
        audioOut.clip = _ac;
    }
    public void Clip( string _ac ) {
        if ( clipDict[ _ac ] != null ) {
            Clip( clipDict[ _ac ] );
        } else {
            Debug.LogWarning( $"AudioClip missing : {_ac} : {gameObject.name}" );
        }
    }


    public void Pitch( float _n = 0f ) {
        rPitch.val = _n;
    }

    public void Volume( float _n = 0f ) {
        rVol.val = _n;
    }

    public void RangePitch( float _min = 0f, float _max = 1f ) {
        rPitch.min = _min;
        rPitch.max = _max;
    }
    public void RangeVol( float _min = 0f, float _max = 1f ) {
        rVol.min = _min;
        rVol.max = _max;
    }

    public void MapPitch( float _val, float _min = 0f, float _max = 1f ) {
        rPitch.val = math.remap( _min, _max, rPitch.min, rPitch.max, _val );
        audioOut.pitch = rPitch.val;
    }

}
