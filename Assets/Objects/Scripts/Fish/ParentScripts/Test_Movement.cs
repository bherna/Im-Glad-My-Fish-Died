using Assests.Inputs;
using UnityEngine;

public class Test_Movement : Parent_Movement
{
    public float testVel = 3;

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Pressed");
            var dir = (CustomVirtualCursor.GetMousePosition_V2() - (Vector2)transform.position).normalized;
            rb.AddForce(dir * testVel, ForceMode2D.Impulse);
        }
    }

}
