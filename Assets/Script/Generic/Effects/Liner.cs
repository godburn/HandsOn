using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liner : MonoBehaviour {
    [SerializeField] List<Transform> transforms;
    [SerializeField] int addMid = 3;
    [SerializeField] int SmoothIterations = 3;
    [SerializeField] bool keepStartAndEndObjects = true;
    [SerializeField] AnimationCurve stiffness;
    [SerializeField] AnimationCurve movement;
    [SerializeField] GameObject lockToObject;
    bool isLockToObject = false;
    List<Vector3> poses;
    LineRenderer lineRenderer;
    int offset = 0;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        poses = new List<Vector3>();
        foreach ( Transform t in transforms )
            poses.Add( t.position );

        poses.Add( transforms[ transforms.Count - 1 ].position );


        if ( lockToObject != null ) {
            isLockToObject = true;


        }


        /*
        // destroy guides
        int di = 0;
        if ( keepStartAndEndObjects ) di = 1;
        for ( int i = di; i < transforms.Count - di; i = i + 1 )
            Destroy( transforms[ i ].gameObject );
        */


        for ( int iii = 0; iii < addMid; iii++ ) {
            poses = AddMid( poses );
            poses = AddSmooth( poses, SmoothIterations );
        }
        poses = AddSmooth( poses, SmoothIterations );
        RenderLine( poses );

    }
    void RenderLine( List<Vector3> vectlist ) {
        lineRenderer.positionCount = vectlist.Count;
        lineRenderer.SetPositions( vectlist.ToArray() );
    }

    List<Vector3> AddSmooth( List<Vector3> vectlist, int iters ) {
        for ( int it = 0; it < iters; it++ ) {
            float _factor = 0f;
            float _time = 0f;
            float _tadd = 1f/(vectlist.Count - offset - 3f);

            for ( int i = offset; i < vectlist.Count - offset - 3; i = i + 1 ) {
                Vector3 mid_ = GetMid( vectlist[ i ], vectlist[ i + 2 ] );

                _factor = stiffness.Evaluate( _time );
                //Debug.Log( $"{_time} : {_factor}" );
                vectlist[ i + 1 ] = GetMid( vectlist[ i + 1 ], mid_ , _factor );
                _time += _tadd;
            }
        }
        return vectlist;
    }
    
    List<Vector3> AddMid( List<Vector3> vectlist ) {
        //offset = offset * 2;
        List<Vector3> newvl= new List<Vector3>();


        newvl.Add( vectlist[ 0 ] );
        for ( int i = 1; i < vectlist.Count -1; i = i + 1 ) {
            newvl.Add( vectlist[ i ] );
            newvl.Add( GetMid( vectlist[ i ], vectlist[ i + 1 ] ) );
        }
        newvl.Add( vectlist[ vectlist.Count - 1 ] );
        return newvl;
    }


    Vector3 GetMid( Vector3 vect1, Vector3 vect3, float factor = 0.5f ) {
        //Vector3 diff = Vector3.zero;
        return vect3 +( (vect1 - vect3) * (1-factor));

        //return new Vector3( (vect1.x + vect3.x) * factor, (vect1.y + vect3.y) * factor, (vect1.z + vect3.z) * factor );
    }
}












/*


    Vector3 GetMidOld( Vector3 vect1, Vector3 vect3, float factor = 0.5f ) {
        return new Vector3( (vect1.x + vect3.x) * factor, (vect1.y + vect3.y) * factor, (vect1.z + vect3.z) * factor );
    }

        /*
    //[SerializeField] float vertexCount = 12;



        var pointList = new List<Vector3>();
        var pointListPart = new List<Vector3>();




        pointList.Add( poses[ 0 ] );

        for ( int i = 0; i < poses.Count - 2; i = i + 1 ) {
            pointListPart = DrawQuadraticBezierCurve( pointList[ pointList.Count - 1 ], poses[ i + 1 ], poses[ i + 2 ] );

            float count = pointListPart.Count / 2;
            if ( i == poses.Count - 3 ) count = pointListPart.Count;

            for ( int ii = 0; ii < count; ii = ii + 1 )
                pointList.Add( pointListPart[ ii ] );

        }

        //pointList = AddSmooth( pointList, SmoothIterations );
        //RenderLine( pointList );

        



List<Vector3> DrawQuadraticBezierCurve( Vector3 point0, Vector3 point1, Vector3 point2 ) {
        var pointList_ = new List<Vector3>();
        //lineRenderer.positionCount = 10;
        float positionCount = vertexCount;
        float t = 0f;
        Vector3 B =  new Vector3(0, 0, 0);
        for ( int i = 0; i < positionCount; i++ ) {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            //lineRenderer.SetPosition( i, B );
            pointList_.Add( B );
            t += (1 / positionCount);
        }
        return pointList_;
    }









void DrawQuadraticBezierCurveOriginal( Vector3 point0, Vector3 point1, Vector3 point2 ) {
    lineRenderer.positionCount = 200;
    float t = 0f;
    Vector3 B = new Vector3(0, 0, 0);
    for ( int i = 0; i < lineRenderer.positionCount; i++ ) {
        B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
        lineRenderer.SetPosition( i, B );
        t += (1 / (float)lineRenderer.positionCount);
    }
}



List<Vector3> GetCurve( Vector3 vect1, Vector3 vect2, Vector3 vect3 ) {


    vect2 = new Vector3( (vect1.x + vect3.x) / 2, (vect1.y + vect3.y) / 2, (vect1.z + vect3.z) / 2 );

    var pointList_ = new List<Vector3>();

    for ( float ratio = 0; ratio <= 1; ratio += 1 / vertexCount ) {
        var tangent1 = Vector3.Lerp(vect1, vect2, ratio);
        var tangent2 = Vector3.Lerp(vect2, vect3, ratio);
        var curve = Vector3.Lerp(tangent1, tangent2, ratio);

        pointList_.Add( curve );
    }


    return pointList_;
    //lineRenderer.positionCount = pointList.Count;
    //lineRenderer.SetPositions( pointList.ToArray() );
}


int n=3;   // number of your objects
List<float> x =  new List<float>();  // your objects

float get_object( float t ) // linearly interpolate objects x[] based in parameter t = <0,1>, return value must be the same type as your objects
   {
    int ix;
    float x0,x1; // the same type as your objects
                 // get segment ix and parameter t
    t *= n;
    ix = (int)Mathf.Floor( t );
    t -= ix;

    // get closest known points x0,x1
    x0 = x[ ix ]; ix++;
    if ( ix < n ) x1 = x[ ix ]; else return x0;
    // interpolate
    return x0 + (x1 - x0) * t;
}



}


Point2.transform.position = new Vector3( (Point1.transform.position.x + Point3.transform.position.x) / 2, Point2Ypositio, (Point1.transform.position.z + Point3.transform.position.z) / 2 );
var pointList = new List<Vector3>();

for ( float ratio = 0; ratio <= 1; ratio += 1 / vertexCount ) {
    var tangent1 = Vector3.Lerp(Point1.position, Point2.position, ratio);
    var tangent2 = Vector3.Lerp(Point2.position, Point3.position, ratio);
    var curve = Vector3.Lerp(tangent1, tangent2, ratio);

    pointList.Add( curve );
}

lineRenderer.positionCount = pointList.Count;
lineRenderer.SetPositions( pointList.ToArray() );
    }

*/