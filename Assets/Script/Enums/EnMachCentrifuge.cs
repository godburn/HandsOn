
public static class EnMachCentrifuge {

    public const string id = "Centrifuge";
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
        zoom_machine, zoom_machine_out,
        zoom_panel, zoom_panel_out, 
        zoom_loader, zoom_loader_out,
         
        lid_unlock, lid_open, lid_close,
        cover_on, cover_off,
        spin_on, spin_off
   
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

    public enum SpinHap {
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
        none, open, up, down, select, start
    }

    public enum LidState { closed, half, open }
}

