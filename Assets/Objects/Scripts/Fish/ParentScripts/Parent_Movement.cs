
using System.Collections;
using System.Threading;
using UnityEngine;

//just a quick run down on them
//Constant is used whenever our fish wants to move at a constat pace around the tank
//burst is for when they want to move with more of a dashing motion
//Idle , we aren't moving, but is useful to have for rotation
public enum MovementType { Constant, Burst, Idle};

public class Parent_Movement : MonoBehaviour
{

    // --------------------------------- Sprite ---------------------------------
    [SerializeField] protected Transform sprite_transform;   //get transform of pet sprite


    // --------------------------------- Targeting ---------------------------------
    public Vector3 curr_roamTarget { get; protected set; }
    protected float targetRoam_ReachedRadius = 0.5f;                                    //used in determining if we have reached our destination
    protected float targetClusterRoam_ReachedRadius = 0.3f;                             //same as targetRoam, but for clusterRoam
    protected float newRoamTarget_MinDistanceRad = 3;                                   //the minimum distance away from our fish current position, Used in Roam, 
    protected float[] newClusterTarget_RangeDisRad = new float[2] { 1.7f, 3f };         //the max distance from the fiish at curr position, used in cluster roam


    // --------------------------------- Burst Velocity ---------------------------------
    protected float curr_BurstVelocity = 3;                                         //current rotation burst speed,
    protected float[] range_BurstVeloocity = new float[2] { 0.68f, 2.43f };         //used in getting anew curr_burst velocity
    protected float baseVelocity = 0.1f;                                            //the slowest a fish will go while using burst movement
    protected float[] range_BurstSwimAnimeSPD = new float[2] { 70 , 100};           //used in getting a new swim speed (curr_SwimSpd)


    //--------------------------------- Const Velocity ---------------------------------
    protected float curr_ConstVelocity = 3;                                        //current constant velocity
    protected float[] range_ConstVeloocity = new float[2] { 0.8f, 1.5f };          //used in getting a new curr_const vel


    // --------------------------------- Turning ---------------------------------
    protected bool isTurning;                                                           //used in determinig if we are activly rotating the fish
    protected Quaternion start_TurningVector = Quaternion.Euler(Vector3.zero);          //Keeps track of the start rotation angle for lerp
    protected Quaternion end_TurningVector = Quaternion.Euler(Vector3.zero);            //the new angle we plan on having this fish turn towards
    protected float curr_RotationSeconds = 0;                                           //used in lerp
    //these are used twogether
    protected float total_secsTurnTime = 0.5f;                                          //how long it takes for this fish to finish turning around
    protected float[] total_TurnTimeCount = new float[3] { 0.55f, 0.35f, 0.15f };         //and its differ turning speeds for: [roam, cluster, panic]


    // --------------------------------- Swimming ---------------------------------
    //these ones are for the animation side of swimming, not actually the movement 
    private const float max_SwimDegree = 20;                    //how much this fish can turn its body while swimming
    private float curr_SwimDegree = 0;                          //what is this fish current turn'd degree
    private float start_SwimAnimeSpd = 40;                      //how fast this fish does its swimming animation, (if its a constant speed 40 is good)
    private float curr_SwimAnimationSpeed = 0;                  //this depending on movementtype will change
    private float curr_SwimDir = 1;                             //used in updating the direction its either 1 or -1
    private float curr_SwimLerpSecs = 0;                        //used in updating our swimming swaddle (lerp)

    //doesn't really have a section to be put under, but this is our linear dampening for this fish
    //this one is more of a const cause its only going to get referenced/used to set 
    protected const float linearDamp = 0.5f;

    //references
    protected Rigidbody2D rb;

    //well, it does what it says really, every times this fish touches a boundry it bouces off with this addforce str
    public float TankBoundryBounceStr = 0.25f;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    protected virtual void Update()
    {

        //is game paused, (need to pause fish, since they repeatedly get free force when unpaused
        if (Controller_EscMenu.instance.paused)
        {
            return;
        }

        //SmootTurnRotation();
    }




    //basic way fish move
    protected void UpdatePosition(Vector3 target_pos, float current_Vel, MovementType moveType)
    {
        //check here if we are currently turning
        if (ActivelyTurningRotation()) { return; }

        //will always need a direction
        var dir = (target_pos - transform.position).normalized;

        //and we update our swmming animation
        SwimmingRotation();

        switch (moveType)
        {

            //this is not used, this is just basic movement towards object without rigidbody
            //transform.position = Vector2.MoveTowards(transform.position, target_pos, current_Vel * Time.deltaTime);


            //all this does is create a constant velocity towards the target
            case (MovementType.Constant):
                rb.AddForce(dir * current_Vel * Time.deltaTime, ForceMode2D.Impulse);

                //dont need to update our swimming animation here
                //its constant, just use the start animation speed

                break;

            
            //takes current curr_velocity as max velocity
            //then we use a lerp to slowly decrease to a base movement speed
            case (MovementType.Burst):

                
                //update our swiming (animation)
                curr_SwimLerpSecs = 4 *Time.deltaTime;
                Debug.Log(curr_SwimLerpSecs);
                //if we finish this burst, get a new velocity 
                if( Mathf.Abs(rb.linearVelocityX) + Mathf.Abs(rb.linearVelocityY) < baseVelocity) 
                {
                    NewBurstVariables();
                    rb.AddForce(dir * curr_BurstVelocity, ForceMode2D.Impulse);
                }

                break;



            case (MovementType.Idle):
                //do nothing really

                //just make sure our swimming animation stops
                //setting our curr_SwimLerpSecs to 1, sets our swimming animation to 0 
                curr_SwimLerpSecs = 1;
                break;


            default:
                Debug.Log("Dont know how you got here");
                break;
        }
    }







    //create a new idle target, that is within the tank dimensions and outside the fish range.
    protected virtual void NewRandomIdleTarget_Tank(Guppy_States guppy_state = Guppy_States.Roam)
    {

        //tanke dememsions
        float[] swimDem = TankBoundries.instance.swim_arr;

        //run this once atleast cause it'll just think we found a good enough target cause we are already on it
        do
        {

            curr_roamTarget = new Vector3(
                Random.Range(swimDem[0], swimDem[1]),
                Random.Range(swimDem[2], swimDem[3]),
                0);
        }
        //get a new target area thats far from this guppy
        while (Mathf.Abs(Vector2.Distance(curr_roamTarget, transform.position)) < newRoamTarget_MinDistanceRad);

        //since new target
        //doo last since some variables update based on new target
        NewTargetVariables(curr_roamTarget, guppy_state);
    }



    //whenever a new target is set we reset some variables
    //this one is specific to a completely new rotation, 
    public virtual void NewTargetVariables(Vector3 newTarget, Guppy_States guppy_State)
    {
        //just for clear-ity, im just gonna put this in here
        switch (guppy_State)
        {
            case Guppy_States.Roam:
                total_secsTurnTime = total_TurnTimeCount[0];
                break;

            case Guppy_States.ClusterRoam:
                total_secsTurnTime = total_TurnTimeCount[1];
                break;

            case Guppy_States.Panic:
                total_secsTurnTime = total_TurnTimeCount[2];
                break;

            default:
                //dont care
                break;
        }

        //new random velociies (dont care what type it is, just get two new ones)
        curr_BurstVelocity = Random.Range(range_BurstVeloocity[0], range_BurstVeloocity[1]);
        curr_ConstVelocity = Random.Range(range_ConstVeloocity[0], range_ConstVeloocity[1]);

        //incase we are arn't using burst movement next rotation
        //set our curr_SwimSpd to something const
        //and reset our lerp counter
        start_SwimAnimeSpd = 40;
        curr_SwimLerpSecs = 0;

        //now we update our rotation, kinda long to just throw in 
        GetSetNewTurnRotation(newTarget);

    }





    //this is used in reseting our burst variables, 
    private void NewBurstVariables()
    {
        //new random velocity
        curr_BurstVelocity = Random.Range(range_BurstVeloocity[0], range_BurstVeloocity[1]);

        //new animation sppeed
        start_SwimAnimeSpd = Random.Range(range_BurstSwimAnimeSPD[0], range_BurstSwimAnimeSPD[1]);
        curr_SwimLerpSecs = 0;
    }








    private void GetSetNewTurnRotation(Vector3 newTarget)
    {
        //Set the Quaternion rotation to face towards the target
        //this is pretty complicated to explain, but essentially, 
        /*
         * just imagine a unit circle, 
         * when the fish is facing at its normal (1,0) direction
         * 
         * #1: if the fish needs to go to some direction between (1,0) and (0,1), we just rotate the body towards that direction
         * 
         * #2: if the fish needs to go in some dir between (0,1) and (-1,0), X is now negative so we have to flip the fish to compensate
         * 
         * #3: if the fish needs to go in some dir between (-1,0) and (0, -1), X and Y are negative, so we have to do the flip, we have to fix our Y to point negative,
         * 
         * #4: if the fish needs to go in soe dir between (0, -1) and (1, 0), Y is negative, we have to set our direction to point negative
         * 
         * 
         *  - when we have to set our direction to negative, this means we are using negative degrees, we do this to avoid the fish
         *    doing a whole 270 degree spin before getting to what rotation it needs to point at
         *    
         *  - when we have to flip, the fish has to do a turn around to face in the opposite direction (-1, 0), else we have a fish swimming upside down
         *  
         *  
         *  
         *  - the math on the other hand is straight forwad, if -x, we flip and we mul -1 on dir.x directly
            - if we have -y we have to do an abs() on our dir.y variable to get the degree, then we mul -1 on the entire func
        */

        Vector3 newAngle = new Vector3(); //just a thing to hold our new angle we are swithching too
        var dir = newTarget - transform.position; //this is the new directional vector that this fish is swimming towards


        //#3 case
        if (dir.x < 0 && dir.y < 0)
        {
            newAngle.x = 0;
            newAngle.y = 180;
            newAngle.z = -1 * Mathf.Atan2(-1 * dir.y, -1 * dir.x) * (180 / Mathf.PI);
        }
        //#2 case
        else if (dir.x < 0)
        {
            //first flip the image so its facing the other way
            newAngle.x = 0;
            newAngle.y = 180;
            newAngle.z = Mathf.Atan2(dir.y, -1 * dir.x) * (180 / Mathf.PI);
        }
        //#4 case
        else if (dir.y < 0)
        {
            newAngle.x = 0;
            newAngle.y = 0;
            newAngle.z = -1 * Mathf.Atan2(-1 * dir.y, dir.x) * (180 / Mathf.PI);
        }
        //normal case, #1
        else
        {
            newAngle.x = 0;
            newAngle.y = 0;
            newAngle.z = Mathf.Atan2(dir.y, dir.x) * (180 / Mathf.PI);

        }

        //transform.rotation = Quaternion.Euler(newAngle);
        StartTurningRotation(Quaternion.Euler(newAngle));
    }



    //We cut this as its own func cause we either do a new  turn based on two different states
    //one is the above func of getting and setting a new turning around animation rotation
    //the other is for specific cases, like a guppy targeting a food 
    protected void StartTurningRotation(Quaternion newRotation)
    {
        /*
        //if we were already working on a rotation,
        //we want to set it to not clash with the next one about to happend
        if (isTurning)
        {
            //hard set our transform to our _OLD_ final rotation
            transform.rotation = end_TurningVector;

        }
        */

        //before we can turn we need to 
        //lower our velocity somehow, so we will set our linear damp super high in here
        rb.linearDamping = 5;

        //get new start rotation variables
        curr_RotationSeconds = 0;                       //reset our counter
        start_TurningVector = transform.rotation;       //set our current transform as our base rot
        end_TurningVector = newRotation;                //our rotation we want to be at


        //also reset our swiming variables (might get  changed)
        curr_SwimDegree = 0;

    }




    /// <summary>
    //PART of the UPdate call
    //All active turning logic is put in this func call
    //there are two states we can be (turning around or swimming)
    //if we are  turning around we do:
    //          - a lerp for rotating the fish towards a new angle rotation vector3,
    //            look at the GetSetNewTurnRotation func
    //
    //          -- a normal swiming rotation, just moving the tail to make it look like
    //              the fish is swimming
    //             (this one is on its own function down below)
    /// </summary>
    /// <returns></returns>
    private bool ActivelyTurningRotation()
    {
        //first case of we are currently turning around
        //the lerp finishes once we reach 1, so anything before 1 is turning around logic
        if (curr_RotationSeconds < 1)
        {
            //essentially, we do a lerp, 
            //where its length of time is based on total_TurnTimeCount, 
            //kinda have to work around the lerp  0 to 1 input so its set up this wayy
            transform.rotation = Quaternion.Lerp(start_TurningVector, end_TurningVector, curr_RotationSeconds);
            curr_RotationSeconds += Time.deltaTime * (1 / total_secsTurnTime);

            //this chunk determines exit
            //the flat value is what would normally be 1, 
            //but we want to exit a bit early so we can look more natural, but still finsh the lerp
            if (curr_RotationSeconds >= 0.8f)
            {
                //all we reset is our linear damp and return false to coninue moving
                rb.linearDamping = linearDamp;
                return false;
            }
            else
            {
                //else we keep turning around
                return true;
            }
        }
        
        return false;

    }



    /// <summary>
    /// description taken from activelyturning func:
    /// 
    /// -- a normal swiming rotation, just moving the tail to make it look like
    //              the fish is swimming
    //              
    /// </summary>
    protected void SwimmingRotation()
    {

        //we are not turning, so we can do a swimming animation
        //since we are just messing with the Y, we have to keep the  x and Z the same
        Quaternion newSwim = end_TurningVector;
        Vector3 temp = newSwim.eulerAngles;

        //now update Y
        curr_SwimDegree += curr_SwimAnimationSpeed * curr_SwimDir * Time.deltaTime;

        //now if  we reach max turning , we want to start doing the other way
        if (Mathf.Abs(curr_SwimDegree) >= max_SwimDegree)
        {
            curr_SwimDir *= -1;
        }

        //update swimspeed/ give falloff
        curr_SwimAnimationSpeed = Mathf.Lerp(start_SwimAnimeSpd, 0, curr_SwimLerpSecs);

        //now update fish transform
        temp.y = temp.y + curr_SwimDegree;
        newSwim.eulerAngles = temp;
        transform.rotation = newSwim;
    }




    //when we hit an edge just head back to center of tank, easiest way to solve for now
    //reset the velocity and just transform.pos to inside the tank
    //also we want to make it so we stay as close to the edge as possible
    public void TankEdgeReached(Collider2D other)
    {
        //rb.linearVelocity = Vector2.zero;
        //transform.position = Vector2.MoveTowards(transform.position, curr_roamTarget, 1 * Time.deltaTime);

        //rb.linearVelocity = Vector2.zero;
        //var dir = (curr_roamTarget - transform.position).normalized;
        //rb.AddForce(dir * TankBoundryBounceStr * Time.deltaTime, ForceMode2D.Impulse);

        var dir = (curr_roamTarget - transform.position).normalized;
        rb.linearVelocity = dir * TankBoundryBounceStr;
    }









    protected void OnDrawGizmosSelected()
    {
        
        //if idle is null, dont show it
        if (curr_roamTarget == null || curr_roamTarget == new Vector3(0, 0, 0))
        {
            //dont show
        }
        else
        {
            //current target for fish
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(curr_roamTarget, targetRoam_ReachedRadius);//current target for fish
        }
        

        //current range untill new target
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, newRoamTarget_MinDistanceRad);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, newClusterTarget_RangeDisRad[0]);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, newClusterTarget_RangeDisRad[1]);


    }
}
