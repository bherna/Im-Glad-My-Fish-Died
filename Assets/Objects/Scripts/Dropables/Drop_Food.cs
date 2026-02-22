using UnityEngine;

public class Drop_Food : Drop_Parent
{

    [SerializeField] FoodTypes foodType; //this is set in the inspector


    public FoodTypes Get_foodType()
    {
        return foodType;
    }


    public override void OnTrashDrop()
    {
        base.OnTrashDrop();

        //event to food controller
        Controller_Food.instance.TrashThisFood_ByGameObject(gameObject);
    }
}
