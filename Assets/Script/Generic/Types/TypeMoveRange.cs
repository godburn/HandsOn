using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveRange3d {
    public Vector3 from;
    public Vector3 to;
    public Vector3 dif;
    public Vector3 dir;

    public Quaternion fromR;
    public Quaternion toR;
    public Quaternion difR;

    public bool isSwap = false;

    // extra
    public Vector3 pos;
    public Quaternion rot;
    public float factor = 0f;

    public MoveRange3d( Vector3 _from = new Vector3() , Vector3 _to = new Vector3(), Quaternion _fromR = new Quaternion(), Quaternion _toR = new Quaternion() ) {
        ResetPos( _from, _to , _fromR , _toR );
    }

    public void ResetPos( Vector3 _from, Vector3 _to, Quaternion _fromR = new Quaternion(), Quaternion _toR = new Quaternion() ) {
        pos = _from;
        from = _from;
        to = _to;

        rot = _fromR;
        fromR = _fromR;
        toR = _toR;
        //Debug.Log( "ResetPos from " + fromR );
        //Debug.Log( "ResetPos to " + toR );

        Recalc();
    }

    float GetDir( float a, float b ) {
        if ( a < b ) return 1;
        if ( a > b ) return -1;
        return 0;
    }
    public void Recalc() {
        dif = to - from;
        dir = new Vector3( GetDir( from.x, to.x ), GetDir( from.y, to.y ), GetDir( from.z, to.z ) );
    }

    public void Swap() {
        Vector3 tmpTo = to;
        to = from;
        from = tmpTo;

        Quaternion tmpToR = toR;
        toR = fromR;
        fromR = tmpToR;

        Recalc();
        isSwap = !isSwap;
    }

    public Vector3 FactorPos( float factorX, float factorY, float factorZ ) {
        float xPos = from.x + (dif.x * factorX);
        float yPos  = from.y + (dif.y * factorY);
        float zPos  = from.z + (dif.z * factorZ);

        pos = new Vector3( xPos, yPos, zPos );

        return pos;
    }

    public Quaternion FactorRot( float factor ) {
        // float xRot = fromR.x + (difR.x * factor);
        //float yRot  = fromR.y + (difR.y * factor);
        //float zRot  = fromR.z + (difR.z * factor);

        rot = Quaternion.Lerp( fromR, toR, factor );

        return rot;
    }

}


public class MoveRange2d {
    public Vector2 from;
    public Vector2 to;
    public Vector2 dif;
    public Vector2 dir;

    public float fromR;
    public float toR;
    public float difR;

    public bool isSwap = false;

    // extra
    public Vector2 pos;
    public float rot;
    public float factor = 0f;

    public MoveRange2d( Vector2 _from = new Vector2(), Vector2 _to = new Vector2(), float _fromR = 0, float _toR = 0 ) {
        pos = _from;
        from = _from;
        to = _to;

        rot = _fromR;
        fromR = _fromR;
        toR = _toR;
        Recalc();
    }

    float GetDir( float a, float b ) {
        if ( a < b ) return 1;
        if ( a > b ) return -1;
        return 0;
    }

    public void Recalc() {
        dif = to - from;
        difR = toR - fromR;
        dir = new Vector2( GetDir( from.x, to.x ), GetDir( from.y, to.y ) );
    }

    public void Swap() {
        Vector2 tmpTo = to;
        to = from;
        from = tmpTo;

        float tmpToR = toR;
        toR = fromR;
        fromR = tmpToR;
        Recalc();
        isSwap = !isSwap;
    }


    public Vector2 FactorPos( float factorX, float factorY ) {
        float xPos = from.x + (dif.x * factorX);
        float yPos  = from.y + (dif.y * factorY);

        pos = new Vector2( xPos, yPos );

        return pos;
    }
}
