using System.Collections;
using UnityEngine;

public class Drop_Food : Drop_Parent
{

    [SerializeField] FoodTypes foodType; //this is set in the inspector

    protected const float total_time = 1f; //how long before this food disapears


    public FoodTypes Get_foodType()
    {
        return foodType;
    }


    protected override void OnTrashDrop()
    {
        base.OnTrashDrop();

        StartCoroutine(Countdown());
    }


    //since we want to wait before destroying this food, we run it this way
    IEnumerator Countdown()
    {
        /*eventually will make the food fade away to add some animation to it
         * 
            Color c = renderer.material.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            renderer.material.color = c;
            // Wait for 0.1 seconds before the next iteration
            yield return new WaitForSeconds(.1f);
    }
         
         */
        yield return new WaitForSeconds(total_time);

        //event to food controller
        Controller_Food.instance.TrashThisFood_ByGameObject(gameObject);
    }




    //this function is different than ontrashdrop but is essentially the same, 
    //mostly used by other fish to eat this food
    public void OnFoodEated()
    {
        //event to food controller
        Controller_Food.instance.TrashThisFood_ByGameObject(gameObject);
    }
}
