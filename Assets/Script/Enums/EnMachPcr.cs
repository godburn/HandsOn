
public static class EnMachPcr {

    public const string id = "PCR";
    /*
    public enum Look {
        none,
        machine,
        panel,
        load,
        lid,
        info
    }
    */
    public enum Hap {
        trigger,
        zoom_info, zoom_info_out,
        zoom_machine, zoom_tablet, 
        zoom_machine_out, zoom_tablet_out,
        lid_open, lid_close

    }

    /*
    public enum Status {
        off,
        on,
        run,
        runBad
    }
    public enum Mouse {
        none,
        machine,
        panel,
        load,
        lid,
        cover
    }
    */
    public enum SpinMode {
        off,
        up,
        hold,
        down
    }

    public enum EditMode {
        none, rpm, time
    }

    public enum Part {
        machine, panel, button, pick, lid, loader, cover
    }
    
    public enum Butt {
        none, open, up, down, select, start, next, previous
    }

    public enum LidState { closed, open }
}

