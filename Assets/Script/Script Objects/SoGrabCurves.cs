using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "SoGrabCurves", menuName = "VSCI/Grab Curves" )]
public class SoGrabCurves : ScriptableObject {
    public string label = "Grab Curves";

    [SerializeField] public GameObject destinationObject;
    [SerializeField] public bool effectRotation = false;

    [Header("Ratio curves ")]

    [SerializeField] public AnimationCurve xCurve;
    [SerializeField] public AnimationCurve yCurve;
    [SerializeField] public AnimationCurve zCurve;

    [Header("Addition curves ")]

    [SerializeField] public AnimationCurve xAddCurve;
    [SerializeField] public AnimationCurve yAddCurve;
    [SerializeField] public AnimationCurve zAddCurve;

    [Header("Addition curve factor ")]

    [SerializeField] public float xAddFactor = 0f;
    [SerializeField] public float yAddFactor = 0f;
    [SerializeField] public float zAddFactor = 0f;

}
