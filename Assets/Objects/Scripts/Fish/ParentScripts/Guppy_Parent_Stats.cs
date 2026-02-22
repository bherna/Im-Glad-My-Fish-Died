using UnityEngine;

public class Guppy_Parent_Stats : Parent_Stats
{
    //references
    Guppy_Parent_SM guppy_Parent_SM;
    [SerializeField] SpriteRenderer guppySpriteRender;

    // --------------------------------- hunger related ----------------------------------------//
    protected float curr_stomach;               //current stomach fill, if this reaches 0, guppy should die of starvation
    private float burnRate = 1;                 //keep at 1, just so have it reference as -1 unit per second, or can be upped on certain condiions
    protected int threshold_StartHunger;          //at what number of a stomach will the guppy be in starvation mode

    //these are just used in changing the color of the fish depending on the situation
    private Color hungryColor = new Color(0.63f, 0.66f, 1f); //
    private Color fullColor = new Color(1, 1, 1);
    //maybe somethiing like StunnedColor, for when guppys are stunned



    protected virtual void Start()
    {
        guppy_Parent_SM = GetComponent<Guppy_Parent_SM>();
    }



    void Update()
    {
        //burn stomach
        curr_stomach -= burnRate * Time.deltaTime;

        //check if fish starved to death
        if (curr_stomach <= 0)
        {
            Died();
        }
        //if guppy became hungry
        else if (curr_stomach < threshold_StartHunger && guppy_Parent_SM.guppy_current_state != Guppy_States.Hungry)
        {

            GuppyHungry();
        }
    }


    protected virtual void GuppyHungry()
    {

        //guppy is now hungry
        guppy_Parent_SM.GuppyToState(Guppy_States.Hungry);

        //announce to all pets that we are hungry
        //Controller_Pets.instance.Annoucement_Init(Event_Type.guppyHungry, gameObject);

        //change sprite transparancy
        SetGuppyColor(hungryColor);
    }



    //here where setting the guppy's hunger color
    //we don't really need to edit the transperency since this messes with the alpha cutoff, making
    //the guppy insvisiable
    private void SetGuppyColor(Color setColor)
    {

        //for each sprite that is part of this fish
        //we have to check if its a skinned messrender, or a simple meshrender
        /*
        foreach (Transform sprite in sprite_meshList)
        {


            var mesh = sprite.GetComponent<MeshRenderer>();
            if (mesh != null)
            {

                mesh.material.SetColor("_BaseColor", setColor);

            }
            else
            {

                var skindMesh = sprite.GetComponent<SkinnedMeshRenderer>();
                if (skindMesh != null)
                {

                    skindMesh.material.SetColor("_BaseColor", setColor);
                }
            }


        }
        */

        guppySpriteRender.material.SetColor("_BaseColor", setColor);

    }



    //used outside this script
    //mostly in the collision script,
    public virtual void GuppyEated(FoodTypes foodType)
    {


        //now for the typpe of specific food
        switch (foodType)
        {
            case FoodTypes.Pellet_0:
            case FoodTypes.Pellet_1:
            case FoodTypes.Pellet_2:
                //eating ages guppy
                //Ate();
                //update fish stomach to add food value
                curr_stomach += FoodTypes_Class.FoodTypes_Values[foodType];
                break;

            case FoodTypes.Burger:
                break;
        }


        //only exit hunger mode if the guppy 
        //return color to fish
        SetGuppyColor(fullColor);
        //set our state to idle again
        guppy_Parent_SM.GuppyToState(Guppy_States.Idle);


    }


}
