using Assests.Inputs;
using UnityEngine;

public class Test_Movement : Parent_Movement
{
    public float burst_velocity = 3;

    //go to parent_movement to kow
    private float test_curr_SwimLerpSecs = 0;
    private float test_curr_SwimDegree = 0;
    private float test_curr_SwimAnimationSpeed = 0;
    private int test_curr_SwimDir = 1;
    private int test_start_SwimAnimeSpd = 0;
    [Space(10)]

    public int test_max_SwimDegree = 20;
    public float test_necro_swimSpeed = 0.2f;
    public int test_set_SwimanSPEd = 0;




    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Pressed");
            var dir = (CustomVirtualCursor.GetMousePosition_V2() - (Vector2)transform.position).normalized;
            rb.AddForce(dir * burst_velocity, ForceMode2D.Impulse);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            test_start_SwimAnimeSpd = test_set_SwimanSPEd;
            test_curr_SwimLerpSecs = 0;
        }

        SwimmingRotation();

        test_curr_SwimLerpSecs += test_necro_swimSpeed * Time.deltaTime;
        test_curr_SwimLerpSecs = Mathf.Clamp(test_curr_SwimLerpSecs, 0, 1);

    }



    protected override void SwimmingRotation()
    {

        //we are not turning, so we can do a swimming animation
        //since we are just messing with the Y, we have to keep the  x and Z the same
        Quaternion newSwim = end_TurningVector;
        Vector3 temp = newSwim.eulerAngles;

        //now update Y
        test_curr_SwimDegree += test_curr_SwimAnimationSpeed * test_curr_SwimDir * Time.deltaTime;

        //now if  we reach max turning , we want to start doing the other way
        if (Mathf.Abs(test_curr_SwimDegree) >= test_max_SwimDegree)
        {
            test_curr_SwimDir = (int)Mathf.Clamp(test_curr_SwimDegree, -1, 1) * -1;
        }

        //update swimspeed/ give falloff
        //test_curr_SwimAnimationSpeed = Mathf.Lerp(test_start_SwimAnimeSpd, 0, test_curr_SwimLerpSecs);
        test_curr_SwimAnimationSpeed = test_start_SwimAnimeSpd * (-1 * Mathf.Pow(test_curr_SwimLerpSecs, 2) + 1);
        Debug.Log("newSPd: "+ test_curr_SwimAnimationSpeed);

        //now update fish transform
        temp.y = temp.y + test_curr_SwimDegree;
        newSwim.eulerAngles = temp;
        transform.rotation = newSwim;
    }
}
