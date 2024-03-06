
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public static class EnPsu {

    public enum Look {
        none,
        machine,
        tank,
        psu,
        gel,
        info
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
