using UnityEngine;

public class Guppy_Parent_Collision : Parent_Collision
{
    Guppy_Parent_SM guppy_Parent_SM;
    Guppy_Parent_Stats guppy_Parent_Stats;

    protected override void Start()
    {
        base.Start();

        guppy_Parent_SM = GetComponent<Guppy_Parent_SM>();
        guppy_Parent_Stats = GetComponent<Guppy_Parent_Stats>();
    }



    protected override void OnTriggerStay2D(Collider2D other)
    {
        //this has tank edge stuff
        base.OnTriggerStay2D(other);
        

        //----------------- FOOD ---------------------------------
        //if fish is hungry and we collided with food
        if (Guppy_States.Hungry == guppy_Parent_SM.guppy_current_state && other.gameObject.CompareTag("Food"))
        {
            //eat + destroy obj
            var foodscript = other.GetComponent<Drop_Food>();
            guppy_Parent_Stats.GuppyEated(foodscript.Get_foodType());
            other.GetComponent<Drop_Food>().OnFoodEated(); //probably should be last since this is a DESTROY func
        }


    }
}
