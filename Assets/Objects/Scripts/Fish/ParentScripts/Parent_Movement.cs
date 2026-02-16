using UnityEngine;

public class Parent_Movement : MonoBehaviour
{

    // --------------------------------- Sprite ---------------------------------
    [SerializeField] protected Transform sprite_transform;   //get transform of pet sprite
    protected float startTime;
    protected float h_turningSpeed = 1.5f;
    protected float y_angle = 0;


    // --------------------------------- Targeting ---------------------------------
    protected Vector3 idleTarget;
    protected float targetRadius = 0.5f;
    protected float newTargetMinLengthRadius = 6; //the minimum length away from our fish current position
    protected float idle_velocity = 1;


    //references
    protected Rigidbody2D rb;

    //used in update position function for determining if we are going to use addforce or just update litteral position
    protected bool IStatic = true;
    //same as above but for part 2 of code: profile vs non profiled viewd fish code
    protected bool IProfile = true;



    protected void Start()
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
        rb.AddForce(dir * current_Vel * Time.deltaTime, ForceMode2D.Force);

       
    }



    //create a new idle target, that is within the tank dimensions and outside the fish range.
    protected virtual void NewRandomIdleTarget_Tank()
    {

        //since new target
        NewTargetVariables();

        //new target
        var curr_pos = new Vector3(transform.position.x, transform.position.y, 0);

        //tanke dememsions
        float[] swimDem = TankBoundries.instance.spawn_arr;

        //get a new target area thats far from this guppy
        while (Mathf.Abs(Vector2.Distance(idleTarget, curr_pos)) < newTargetMinLengthRadius)
        {

            idleTarget = new Vector3(
                Random.Range(swimDem[0], swimDem[1]),
                Random.Range(swimDem[2], swimDem[3]),
                0
            );
        }

    }



    //whenever a new target is set we reset our sprite variables
    protected virtual void NewTargetVariables()
    {
        startTime = Time.time;      //reset our turning time for lerp
    }



    //Guppy will just do a roam mode but just fast and consistent
    public void PanicMode()
    {

    }

    //Guppy is activly heading towards a food pellet
    public void HungryMode()
    {

    }

    //Guppy is just chilling and activly swimming to next destination
    public void RoamMode()
    {

    }

    //is just stationary, will conver into roam mode after it finishes.
    public void IdleMode()
    {

    }






    protected void OnDrawGizmosSelected()
    {

        //if idle is null, dont show it
        if (idleTarget == null || idleTarget == new Vector3(0, 0, 0))
        {
            //dont show
        }
        else
        {
            //current target for fish
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(idleTarget, targetRadius);
        }


        //current range untill new target
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, newTargetMinLengthRadius);



    }
}
