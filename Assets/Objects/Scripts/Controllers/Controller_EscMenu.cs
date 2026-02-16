using UnityEngine;

public class Controller_EscMenu : MonoBehaviour
{
    public bool paused { get; private set; } = false;


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
}
