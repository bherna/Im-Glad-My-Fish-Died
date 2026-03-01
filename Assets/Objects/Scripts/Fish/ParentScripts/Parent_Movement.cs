
using System.Collections;
using System.Threading;
using UnityEngine;


public class Parent_Movement : MonoBehaviour
{

    // --------------------------------- Sprite ---------------------------------
    [SerializeField] protected Transform sprite_transform;   //get transform of pet sprite
    protected float startTime;
    protected float h_turningSpeed = 1.5f;
    protected float y_angle = 0;


    // --------------------------------- Targeting ---------------------------------
    protected Vector3 curr_roamTarget;
    protected float targetRoam_ReachedRadius = 0.5f;        //used in determining if we have reached our destination
    protected float targetClusterRoam_ReachedRadius = 0.1f; //same as targetRoam, but for clusterRoam
    protected float newTarget_MinDistanceRad = 3;           //the minimum distance away from our fish current position, Used in Roam, 
    protected float newTarget_MaxDistanceRad = 2;           //the max distance from the fiish at curr position, used in cluster roam
    protected float curr_roam_velocity = 1;
    protected float[] range_roam_veloocity = new float[2] { 0.4f, 1.5f };


    protected bool isRotating;                                                             //used in determinig if we are activly rotating the fish
    protected Quaternion newRotationDestin = Quaternion.Euler(Vector3.zero);               //the new angle we plan on having this fish turn towards
    protected float curr_RotationSeconds = 0;                                              //used in lerp
    protected Quaternion start_Rotation = Quaternion.Euler(Vector3.zero);                  //Keeps track of the start rotation angle for lerp
    protected const float time_RotationFinish = 0.7f;                                      //how long it takes for this fish to finish rotating

    //references
    protected Rigidbody2D rb;

    //used in update position function for determining if we are going to use addforce or just update litteral position
    protected bool IStatic = true;
    //same as above but for part 2 of code: profile vs non profiled viewd fish code
    protected bool IProfile = true;
    protected int TankBoundryBounceStr = 15;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startTime = Time.time;
    }


    protected virtual void Update()
    {

        //is game paused, (need to pause fish, since they repeatedly get free force when unpaused
        if (Controller_EscMenu.instance.paused)
        {
            return;
        }

        SmoothRotation();
    }




    //basic way fish move
    protected void UpdatePosition(Vector3 target_pos, float current_Vel)
    {

        //if the guppy is static use this one
        //transform.position = Vector2.MoveTowards(transform.position, target_pos, current_Vel * Time.deltaTime);

      
        //this one uses rb, so make sure its attached
        var dir = (target_pos - transform.position).normalized;
        rb.AddForce(dir * current_Vel * Time.deltaTime, ForceMode2D.Impulse);

    }



    //create a new idle target, that is within the tank dimensions and outside the fish range.
    protected virtual void NewRandomIdleTarget_Tank(Guppy_States targetType = Guppy_States.Roam)
    {

        //tanke dememsions
        float[] swimDem = TankBoundries.instance.swim_arr;

        //get a new target area thats far from this guppy
        while (Mathf.Abs(Vector2.Distance(curr_roamTarget, transform.position)) < newTarget_MinDistanceRad)
        {

            curr_roamTarget = new Vector3(
                Random.Range(swimDem[0], swimDem[1]),
                Random.Range(swimDem[2], swimDem[3]),
                0
            );
        }


        //since new target
        //doo last since some variables update based on new target
        NewTargetVariables(curr_roamTarget);
    }



    //whenever a new target is set we reset our sprite variables
    protected virtual void NewTargetVariables(Vector3 newTarget)
    {
        //reset our turning time for lerp
        startTime = Time.time;


        //new random velocity
        curr_roam_velocity = Random.Range(range_roam_veloocity[0], range_roam_veloocity[1]);

        //now we update our rotation, kinda long to just throw in here
        UpdateRotation(newTarget);
    }



    private void UpdateRotation(Vector3 newTarget)
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


        //#2 case
        if (dir.x < 0)
        {
            //first flip the image to its facing the other way
            newAngle.x = 0;
            newAngle.y = 180;
            newAngle.z = Mathf.Atan2(dir.y, -1 * dir.x) * (180 / Mathf.PI);
        }
        //#3 case
        else if (dir.x < 0 && dir.y < 0)
        {
            newAngle.x = 0;
            newAngle.y = 180;
            newAngle.z = -1 * Mathf.Atan2(-1 * dir.y, -1 * dir.x) * (180 / Mathf.PI);
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
        StartRotation(Quaternion.Euler(newAngle));
    }




    //its just a lerp for rotating the fish towards a new angle rotation vector3, part of the UPdate call
    private void SmoothRotation()
    {
        transform.rotation = Quaternion.Lerp(start_Rotation, newRotationDestin, curr_RotationSeconds);
        curr_RotationSeconds += Time.deltaTime;

        if(curr_RotationSeconds > 1)
        {
            //stop doing this
            isRotating = false;
        }
    }

    //dont know how to send a message to the UPdate that we are starting a rotation
    protected void StartRotation(Quaternion newRotation)
    {
        //if we were already working on a rotation,
        //we want to set it to not clash with the next one about to happend
        if (isRotating) 
        {
            //hard set our transform to our old final rotation
            transform.rotation = newRotationDestin;

        }


        //get a new start rotation
        curr_RotationSeconds = 0;
        start_Rotation = transform.rotation; 
        isRotating = true;
        newRotationDestin = newRotation;

        
    }




    //when we hit an edge just head back to center of tank, easiest way to solve for now
    //reset the velocity and just transform.pos to inside the tank
    //also we want to make it so we stay as close to the edge as possible
    public void TankEdgeReached(Collider2D other)
    {
        rb.linearVelocity = Vector2.zero;
        //transform.position = Vector2.MoveTowards(transform.position, curr_roamTarget, 1 * Time.deltaTime);

        var dir = (curr_roamTarget - transform.position).normalized;
        rb.AddForce(dir * TankBoundryBounceStr * Time.deltaTime, ForceMode2D.Impulse);
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
        Gizmos.DrawWireSphere(transform.position, newTarget_MinDistanceRad);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, newTarget_MaxDistanceRad);


    }
}
