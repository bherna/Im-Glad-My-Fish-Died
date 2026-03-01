using UnityEngine;

public class Guppy_Parent_Movement : Parent_Movement
{
    protected Guppy_Parent_SM guppy_SM;

    //food target,  like the roam target
    protected GameObject foodTarget;

    protected float hungry_velocity = 1.7f;

    protected float panic_velocity = 2.1f;


    //idle countdown variables
    protected int[] range_SecsLeft = new int[2] {2, 3}; 
    protected float curr_SecsLeft = 0;


    protected override void Start() 
    {
        base.Start();
        guppy_SM = GetComponent<Guppy_Parent_SM>();
    }


    //Guppy will just do a roam mode but just fast and consistent
    //A guppy has to be either forced out of panic, cause it doesnt have a way to exit
    //compare to roam mode, theres a missing line in the else logic that'll keep a gupppy stuck in this state.
    //its also used in hunger mode to simulate missing food
    public void PanicMode()
    {
        var distance = Vector3.Distance(curr_roamTarget, transform.position);

        if (Mathf.Abs(distance) > targetRoam_ReachedRadius)
        {

            UpdatePosition(curr_roamTarget, panic_velocity);
        }

        //get new point once fish reaches it
        else
        {
            NewRandomIdleTarget_Tank();

        }
    }

    
    //Guppy is activly heading towards a food pellet
    public void HungryMode()
    {
        //if wer not targeting food (ie:current target food is null) 
        //          : target a food
        if (foodTarget == null)
        {
            NewFoodTarget_Tank();
        }

        //if food target is still null
        if (foodTarget == null)
        {
            //run a form of panic mode
            PanicMode();
        }
        else
        {
            //else
            //follow food
            //head towards target 
            UpdatePosition(foodTarget.transform.position, hungry_velocity);
        }
    }
    
    //Guppy is just chilling and activly swimming to next destination, 
    //if we make it to our destination then we just find a new one, or we go into idel
    public void RoamMode()
    {
        var distance = Vector3.Distance(curr_roamTarget, transform.position);

        if (Mathf.Abs(distance) > targetRoam_ReachedRadius)
        {

            UpdatePosition(curr_roamTarget, curr_roam_velocity);
        }

        //get new point once fish reaches it
        else
        {
            //send a message to the State machine that we finished with this rotation
            guppy_SM.FinishStateRotation(Guppy_States.Roam);
            NewRandomIdleTarget_Tank();

        }
    }

    public void ClusterRoamMode()
    {
        var distance = Vector3.Distance(curr_roamTarget, transform.position);

        if (Mathf.Abs(distance) > targetClusterRoam_ReachedRadius)
        {

            UpdatePosition(curr_roamTarget, curr_roam_velocity);
        }

        //get new point once fish reaches it
        else
        {
            //send a message to the State machine that we finished with this rotation
            guppy_SM.FinishStateRotation(Guppy_States.ClusterRoam);
            NewRandomIdleTarget_Tank(Guppy_States.ClusterRoam);

        }
    }

    protected override void NewRandomIdleTarget_Tank(Guppy_States targetType = Guppy_States.Roam)
    {
        

        //tanke dememsions
        float[] swimDem = TankBoundries.instance.swim_arr;

        if(targetType == Guppy_States.ClusterRoam)
        {
            //run this once atleast cause it'll just think we found a good enough target cause we are already on it
            do
            {
                curr_roamTarget = new Vector3(
                    Random.Range(swimDem[0], swimDem[1]),
                    Random.Range(swimDem[2], swimDem[3]),
                    0);
            }
            //get a new target area thats close to this  guppy
            //while our distance is more than the radius, find until its LESS THAN
            while (Mathf.Abs(Vector2.Distance(curr_roamTarget, transform.position)) > newTarget_MaxDistanceRad);
            
        }
        
        //normal target getting
        else
        {
            do
            {

                curr_roamTarget = new Vector3(
                    Random.Range(swimDem[0], swimDem[1]),
                    Random.Range(swimDem[2], swimDem[3]),
                    0);
            }
            //get a new target area thats far from this guppy
            while (Mathf.Abs(Vector2.Distance(curr_roamTarget, transform.position)) < newTarget_MinDistanceRad);
            
        }

        //since new target
        NewTargetVariables(curr_roamTarget);
    }


    //is just stationary, will conver into roam mode after it finishes.
    //this is called after we set a idle ammount, this only transitions into idle from turn to idle function
    //so it'll be IdleMode(); Gotoidle(); in the same func or what ever
    public void IdleMode()
    {
        curr_SecsLeft -= Time.deltaTime;

        if(curr_SecsLeft <= 0)
        {
            //transfer
            guppy_SM.FinishStateRotation(Guppy_States.Idle);

            //then reset our curr_sec for  next time
            curr_SecsLeft = Random.Range((float)range_SecsLeft[0], range_SecsLeft[1]);
        }
        
    }


    
    private void NewFoodTarget_Tank()
    {

        //find food to followe 
        var closestDis = float.PositiveInfinity;
        var allFoods = Controller_Food.instance.GetAllFood();
        if (allFoods.Count == 0) { return; }

        //for all food objs in scene, get the closest
        var tempTarget = allFoods[0];
        foreach (GameObject food in allFoods)
        {

            var newDis = (transform.position - food.transform.position).sqrMagnitude;

            if (newDis < closestDis)
            {

                closestDis = newDis;
                tempTarget = food;
            }
        }
        //
        foodTarget = tempTarget;

        //once the fish or the trash can gets to the food, the food destroysSelf(), and foodtarget = null again


        //new target
        NewTargetVariables(foodTarget.transform.position);
    }
    
}
