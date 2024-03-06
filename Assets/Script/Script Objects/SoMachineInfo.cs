using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu( fileName = "SoMachineInfo", menuName = "VSCI/Machine info" )]
public class SoMachineInfo : ScriptableObject {
    public string label = "Centrifuge";
    [Header("--- DESCRIPTION ---")]

    public string title = "Title here";
    [TextArea(3,3)]
    public string descriptionShort;
    [TextArea(6,16)]
    public string descriptionLong;
    //public Sprite desriptionGraphic;
    /*
    [Space(10)]
    [Header( "--- GUIDE ---" )]
    [TextArea(5,16)]
    public string guide;
    [TextArea(5,16)]
    public string guideA;
    [TextArea(5,16)]
    public string guideB;
    [TextArea(5,16)]
    public string guideC;*/

    //[SerializeField] public List<Texture> textures;
    [SerializeField] public Texture texture;
    [SerializeField] public Sprite sprite;
    //[SerializeField] public Image image;

    public SoMachineInfo() {
        //dim.x = norm.width;
        //dim.y = norm.height;
        //dim = new Vector2( norm.width, norm.height);
    } 
}
