using UnityEngine;

public class Guppy_Basic_Stats : Guppy_Parent_Stats
{
    


    protected override void Start()
    {
        base.Start();

        //what value shuold this guppys stomach start at
        curr_stomach = 30;

        //what value should the threshold be at
        threshold_StartHunger = 15; //jus half for now
    }




}
