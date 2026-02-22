using Assests.Inputs;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller_EscMenu : MonoBehaviour
{
    //Here we have paused and eseMenuOpen, 
    //when the game is paused, the tank's delta time is set to 0
    //but that doesn't mean that the esc menu is open, we can have it open without DIRECTLY pausing
    //but having the esc menu open SHOULD HAVE the tank paused.
    //so
    //if the tank is not pause and we open the esc menu, then we pause here
    //if the tank is alreadyyy paused and we open the esc menu, then we don't bother pausing, but we flag this
    public bool paused { get; private set; } = false;
    public bool escMenuOpen { get; private set; } = false;


    //the flag bool is used to tell this esc menu class to either unpause the tank when we close the menu or not
    //since we can have an _ number of pauses going on, we count total pauses
    // when we pause -> +1 flag, unpause -> -1 flag (then we check if flag == 0, causeing unpause tank)
    private int flag = 0; //


    //singleton this class
    public static Controller_EscMenu instance { get; private set; }
    private void Awake()
    {

        //delete duplicate of this instance

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {


        //escape - open menu
        if (Input.GetKeyUp(KeyCode.Escape) && !escMenuOpen)
        {
            //pause game
            //open escape menu
            OpenMainMenu();
            //dont add anything  relased to opening menu here, put it in the funcction, else we risk bug
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && escMenuOpen)
        {
            //unpause game
            //close escape menu
            CloseMainMenu();
            //dont add anything  relased to closing menu here, put it in the funcction, else we risk bug
        }
    }




    //-----------------------------------------------------------------------------------------------------------
    //this functions are used for opening and closing the main menu ui stuff
    public void OpenMainMenu()
    {
        /*
        escMenuOpen = true;
        //pause all time references (physics, time)
        PauseTank(true);


        //disable ui buttons (so player can't purchase)
        interactive_list = new bool[Shop_Container.transform.childCount];
        int i = 0;
        //                                                          the true here is on purpose, we want to grab all buttons,
        //                                                          not just the active ones
        foreach (var btn in Shop_Container.GetComponentsInChildren<Button>(true))
        {

            interactive_list[i] = btn.interactable;//save
            i++;
            btn.interactable = false; //then disable

        }

        //enable esc ui
        Esc_UI.SetActive(true);

        //if tutorial is active, hide from view
        //make sure it exists first
        if (Controller_Tutorial.instance.tutorial_active)
        {
            TutorialReaderParent.instance.HideTutorial(false);
        }

        */
        //in the virtual mouse:
        //stop restraining real mouse
        //dont want to do this here, cause we have another if case to check for noooow
        CustomVirtualCursor.SetRestrain(false);
    }

    public void CloseMainMenu()
    {

        escMenuOpen = false;
        PauseTank(false);
        /*

        int i = 0;
        foreach (var btn in Shop_Container.GetComponentsInChildren<Button>(true))
        {
            btn.interactable = interactive_list[i];
            i++;
        }

        //disable esc ui
        Esc_UI.SetActive(false);

        //if tutorial is active, return its view
        TutorialReaderParent.instance.HideTutorial(true);
        */
        //re-restrain our mouse
        CustomVirtualCursor.SetRestrain(true);
    }


    //-----------------------------------------------------------------------------------------------------------




    //*

    //this function is used in making sure our tank isn't already paused
    //if it is, we don't want to unpause after we close the esc menu
    //it can also be referenced by other functions, outside this obj
    //parameter: true == pause the tank, false == unpause
    public void PauseTank(bool pauseOrNot)
    {
        /*

        if (pauseOrNot)
        {
            //since we don't know if this is the first or not, we are going to treat it if it is

            //before we do any time changes, we do things that are reliant on it
            //pause audio listeners
            Controller_SoundMixerManager.instance.LowPass(true);

            //now
            //we just set to 0
            Time.timeScale = 0;

            //now we are offisshally paused
            paused = true;
            //just add one to flag
            flag++;
        }
        else
        {
            //we want to unpause
            //so
            flag--; //sub 1

            //then check for 0
            if (flag <= 0)
            {
                //pre time scale change
                //pause audio listeners
                Controller_SoundMixerManager.instance.LowPass(false);

                //unpause time and bool
                Time.timeScale = 1;
                paused = false;
            }
        }
        */
    }
    

    public void GoToMainMenu()
    {

        //return time and audio back to normal
        Time.timeScale = 1;
        AudioListener.pause = false;

        //set our discord activity to in main menu
        //DiscordManager.instance.ChangeActivity(DiscordManager.DiscordState.Menu, 0, 0);

        //return
        SceneManager.LoadScene("MainMenu");
    }



}
