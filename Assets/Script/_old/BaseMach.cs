using UnityEngine;

public class BaseMach : Base {

    public bool isWithin = false;
    //[SerializeField] public MachineCore core;
    //[SerializeField] public ProcedureBase procedure;
    //[SerializeField] public SoMachineInfo info;
    //[SerializeField] public AudioSource audioOut;
    //public CoreAddonCamLayers camLayers;

    [SerializeField] public MachineType type;
    public MachineLook look;
    //CoreAddonCamLayers CamLayers;

    /*
    private CoreAddonCamLayers camLayers;
    public CoreAddonCamLayers CamLayers {
        get { return camLayers; } set { camLayers = value; }
    }
    */

    void Start() {
        //CamLayers = gameObject.GetComponent<CoreAddonCamLayers>();
        //if ( CamLayers != null ) CamLayers.SetCoreTo( this );
        StartPass();
    }

    public virtual void StartPass() { }


    /// <summary> when the user moves into range </summary>
    public void ProximityTrigger( bool _isIn ) {
        isWithin = _isIn;
        //G.I.ui.MachineInfoButton( _isIn, this, info );
        ProximityTriggerPass( _isIn );
    }

    public virtual void ProximityTriggerPass( bool _isIn ) {
    }

    public void ChangeCam( MachineLook _look, bool _first = false ) {
        //if ( CamLayers != null ) CamLayers.ChangeCam( _look, _first );
        //look = _look;
    }

    //

}

