using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GcScenes : MonoBehaviour {
    //Create a custom enumrator for each of our scene

    public static int on = 0;

    //Reset method when we die and respawn
    //As part of Bedtime Maddness we won't have a need for reset method but
    //its one of those good to know methods.
    public void ResetScene() {
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
    }

    //When we die we want to have a custom scene to manage GameOver beahavior
    public void GameOver() {
        SceneManager.LoadScene( "GameOver" );
    }

    //As we develop we will have a test level therefore for now by default
    //lets load our test level
    public void BeginGame() {
        SceneManager.LoadScene( (int)Scenes.c_tour );
        //G.State = AppState.Play;
    }

    public void MainScene() {
        SceneManager.LoadScene( (int)Scenes.c_tour );
    }

    //Quit Game Method
    public void ExitGame() {
        Application.Quit();
    }

    public void NextLevel() {
        switch ( on ) {
            case 2:
            case 3:
            case 4:
            case 5: {
                    SceneManager.LoadScene( on + 1 );
                    break;
                }
        }
    }

    public void SceneAction( Mode l_mode ) {

        switch ( l_mode ) {
            case Mode.start:
                break;
            case Mode.restart:
                break;
            case Mode.end:
                break;
            default:
                break;
        }
    }
    public enum Mode {
        start,
        restart,
        end
    }
}

public enum Scenes {
    a_boot, // Loading up the scene
    b_menu, // This is where we are going to have our title and start menu
    c_tour, // General level reference
    practical_001,
    d_over
}
