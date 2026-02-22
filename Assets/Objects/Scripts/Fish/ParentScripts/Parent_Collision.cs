using UnityEngine;

public class Parent_Collision : MonoBehaviour
{


    Parent_Movement parent_Movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        parent_Movement = GetComponent<Parent_Movement>();
    }

    
  

    protected virtual void OnTriggerStay2D(Collider2D other)
    {

        //----------------- Edge Collision -----------------------
        //when we are making contact with the tank edge, we are just going to run this
        if(other.CompareTag("Boundry") || other.CompareTag("Trash Can"))
        {
            Debug.Log("TOuhcing TIPS");
            parent_Movement.TankEdgeReached(other);
        }


        /*
        //----------------- FOOD ---------------------------------
        //if fish is hungry and we collided with food
        if (Guppy_States.hungry == guppy_SM.guppy_current_state && other.gameObject.CompareTag("Food"))
        {
            //eat + destroy obj
            var foodscript = other.GetComponent<Drop_Food>();
            switch (foodscript.foodType)
            {

                case FoodTypes.feed:
                    var foodValue = foodscript.GetFoodValue();
                    Controller_Food.instance.TrashThisFood(other.gameObject);
                    guppy_Stats.GuppyEated(foodValue);
                    break;

                case FoodTypes.burger:
                    //dont need to get food value, since its just a birthday
                    Controller_Food.instance.TrashThisFood(other.gameObject);
                    guppy_Stats.GuppyBurgered();
                    break;

                default:
                    Debug.Log("No food type selected/found");
                    break;
            }
        }
        */
    }
}
