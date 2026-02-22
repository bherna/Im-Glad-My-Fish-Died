using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Just a list of all the different types of food a gupppy will be able to eat
/// not limited to just pellets, but can eat burgers aswell
/// 
/// //I going to have the Pellets seperated by tier, make it easier to track what pellet value is what
/// </summary>
public enum FoodTypes { Pellet_0, Pellet_1, Pellet_2, Burger };

public static class FoodTypes_Class
{

    /// <summary>
    /// also setting the foods into a dictionary to keep track what their value is
    /// </summary>
    public static Dictionary<FoodTypes, int> FoodTypes_Values { get; private set; } = new Dictionary<FoodTypes, int>
    {
        {FoodTypes.Pellet_0, 10 },
        {FoodTypes.Pellet_1, 20 },
        {FoodTypes.Pellet_2, 30 },
        {FoodTypes.Burger, 0 },

    };


}

public class Controller_Food : MonoBehaviour
{

    //----------------------- references ---------------------------
    [SerializeField] GameObject[] foodPellets; // this is the list of the different food pellets this tank has (0 being the first unlock)
    private int curr_foodPelletUnlockIndex = 0; //start with index 0 sinice thats first unlock


    ////----------------------- audios -----------------------------
    [SerializeField] AudioClip createSound;
    [SerializeField] AudioClip destroySound;

    // -------------------------------- privates --------------------------------
    private int maxFood = 3;
    private List<GameObject> foodPellets_list;



    //singleton this class
    public static Controller_Food instance { get; private set; }
    void Awake()
    {

        //delete duplicate of this instance

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //start empy array for food
        foodPellets_list = new List<GameObject>();
    }




    // Update is called once per frame
    void Update()
    {

        

    }



    //used for spawning player placed food pellet
    public void SpawnFood_Pellet(Vector3 pelletPos, bool removeOld)
    {

        if (removeOld)
        {
            //check if at max food,
            //delete oldest
            FoodList_MakeSpace();
        }


        //spawn new
        foodPellets_list.Add(Instantiate(foodPellets[curr_foodPelletUnlockIndex], pelletPos, Quaternion.identity));

    }


    //used for spawning non conventional food pellet (like pet created foods ex: burger)
    //this new food pellet should be created before hand, all we do here is make space in tank and add to foodlist
    public void AddFood_Gameobject(GameObject food_obj, bool removeOld)
    {

        if (removeOld)
        {
            //check if at max food,
            //delete oldest
            FoodList_MakeSpace();
        }

        //add given gameobject to list
        foodPellets_list.Add(food_obj);
    }


    //used in keeping track of pellet count in the tank
    //if we are currently at max food capacity in tank,
    //          delete the oldest food (which is first index)
    private void FoodList_MakeSpace()
    {

        if (foodPellets_list.Count >= maxFood)
        {
            TrashThisFood_ByIndex(0);

            //play sound to know we destroy'd food
            //badd sound effect
            //Controller_FXSoundsManager.instance.PlaySoundFXClip(destroySound, transform, 1f, 1f);
        }
        else
        {
            //play good sound effect
            //Controller_FXSoundsManager.instance.PlaySoundFXClip(createSound, transform, 1f, 1f);
        }
    }



        //used in mostly guppys since guppys need to eat
        public List<GameObject> GetAllFood(){
            return foodPellets_list;
        }

        public void TrashThisFood_ByGameObject(GameObject foodObj){

            foodPellets_list.Remove(foodObj);
            Destroy(foodObj);
        }
        private void TrashThisFood_ByIndex(int index)
        {
            Destroy(foodPellets_list[index]);
            foodPellets_list.RemoveAt(index);
        }



        /*
        /// FUNCTIONS FOR OTHER SCRIPTS TO CALL 


        public int GetFoodLength(){
            return foodPellets_list.Count;
        }

        public void Upgrade_FoodMax(){
            maxFood += 1;
        }

        //upgrades food power
        //also returns true if the array is finished
        public void Upgrade_FoodPower(){
            //increment array index
            index_foodPelletType++;
        }
        */
    
}
