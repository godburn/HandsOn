using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Generic float with range settings
/// </summary>

public class NumRange {

    public float max;
    public float min;
    /// <summary> Resolution, # of divisions </summary>
    public float res;

    /// <summary> Difference </summary>
    public float dif;
    /// <summary> One step </summary>
    public float one;
    /// <summary> -1 if min > max </summary>
    public int dir;

    /// <summary> Current stepped value </summary>
    private float _val;
    /// <summary> Current stepped value </summary>
    public float val {
        get { return _val; }
        set { _val = Mathf.Clamp( value, min, max ); }
    }

    private float _valT;
    /// <summary> Extra value with no functionality </summary>
    public float valT {
        get { return _valT; }
        set { _valT = value; }// Mathf.Clamp( value, min, max ); }
    }
    /// <summary> One step for extra value </summary>
    public float step = 1f;

    public float valLerp;
    /// <summary> Generic float with range settings.  RES can be used as a simple +/- step or a resolution step</summary>
    public NumRange( float _val, float _max, float _min = 0f, float _res = 100f ) {
        min = _min;
        max = _max;
        res = _res; 
        Recalculate();
        val = _val; // dir == 1 ? l_min : l_max;
    }
    /// <summary> recalcs dif and step </summary>
    public void Recalculate() {
        dir = max > min ? 1 : -1;
        dif = SMath.FindDifference( min, max );
        one = dif / res;
        Clamp();
    }
    /// <summary> send a normalised float </summary>
    public float FactIn( float l_n ) {
        return min + (dif * l_n);
    }
    /// <summary> returns a normalised float </summary>
    public float FactOutFromIn( float l_n ) {
        l_n = Mathf.Clamp( l_n, min, max );
        return (l_n - min) / dif;
    }
    /// <summary> returns a normalised float </summary>
    public float FactOut() {
        float l_n = Mathf.Clamp( val, min, max );
        return (l_n - min) / dif;
    }
    /// <summary> returns a normalised float with max min</summary>
    public float FactOutMinMax( float l_max, float l_min ) {
        float l_n = Mathf.Clamp( val, l_min, l_max );
        return (l_n - min) / dif;
    }
    /// <summary> adds step to current value and returns value </summary>
    public float Step() {
        val += (one * dir);
        return val;
    }
    /// <summary> adds step to current value and returns value </summary>
    public float StepOne(float _dir = 1) {
        val += (one * _dir * dir);
        return val;
    }
    /// <summary> adds step to current value and returns value </summary>
    public float StepRes( float _dir = 1 ) {
        val += (res * _dir * dir);
        return val;
    }
    /// <summary> adds step to current value and returns value </summary>
    public float GetLerp(float _lerpFact = 0.1f) {

        valLerp = valLerp + ((val -valLerp)* _lerpFact) ;
        //val += (one * _dir * dir);
        return valLerp;
    }

    /// <summary> checks if next step would be last </summary>
    public bool IsNextStepEnd() {
        if ( (_val + (one * dir)) >= max || (val + (one * dir)) <= min ) return true;
        return false;
    }
    /// <summary> checks if no more steps </summary>
    public bool IsStepEnd() {
        if ( _val >= max || _val <= min ) return true;
        return false;
    }
    float Clamp() {
        return Mathf.Clamp( _val, min, max );
    }
    public float Max() {
        _val = max;
        return _val;
    }
    public float Min() {
        _val = min;
        return _val;
    }
}
