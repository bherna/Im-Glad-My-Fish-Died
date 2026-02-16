using UnityEngine;



//this class is designed to just grab boundries that any of the swimmable objects can use.
public class TankBoundries : MonoBehaviour
{


    //reference to tank area collision box
    [SerializeField] Transform swimRange;

    //reference to spawn collision box
    [SerializeField] Transform spawnRange;




    //swim range
    public float[] swim_arr { get;  private set; } = new float[4];

    //spawn range
    public float[] spawn_arr { get; private set; } = new float[4];


    //singleton
    public static TankBoundries instance { get; private set; }
    private void Awake()
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


        GetBoundry(swimRange, swim_arr);
        GetBoundry(spawnRange, spawn_arr);

/*
        Debug.Log(string.Format("Swim: "));
        foreach (float item in swim_arr)
        {

            Debug.Log(string.Format("{0}, ", item));
        }

        Debug.Log(string.Format("Spawn: "));
        foreach (float item in spawn_arr)
        {

            Debug.Log(string.Format("{0}, ", item));
        }
*/
    }

    //stright forward functions, just get the transform size and convert to useable info
    private void GetBoundry(Transform RangeType, float[] _arr)
    {

        float w = RangeType.lossyScale.x;
        float h = RangeType.lossyScale.y;

        var _pos = RangeType.position;

        _arr[0] = _pos.x - w / 2;
        _arr[1] = _pos.x + w / 2;

        _arr[2] = _pos.y - h / 2;
        _arr[3] = _pos.y + h / 2;
    }


}
