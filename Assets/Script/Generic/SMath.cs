using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Generic maths stuff </summary>
public static class SMath {

    public static float FindDifference( float nr1, float nr2 ) {
        return Mathf.Abs( nr1 - nr2 );
    }

    public static float Remap( this float value, float from1, float to1, float from2, float to2 ) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static float Clamp( float val, float a, float b ) {
        if ( a < b ) return Mathf.Clamp( val, a, b );
        return Mathf.Clamp( val, b, a );
    }

    public static int RandInt() {
        return UnityEngine.Random.Range( 2, 10 );
    }

    public static float PosNegAngle( Vector3 a1, Vector3 a2, Vector3 n ) {
        float angle = Vector3.Angle(a1, a2);
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a1, a2)));
        return angle * sign;
    }

    public static float ClampAngle( float current, float min, float max ) {
        float dtAngle = Mathf.Abs(((min - max) + 180) % 360 - 180);
        float hdtAngle = dtAngle * 0.5f;
        float midAngle = min + hdtAngle;

        float offset = Mathf.Abs(Mathf.DeltaAngle(current, midAngle)) - hdtAngle;
        if ( offset > 0 )
            current = Mathf.MoveTowardsAngle( current, midAngle, offset );
        return current;
    }

    public static float ClampAngleMod( float current, float min, float max ) {
        float dtAngle = Mathf.Abs(((min - max) + 180) % 360 - 180);
        float hdtAngle = dtAngle * 0.5f;
        float midAngle = min + hdtAngle;

        float offset = Mathf.Abs(Mathf.DeltaAngle(current, midAngle)) - hdtAngle;
        if ( offset > 0 )
            current = Mathf.MoveTowardsAngle( current, midAngle, offset );
        return current;
    }

    public static Tuple<int, float> GetCLosestFloatInList( float val_, ref List<float> inArr_ ) {
        if ( inArr_ == null || inArr_.Count == 0 )
            return new Tuple<int, float>( -1, -999f );

        float _closest = 100000000;
        int _index = -1;
        float _value = 0;

        for ( int i = 0; i < inArr_.Count; i++ ) {
            if ( Math.Abs( val_ - inArr_[ i ] ) < _closest ) {
                _closest = Math.Abs( val_ - inArr_[ i ] );
                _index = i;
                _value = inArr_[ i ];
            }

        }
        return new Tuple<int, float>( _index, _value );
    }

}


