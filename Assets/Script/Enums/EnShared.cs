
public enum MaterialType { 
    diffuse, normal, specular 
}

public enum UserStatus {
    none, move, point, machineView, zoom, overlay
}

public enum GMode {
    none, play, tour
}

public enum GModeOverlay {
    none, menu, question, info
}

public enum CamFocusMode {
    none, dynamic, last, menu, mid, close, machine, tour
}



// INTERACTIONS ////////////////////////////////////

public enum RayResponse {
    pass, block, trigger
}

public enum GrabType {
    none, source, destination
}

public enum InputMouse {
    left, middle, right
}

public enum ButtonStatus {
    norm, over, down, selected, disabled, other
}

public enum MouseEvent {
    none, enter, exit, press, release, releaseOut, dragStart, dragIn, dragOut, dragInUp, hold
}



// MACHINE ////////////////////////////////////

public enum MachineType {
    none, tablet, centrifuge, imagerUv, gelElectro, pcr, spectro, microlight
}

public enum MachineModel {
    none, tablet, centrifugeSigma, imagerUvEnduro, gelElectroMini, pcr
}

public enum MachineCore {
    none,
    tablet, psu,
    centrifugeMain, centrifugePanel, centrifugeLoader,
    imagerUvMain, imagerUvTablet,
    gelElectroMain, gelElectropad,
    pcrMain
}


public enum MachineLook {
    none,
    back,
    info,
    machine,
    lid,
    panel,

    psu,
    load,
    tank,
    gel,
}

/*

public enum TypeDimension {
    button2d, button2dWorld, button3d
}

public enum InputKey {
    left, right, up, down, duck, escape, space
}

public enum GHap {
    openMenuFromGame, openMenuFromTour, startGameFromTour
}

*/
