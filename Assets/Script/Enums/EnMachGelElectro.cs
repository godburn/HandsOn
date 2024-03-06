
public static class EnMachGelElectro {

    public const string id = "GelElectro";
    /*
    public enum Look {
        none,
        machine,
        tank,
        psu,
        gel,
        info
    }
    */
    public enum Hap {
        trigger,
        zoom_info, zoom_info_out,
        zoom_machine, zoom_panel, 
        zoom_machine_out, zoom_panel_out,
        cover_on, cover_off,
        psu_on, psu_off, psu_change,
        bung_off, comb_out,
        tray_water_in, tray_water_out, tray_imager, tray_empty, tray_end,
        inkEnd
    }

    public enum Status {
        off,
        on,
        run,
        runBad
    }

    public enum OperationMode {
        off,
        pour,
        ink,
        electro
    }

    public enum Part {
        bath,
        psu,
        tray,
        cylinder,

        cover,
        gelink,

        bung,
        comb,
        gelpad

    }

    public enum Butt {
        none,
        dial,
        left,
        right

    }

}
