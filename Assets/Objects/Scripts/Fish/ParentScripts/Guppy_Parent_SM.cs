using UnityEngine;




//idle, doing  absolutly nothing
//roam, just casually swmimming to next destination
//hungry, actively looking for food, will move faster
//grabbed, doing nothing, but is currently grabbed by player
//

//additional states:
//Follow, added by pet charlie for creating a swarm state, below the hunger state
public enum Guppy_States { Grabbed, Hungry, Roam, ClusterRoam, Idle, Panic, Follow};




//the majority of guppys will have about the same state preferences.
//this is the general setup:
//      Grabbed > Panic = Hungry > Roam > Idle
//      High > Low Precedence
public class Guppy_Parent_SM : Parent_SM
{

    // --------------------------------- gubby script reference --------------------------------- 
    protected Guppy_Parent_Movement guppy_Parent_Movement; //is auto added, no need to manually add



    //variables
    public Guppy_States guppy_current_state { get; private set; } = Guppy_States.Roam;

    //State rotation trakcer
    public int curr_rotationCountdown = 1;
    protected int[] range_rotationCountdown = new int[2] { 1, 7 };




    // Start is called before the first frame update
    protected void Start()
    {
        guppy_Parent_Movement = GetComponent<Guppy_Parent_Movement>();
    }




    // Update is called once per frame
    protected void Update()
    {

        //this update will follow the basic 4 movement types (panic, hungry, roam idle), if more are added, just update it in that guppy


        //if ther's no food in the tank, become panic'd
        /*
        if (guppy_current_state == Guppy_States.hungry && Controller_Food.instance.GetFoodLength() == 0)
        {
            guppy_current_state = Guppy_States.Panic;
        }
        */
        EnterState();
    }

    protected void EnterState()
    {
        //enter state logic
        switch (guppy_current_state)
        {
            case Guppy_States.Panic:
                guppy_Parent_Movement.PanicMode();
                break;
            case Guppy_States.Hungry:
                guppy_Parent_Movement.HungryMode();
                break;
            case Guppy_States.Roam:
                guppy_Parent_Movement.RoamMode();
                break;
            case Guppy_States.Idle:
                guppy_Parent_Movement.IdleMode();
                break;
            case Guppy_States.ClusterRoam:
                guppy_Parent_Movement.ClusterRoamMode();
                break;
            default:
                Debug.Log("No current state for guppy");
                break;
        }
    }



    //make sure when using this function that its called when we actually finish a rotation, 
    //also all states share the same rotation  count down which can become confusing
    public void FinishStateRotation(Guppy_States stateCalledFrom)
    {

        curr_rotationCountdown -= 1;

        if (curr_rotationCountdown <= 0)
        {
            switch (stateCalledFrom)
            {
                case Guppy_States.Idle:
                    GuppyToState(Guppy_States.Roam);
                    break;

                case Guppy_States.Roam:
                    GuppyToState(Guppy_States.ClusterRoam);
                    break;

                case Guppy_States.ClusterRoam:
                    GuppyToState(Guppy_States.Idle);
                    break;

                default:
                    Debug.Log("Case has not been added yet.");
                    break;
            }
        }
        

        
        

    }

    public void GuppyToState(Guppy_States newState)
    {
        switch (newState)
        {
            case Guppy_States.Hungry:
                guppy_current_state = newState;
                break;

            default:
                guppy_current_state = newState;
                curr_rotationCountdown = Random.Range(range_rotationCountdown[0], range_rotationCountdown[1]);
                break;
        }
    }




    /*
    
    
    public void GuppyToFollow(GameObject schoolTeacher)
    {
        StartCoroutine(guppytofollow(schoolTeacher));
    }

    private IEnumerator guppytofollow(GameObject schoolTeacher)
    {

        float randTime = Random.Range(0f, 1.7f);
        yield return new WaitForSeconds(randTime);

        guppy_Movement.UpdateFollowObj(schoolTeacher);
        guppy_current_state = Guppy_States.follow;
    }
    */
}
