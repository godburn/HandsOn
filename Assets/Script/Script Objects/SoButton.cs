using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "SoButton", menuName = "repol/Button" )]
/// <summary> Button set </summary>
public class SoButton : ScriptableObject {
    public string label = "na";
    //public CursorType type;
    [Header("--- TEXTURES ---")]
    public Sprite norm;
    public Sprite over;
    public Sprite down;
    public Sprite disabled;
    public Sprite selected;

    //[Space(10)]
    //public Vector2 offset;
    //public Vector2 dim;

    [Header( "--- LISTS ---" )]
    public List<string> labels;
    public List<Sprite> symbols;
    public List<Sprite> overlays;


    public SoButton() {
        //dim.x = norm.width;
        //dim.y = norm.height;
        //dim = new Vector2( norm.width, norm.height);
    } 
}
