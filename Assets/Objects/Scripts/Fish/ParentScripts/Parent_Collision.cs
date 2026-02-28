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

        
    }
}
