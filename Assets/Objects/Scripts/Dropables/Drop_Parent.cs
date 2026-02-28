using UnityEngine;

public class Drop_Parent : MonoBehaviour
{

    [SerializeField] float gravity_Scale = 0.02f; //can be changed in the inpector


    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //nothing so far, just for organizational purposes



    //dont want to put anyting here cause different drops will do different things on touching trash can
    public virtual void OnTrashDrop() { }



    //so far only used on touching the ground, we dont want to go past the ground
    //child drop types will probably have other additions, like food getting destroyed
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Trash Can"))
        {
            //when touch trash can
            //stop moving down, so just set gravity to zero
            rb.gravityScale = 0;

            //also set y velocity to zero cause we still moving
            rb.linearVelocityY = 0;

            //will have to update gravity scale when we exit trash can

            //also run trash drop as a funtion here so we have a way to add more 
            OnTrashDrop();
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Trash Can"))
        {
            //reset gravity
            rb.gravityScale = gravity_Scale;

        }
    }
}
