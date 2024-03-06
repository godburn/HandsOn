//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class InkBlock {
    public bool isOn = true;
    public GameObject go;
    public Image img;
    public float varySpeed;
    //public float pairVarySpeed;
    public float halfDimH;
    public bool faster;
    public int lane;
    //public Vector3 pos = Vector3.zero;
    public Vector3 startPos;
    public Vector3 endPos;
    public Vector3 difPos;
    public Vector3 dir = Vector3.up;
    int animIndex = 0;
    int lastAnimIndex =- 1;
    float posFact = 0f;
    float blurFact;
    float sizeFact;
    public bool fading = false;
    InkMovement from;
    public InkBlock( InkMovement _from, GameObject _go, Vector3 _startPos, Vector3 _endPos, int _lane, bool _faster, float _halfDim ) {
        from = _from;
        go = _go;
        startPos = _startPos;
        endPos = _endPos;
        startPos = _startPos;
        difPos = endPos - startPos;
        lane = _lane;
        faster = _faster;
        img = go.GetComponent<Image>();
        halfDimH = _halfDim;
        //ResetMe( _pairVarySpeed );
    }

    public void ResetMe( float _pairVarySpeed ) {
        //pairVarySpeed = _pairVarySpeed;

        img.DOFade( 1f, 0.1f );

        isOn = true;
        animIndex = 0;
        posFact = 0f;
        blurFact = Random.Range( 0.2f, 1f );
        sizeFact = Random.Range( 0.2f, 0.8f );
        //go.transform.localPosition = startPos;
        if ( faster ) varySpeed = Random.Range( 0.85f, 1f ) * _pairVarySpeed;
        else varySpeed = Random.Range( 0.75f, 0.83f ) * _pairVarySpeed;
        //animIndexAdd = varySpeed * 0.1f;
        WriteMe();
    }

    void DelayedStart() {
        isOn = true;
    }
    public void SetPosAdd( float speed ) {
        //if ( isOn ) {
        posFact += speed * varySpeed;

        if ( !fading && posFact >= 0.9f ) {
            fading = true;
            img.DOFade( 0f, 1.5f );

        }
        

        //go.transform.localPosition += dir * (speed * varySpeed);
        if ( posFact >= 1f ) isOn = false;
        WriteMe();
        //}
    }

    void WriteMe() {
        if ( isOn ) {
            GetNewImage();
            go.transform.localPosition = startPos + (difPos * posFact);
            go.transform.localScale = new Vector3( 1f, from.sizeFact.Evaluate( posFact * sizeFact ), 1f );
        }
    }

    void GetNewImage() {

        animIndex = (int)(from.blurFact.Evaluate( posFact ) * from.m_Textures.Count * blurFact);

        if ( animIndex != lastAnimIndex )
            img.sprite = from.GetImage( animIndex );
        lastAnimIndex = animIndex;
    }
}


public class InkMovement : MonoBehaviour {

    [SerializeField] BaseMach coreTo;

    [SerializeField] public AnimationCurve blurFact;
    [SerializeField] public AnimationCurve sizeFact;
    //[SerializeField] public List<Texture2D> m_Textures2;
    [SerializeField] public List<Sprite> m_Textures;

    [SerializeField] List<GameObject> setA;
    [SerializeField] List<GameObject> setB;

    List<InkBlock> inkA = new List<InkBlock>();
    List<InkBlock> inkB = new List<InkBlock>();

    public float factorSpeed = 0f;
    //[SerializeField] [Range(0f,1f)] float factorSpeed = 0.3f;

    float speed = 0.05f;
    float halfDimW = 0.375f;
    float halfDimH = 0.43f;
    int lanes = 6;
    float spacing;
    public bool isOn = true;

    void Start() {
        spacing = (halfDimW * 2f) / (lanes - 1);

        for ( int i = 0; i < lanes; i++ ) {
            Vector3 vs = new Vector3( (i * spacing) - halfDimW, -halfDimH, 0f );
            Vector3 ve = new Vector3( (i * spacing) - halfDimW, halfDimH+(spacing*1.2f), 0f );
            float pairVarySpeed = Random.Range( 0.5f, 1f );

            inkA.Add( new InkBlock( this, setA[ i ], vs, ve, i, false, halfDimH ) );
            inkB.Add( new InkBlock( this, setB[ i ], vs, ve, i, true, halfDimH ) );
        }
        ResetAll();
    }



    private void ResetAll() {
        for ( int i = 0; i < lanes; i++ ) {
            float pairVarySpeed = Random.Range( 0.5f, 1f );
            inkA[ i ].ResetMe( pairVarySpeed );
            inkB[ i ].ResetMe( pairVarySpeed );
        }
        isOn = false;
        //Invoke( "DelayedStart", 2f );
    }

    public void RestartAll() {

        ResetAll();
        isOn = true;
        Debug.Log( "RestartAll" );
    }

    void DelayedStart() {
        for ( int i = 0; i < lanes; i++ ) {
            inkA[ i ].isOn = true;
            inkB[ i ].isOn = true;
        }
    }

    private void Update() {
        UpdatePos( GV.rate );
    }

    void UpdatePos( float rate ) {
        if ( isOn ) {
            for ( int i = 0; i < lanes; i++ ) {
                inkA[ i ].SetPosAdd( speed * rate * factorSpeed );
                inkB[ i ].SetPosAdd( speed * rate * factorSpeed );
            }
            


            for ( int ii = 0; ii < lanes; ii++ )
                if ( inkA[ ii ].isOn ) return;
            for ( int ii = 0; ii < lanes; ii++ )
                if ( inkB[ ii ].isOn ) return;

            Debug.Log( "hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh" );
            coreTo.Actions( SActions.processEnd );
            //G.I.uiLabel.CreateSubtext( "Switch the power off and remove the gel tray to blue tissue paper", "psuoff", 0.2f );
            
            isOn = false;
            //ResetAll();

        }
    }



    public Sprite GetImage( int _ind ) {

        if ( _ind >= m_Textures.Count ) return m_Textures[ m_Textures.Count - 1 ];
        return m_Textures[ _ind ];

    }
}
