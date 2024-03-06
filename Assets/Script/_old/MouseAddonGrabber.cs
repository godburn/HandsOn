
using UnityEngine;
using Unity.Mathematics;
using DG.Tweening;

public class MouseAddonGrabber : MonoBehaviour {

    [Header("MouseAddonGrabber ")]

    [SerializeField] GameObject sourceObject;
    [SerializeField] GameObject destinationObject;

    [SerializeField] SoGrabCurves curveSo;

    [SerializeField] bool isDragX = true;

    [Header("Audio")]

    [SerializeField] AudioSource audioOut;
    [SerializeField] AudioClip audioOff;
    [SerializeField] AudioClip audioLift;
    [SerializeField] AudioClip audioLay;
    [SerializeField] AudioClip audioBack;


    MoveRange3d move;
    MoveRange2d move2d;

    Curver curverX;
    Curver curverY;
    Curver curverZ;
    Curver curverXadd;
    Curver curverYadd;
    Curver curverZadd;

    MouseBase mouseBase;

    Colourise colouriser;
    bool isColouriser = false;

    public bool isRemoved = false;
    float normFactor = 0f;
    float moveLerp = 0.3f;
    float xAdd = 0f;
    float yAdd = 0f;
    float zAdd = 0f;

    enum Mode { none, move, fly }
    Mode mode = Mode.none;

    void Start() {

        mouseBase = GetComponent<MouseBase>();
        if ( mouseBase != null )
            Debug.LogWarning( $"MouseBase missing : MachSigmaIdentifier :{gameObject.name}" );

        colouriser = gameObject.GetComponent<Colourise>();
        if ( colouriser != null ) isColouriser = true;

        curverX = new Curver( curveSo.xCurve );
        curverY = new Curver( curveSo.yCurve );
        curverZ = new Curver( curveSo.zCurve );
        curverXadd = new Curver( curveSo.xAddCurve );
        curverYadd = new Curver( curveSo.yAddCurve );
        curverZadd = new Curver( curveSo.zAddCurve );

        //moveInit = new MoveRange3d( gameObject.transform.position, destinationObject.transform.position, gameObject.transform.rotation, destinationObject.transform.rotation );
        move = new MoveRange3d( sourceObject.transform.position, destinationObject.transform.position, sourceObject.transform.rotation, destinationObject.transform.rotation );
        move2d = new MoveRange2d();
    }

    public void ChangeCurverSo( SoGrabCurves _gc ) {
        curveSo = _gc;
        curverX.curve = curveSo.xCurve;
        curverY.curve = curveSo.yCurve;
        curverZ.curve = curveSo.zCurve;
        curverXadd.curve = curveSo.xAddCurve;
        curverYadd.curve = curveSo.yAddCurve;
        curverZadd.curve = curveSo.zAddCurve;
    }


    public void ResetPos() {
        move.ResetPos( sourceObject.transform.position, destinationObject.transform.position, sourceObject.transform.rotation, destinationObject.transform.rotation );

    }

    public void ChangeDestination( GameObject go ) {

        destinationObject = go;
        //move.Swap();
        move.isSwap = false;
        move.ResetPos( sourceObject.transform.position, destinationObject.transform.position, sourceObject.transform.rotation, destinationObject.transform.rotation );

    }

    /*
    
    /// <summary> Filter mouse rays based on machine state </summary>
    public override RayResponse IsRayable() {
        if ( mode != Mode.none ) return RayResponse.block;
        return RayResponse.trigger;
    }
    */
    void MoveRelease() {

        mode = Mode.fly;

        if ( normFactor < 0.5f ) {
            DOTween.To( () => normFactor, x => normFactor = x, 0f, 0.5f ).OnComplete( LandMe );
        } else {
            DOTween.To( () => normFactor, x => normFactor = x, 1f, 0.5f ).OnComplete( LandMeSwap );
        };
    }

    void AudioLandMe() {
        if ( audioOut != null ) {
            if ( isRemoved ) audioOut.PlayOneShot( audioLay, 0.3f );
            else audioOut.PlayOneShot( audioBack, 0.5f );
        }
    }
    void LandMe() {
        //MoveMe( 0 );
        //SetModelTransform();
        mode = Mode.none;
        //gameObject.transform.position = move.to;
        AudioLandMe();
    }

    void LandMeSwap() {
        //MoveMe( 1f );
        //SetModelTransform();
        mode = Mode.none;
        //Debug.Log( "SWAP " + isRemoved );
        //gameObject.transform.position = move.to;
        move.Swap();
        curverX.Flip();
        curverY.Flip();
        curverZ.Flip();
        AudioLandMe();
        isRemoved = !isRemoved;
        mouseBase.Action( SActions.grabSwap );
    }

    void MoveMe( float pos ) {
        //pos = Mathf.Clamp( pos, 0f, 1f );

        // ratios
        float xFct = curverX.GetY(pos); //xCurve.Evaluate( pos );
        float yFct  = curverY.GetY(pos);
        float zFct  = curverZ.GetY(pos); //zCurve.Evaluate( pos );

        // additions
        xAdd = curverXadd.GetY( pos ) * curveSo.xAddFactor;
        yAdd = curverYadd.GetY( pos ) * curveSo.yAddFactor;
        zAdd = curverZadd.GetY( pos ) * curveSo.zAddFactor;

        // rotation 
        move.rot = move.FactorRot( pos );
        //Db.I.Set( "Grab MoveMe", $"{xFct.ToString()} {yFct.ToString()} {zFct.ToString()} " );

        move.pos = move.FactorPos( xFct, yFct, zFct );
    }

    public void SetModelTransform() {


        if ( mode == Mode.none ) return;
        //sourceObject.transform.position = Vector3.Lerp( sourceObject.transform.position, move.pos, moveLerp ) + (Vector3.up * yAdd);
        sourceObject.transform.position = move.pos + (Vector3.left * xAdd) + (Vector3.up * yAdd) + (Vector3.forward * zAdd);

        sourceObject.transform.rotation = move.rot;
    }


    //void Update() {
    public void UpdatePass( float _rate ) {

        //Db.I.Set( "Grab mode", mode.ToString() );
        //Db.I.Set( "Grab removed", isRemoved.ToString() );
        //Db.I.Set( "Grab normFactor", normFactor.ToString() );

        switch ( mode ) {
            case Mode.move:
                float pos = 0;

                if ( isDragX ) {
                    pos = SMath.Clamp( GV.mousePos.x, move2d.from.x, move2d.to.x );
                    pos = math.remap( move2d.from.x, move2d.to.x, 0.02f, 0.98f, pos );

                } else {

                    pos = SMath.Clamp( GV.mousePos.y, move2d.from.y, move2d.to.y );
                    pos = math.remap( move2d.from.y, move2d.to.y, 0.02f, 0.98f, pos );

                }


                normFactor = normFactor + ((pos - normFactor) * 0.075f);
                MoveMe( normFactor );
                SetModelTransform();
                //}
                break;

            case Mode.fly:
                MoveMe( normFactor );
                SetModelTransform();
                break;
        }
    }



    public void PressMePass() {

        if ( mode != Mode.none ) return;
        mode = Mode.move;
        normFactor = 0f;

        move2d.from = mouseBase.posPressed;
        move2d.to = Camera.main.WorldToScreenPoint( move.to );

        move2d.Recalc();

        if ( audioOut != null ) {
            if ( isRemoved ) audioOut.PlayOneShot( audioLift, 0.5f );
            else audioOut.PlayOneShot( audioOff, 0.5f );
        }
    }

    public void ReleaseMePass() {
        if ( mode != Mode.move ) return;
        MoveRelease();
    }
    public void ReleaseOutsideMePass() {
        if ( mode != Mode.move ) return;
        MoveRelease();
    }

    public void EnterMePass() {
        if ( mode == Mode.move ) return;
    }

    public void ExitMePass() {
        if ( mode == Mode.move ) return;
    }





    /// <summary> deals with basic internal mouse events </summary>
    public void Mouses( MouseEvent mouseEvent_ ) {
        switch ( mouseEvent_ ) {
            case MouseEvent.enter:
                EnterMePass();
                break;
            case MouseEvent.exit:
                ExitMePass();
                break;
            case MouseEvent.press:
                PressMePass();
                break;
            case MouseEvent.release:
                ReleaseMePass();
                break;
            case MouseEvent.releaseOut:
                ReleaseOutsideMePass();
                break;
        }
    }


}