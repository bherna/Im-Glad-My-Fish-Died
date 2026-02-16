using UnityEngine;




//idle, doing  absolutly nothing
//roam, just casually swmimming to next destination
//hungry, actively looking for food, will move faster
//grabbed, doing nothing, but is currently grabbed by player
//

//additional states:
//Follow, added by pet charlie for creating a swarm state, below the hunger state
public enum Guppy_States { Grabbed, Hungry, Roam, Idle, Panic, Follow};




//the majority of guppys will have about the same state preferences.
//this is the general setup:
//      Grabbed > Panic = Hungry > Roam > Idle
//      High > Low Precedence
public class Parent_SM : MonoBehaviour
{

    // --------------------------------- gubby script reference --------------------------------- 
    private Parent_Movement parent_Movement; //is auto added, no need to manually add



    //variables
    public Guppy_States guppy_current_state {get; private set; }





    // Start is called before the first frame update
    void Start()
    {
        parent_Movement = GetComponent<Parent_Movement>();
    }




    // Update is called once per frame
    void Update()
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

    private void EnterState()
    {
        //enter state logic
        switch (guppy_current_state)
        {
            case Guppy_States.Panic:
                parent_Movement.PanicMode();
                break;
            case Guppy_States.Hungry:
                parent_Movement.HungryMode();
                break;
            case Guppy_States.Roam:
                parent_Movement.RoamMode();
                break;
            case Guppy_States.Idle:
                parent_Movement.IdleMode();
                break;
            default:
                Debug.Log("No current state for guppy");
                break;
        }
    }


    /*
    public void GuppyToHungry()
    {
        guppy_current_state = Guppy_States.hungry;
    }
    public void GuppyToIdle()
    {
        guppy_current_state = Guppy_States.idle;
    }
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
