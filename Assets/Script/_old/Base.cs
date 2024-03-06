
using UnityEngine;

public class Base : MonoBehaviour {

    [SerializeField] public AudioSet audioLoop;
    [SerializeField] public AudioSet audioShot;
    public bool isUpdate = true;
    public bool isDebug = false;

    /// <summary> deals with addon events and special events from elsewhere </summary>
    public virtual void Actions( string action, float _n = 1f, int _part = 0, int _butt = 0 ) { }
    /// <summary> deals with addon events and special events from elsewhere </summary>
    public virtual void ActionsOut( string action, float _n = 1f, int _part = 0, int _butt = 0 ) { }

    /// <summary> deals with basic internal mouse events </summary>
    public virtual void Mouses( MouseBase from_, MouseEvent mouseEvent, int part, int sub = 0 ) { }

    /// <summary> Filter mouse rays based on machine state </summary>
    public virtual RayResponse IsRayable( int part, int sub ) { return RayResponse.trigger; }


}
