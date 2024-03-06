using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GStatus {

    Status state;
    public Status State {
        get { return state; }
        private set { state = value; }
    }

    public GStatus() {
    }
    public void SetStatus( Status l_mode ) {

        switch ( l_mode ) {
            case Status.menu:
                break;
            case Status.tour:
                break;
            case Status.practical:
                break;
            case Status.restart:
                break;
            case Status.profile:
                break;
            case Status.options:
                break;
            case Status.results:
                break;
            case Status.quit:
                break;
            default:
                break;
        }
        State = l_mode;
    }


    public enum Status {
        menu,
        tour,
        practical,
        restart,
        profile,
        options,
        results,
        quit
    }


}

