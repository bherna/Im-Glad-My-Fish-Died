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
    }




    //basic way guppys move
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

        //since new target
        NewTargetVariables();

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

    }



    //whenever a new target is set we reset our sprite variables
    protected virtual void NewTargetVariables()
    {
        //reset our turning time for lerp
        startTime = Time.time;


        //new random velocity
        curr_roam_velocity = Random.Range(range_roam_veloocity[0], range_roam_veloocity[1]);
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
