using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu( fileName = "SoCursor", menuName = "repol/Cursor" )]
/// <summary> Cursor set </summary>
public class SoCursor : ScriptableObject {
    public string label = "na";
    public CursorType type;
    [Header("--- TEXTURES ---")]
    public Texture2D norm;
    public Texture2D over;
    public Texture2D down;
    public Texture2D dont;
    [Space(10)]
    public Vector2 offset;
    //public Vector2 dim;

    public SoCursor() {
        //dim.x = norm.width;
        //dim.y = norm.height;
        //dim = new Vector2( norm.width, norm.height);
    } 
}
