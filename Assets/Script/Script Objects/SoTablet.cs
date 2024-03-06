using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "SoTablet", menuName = "VSCI/Tablet info" )]
public class SoTablet : ScriptableObject {
    public string label = "Tablet info";
    [TextArea(3,3)]
    [SerializeField] public string titleText;
    [SerializeField] public Texture titleTexture;
    //[SerializeField] public List<Texture> textures;
    [SerializeField] public List<Sprite> sprites;
    [SerializeField] public bool useFilenames = true;
    [SerializeField] public List<string> userTitles;

    //[Header("Auto")]
    //public List<string> titles;
    //public string contents = "";
    //public int pageNo = 0;
    //public int pageTotal = 0;
    /*
    [Header("--- DESCRIPTION ---")]
    [TextArea(6,16)]
    public string descriptionLong;

    [Space(10)]
    [Header( "--- GUIDE ---" )]
    [TextArea(5,16)]
    public string guide;
    [TextArea(5,16)]
    public string guideA;
    [TextArea(5,16)]
    public string guideB;
    [TextArea(5,16)]
    public string guideC;
    */

    


}
