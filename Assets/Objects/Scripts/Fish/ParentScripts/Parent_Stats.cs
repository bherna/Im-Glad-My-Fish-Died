using UnityEngine;

public class Parent_Stats : MonoBehaviour
{

    //Guess we only add variables that
    //Every Fish in this tank will need,
    //like health
    protected int curr_health = 0; // dont think will do decimal health cause that seems excessive

    
    protected void TakeDamage(int damage)
    {
        curr_health -= damage;

        if(curr_health <= 0)
        {
            Died();
        }
    }


    //seperate Dieing out, cause there could be instant kill enemies/pets/other
    protected virtual void Died()
    {

    }
}
