
using System.Collections.Generic;
using UnityEngine;
public class ImageAnimator : MonoBehaviour {

    [SerializeField] public List<Texture> m_Textures;

    //public float disorder = 0f;
    //public float disorderAdd = 0.02f;
    public int frameStep = 8;
    int frameCount = 0;
     int indCount = 8;
    Renderer m_Renderer;
    // Start is called before the first frame update
    void Start() {
        m_Renderer = GetComponent<Renderer>();
        m_Renderer.material.EnableKeyword( "_NORMALMAP" );
        m_Renderer.material.SetTexture( "_BumpMap", GetImage( 0 ) );

    }

    void Update() {
        frameCount++;
        if ( frameCount >= frameStep ) {
            frameCount = 0;
            indCount++;
            if ( indCount > m_Textures.Count ) indCount = 0;

            m_Renderer.material.SetTexture( "_BumpMap", GetImage( indCount ) );
            //disorder = disorder + (disorderAdd * frameStep);
            //if ( disorder > 1f ) disorder = disorder - 1f;
        }
    }
    public Texture GetImage( int _ind ) {

        if ( _ind >= m_Textures.Count ) return m_Textures[ m_Textures.Count - 1 ];
        return m_Textures[ _ind ];

    }
}
