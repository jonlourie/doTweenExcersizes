using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BouncingCube : MonoBehaviour
{
    [SerializeField]
    private bool changeColor = default;

    [SerializeField]
    private List<Waypoints> waypoints = new List<Waypoints>(); //exposed to the editor so we can initialize each waypoint in the list

    [SerializeField]
    private float jumpPower;

    [SerializeField]
    private int numJumps;

    [SerializeField]
    private float duration;

    [SerializeField]
    private float colorChangeDuratrion;

    [SerializeField]
    private Vector3 cubeScale;

    [SerializeField]
    private float scaleDuration;

    private Material cubeMaterial;
    private Waypoints currentWaypoint;
    private Waypoints previousWaypoint;
    private Sequence _sequence; // a specific sequence type variable intended to kill a squence upon pause button hit


    // Start is called before the first frame update
    void Start()
    {
        cubeMaterial = GetComponent<MeshRenderer>().material;
        currentWaypoint = waypoints[0];


        //have yet to add more funbctionality we figured out how to move the sequence if we wanted we could a set a loop for the first
        //_assignmnet but now we have to take advantage of our list and figure out how to move the cube to multiple points randomly and indefinatley
        //_there are multiple ways to do this

        // if we were to just put ther sequnce class and method itself it would run because it seems sequences autimatically run in dotween i think this is because the method in question returns a sequence
        // _sequence = MoveToWaypointAndChangeColor(currentWaypoint);
        //_sequence.IsPlaying(); 
        //^^ setting a waypoint target.postition from our list of waypoints this is the initial way I got the sequence to execute and then be killed with space bar

        MoveSomewhereIndefintley(currentWaypoint);


    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(_sequence != null)
            {
                _sequence.Kill();//kill method can be added to any tween and will instnalty all twweens i beleive 
                _sequence = null; // this is how we set the sequnece to null 
                MoveToWaypointAndChangeColor(previousWaypoint);
                // this will just move us to some random position - i dont think 

            }
            else
            {
                MoveSomewhereIndefintley(previousWaypoint);
                // move back to opriginal position which is eqaul to waypoint in the assigned method - if the sequence is = null
            }
            
        }
    }

    private void MoveSomewhereIndefintley(Waypoints waypoint) //the waypoint value chjanges after the ending of this function
    {
        int randomVariation = Random.Range(0, waypoints.Count); //we are going to define our randomVariation of waypoints in this method and feed it into our GetNextWaypoin()_

        Waypoints nextWaypoint = GetNextWayPoint(waypoint, randomVariation);
        previousWaypoint = waypoint;

        _sequence = MoveToWaypointAndChangeColor(nextWaypoint);

        //_sequence.SetLoops(-1, LoopType.Restart); // here is  loop to experiemnt 

        _sequence.OnComplete(() => //I guess this lamda captures "this" as in this method and the nextWaypoint, this could be reactrive programming use case, clearly this is how we get looping effects however I wonder if we could simply put a loop here
        {
            MoveSomewhereIndefintley(nextWaypoint);

            //now that we are looping this method with oncomplete - our previousWaypoint as defined in the method is eqaul to whatever our nextWayPoint value was
            //on complete is essentially a callback - oine trhe movement is completed I want to have something hjappen

        });


    }

    private Sequence MoveToWaypointAndChangeColor(Waypoints waypoint) //the sequence of Tweens i.e animations will execute upon reaching the Waypoint targert
    {
        var sequence = DOTween.Sequence();

        sequence.Append(transform.DOJump(waypoint.transform.position, jumpPower, numJumps, duration)
            .SetEase(Ease.Linear));

        //might encapsulate in the boolean if variable change color

        sequence.Append(cubeMaterial.DOColor(waypoint.TargetColor, colorChangeDuratrion)
            .SetEase(Ease.Linear));


        sequence.Append(transform.DOScale(cubeScale, scaleDuration));

        return sequence;



    }

    private Waypoints GetNextWayPoint(Waypoints currentWaypoint, int randomVariation) //the currentWaypoint will change through the index
    {
        var index = waypoints.IndexOf(currentWaypoint);
        int newIndex = index + randomVariation >= waypoints.Count ? index - waypoints.Count + randomVariation : index + randomVariation;//if we get rid of the rest of this copde its going to stop giving us waypoints 
        return waypoints[newIndex]; //should prob be called random  waypoint..


        //essentially the current waypoint index is always changing - and its chanigng within a random range so it will start of at zero and end wherever, however we need to make sure it doesnt exceed the waypoints we have in our list hence why we have that if statment
        //we have a way to make sure our index of waypoints never exceeds the number of waypoints in our list
        // way for us to not exceed the amount of waypoints we have available in us
        

    }

}
