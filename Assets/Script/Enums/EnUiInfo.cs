/// <summary> for both tablets and overlays </summary>
public static class EnUiInfo {

    public const string id = "info";

    public enum Look {
        none,
        machine,
        tablet,
        overlay,
        question
    }

    public enum Hap {
        zoom_machine, zoom_machine_out
    }

    public enum Part {
        machine, tablet, button, question, tour
    }
    
    public enum Butt {
        none, info, next, previous,
        infoOpen, infoClose, back,
        A, B, C, D, E, F
    }

}

