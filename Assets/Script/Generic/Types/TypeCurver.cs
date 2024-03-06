using UnityEngine;

public class Curver {
    public AnimationCurve curve;
    public float xdir = 1;
    public float ydir = 1;
    public Curver( AnimationCurve _curve ) {
        curve = _curve;
    }
    public float GetY( float _x ) {
        _x = SMath.Clamp( _x, 0f, 1f );
        if ( xdir == -1 ) _x = 1 - _x;
        if ( ydir == -1 ) return 1 - curve.Evaluate( _x );
        return curve.Evaluate( _x );
    }
    public void Flip() {
        ydir = 0 - ydir;
        xdir = 0 - xdir;
    }
}
