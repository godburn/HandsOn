using UnityEngine;
using static EnMach;

public class MachGenericFront : MouseBase {
    // public string id = "MachCentrifugeFront : MouseBase";

    [Header("MachGenericFront specialise ")]

    [SerializeField] Part part;
    [SerializeField] Butt type = Butt.none;

    //[Header("MachCentrifugeFront cursor ")]
    //[SerializeField] bool isCursorChange = false;
    //[SerializeField] CursorType cursorType = CursorType.arrow;



    public override void StartPass() {
        //base.Start();
    }
    public override void UpdatePass( float _rate) {
        //base.Update();
    }




    // distribute mouseEvent
    public override void sendMouses( MouseEvent mouseEvent ) {
        base.sendMouses( mouseEvent );
        //if ( isCursorChange ) SwitchMyCursor( mouseEvent, cursorType );
        //coreTo.Mouses( this, mouseEvent, (int)part, (int)type );

    }

    /// <summary> Filter mouse rays based on machine state </summary>
    //public override RayResponse IsRayable() {
    //    return coreTo.IsRayable( (int)part, (int)type );
    //}

    // generic action function
    public override void Action( string action, float _n = 1f ) {
        coreTo.Actions( action, _n, -1, -1 );
    }
}

