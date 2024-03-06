using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum pickupType { container, pipette, tip, bin, pipetteRack, none };
public enum pickupMouse { down, up, drag };

public class MouseSelector : Hierarchy {

    public pickupType type = pickupType.container;
    [SerializeField] public GameObject pickup;
    [SerializeField] public GameObject inGuideA;
    [SerializeField] public GameObject inGuideB;
    [SerializeField] public float width = 0.5f;

    public Vector3 startPos;
    public Quaternion startRot;

    public bool isDown = false;
    public float countDown = 0f;
    public float yMouseStart = 0f;
    public float yMouseOffset = 0f;
    public float yMouseRange = 100f;

    // Start is called before the first frame update
    void Start()    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()    {
        if (isDown) {
            countDown += 1f;
            if ( type == pickupType.container ) {
                Vector2 mousePos = Input.mousePosition;
                yMouseOffset = (yMouseStart - mousePos.y);
                yMouseOffset = SMath.Clamp( yMouseOffset, 0f-yMouseRange, yMouseRange );
                yMouseOffset = SMath.Remap( yMouseOffset, 0f-yMouseRange, yMouseRange, -1f, 1f );
                G.I.MouseAction( pickupMouse.drag, this, yMouseOffset );
            }
        }
    }

    void OnMouseDown() {
        isDown = true;
        G.I.MouseAction( pickupMouse.down, this );
        Vector2 mousePos = Input.mousePosition;
        yMouseStart = mousePos.y;// GV.mouseY;
    }

    void OnMouseUp() {
        isDown = false;
        countDown = 0f;
        G.I.MouseAction( pickupMouse.up, this );
    }

}
