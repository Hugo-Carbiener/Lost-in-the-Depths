using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Canvas help;

    [Header("Elevator variables")]
    [SerializeField] private int travelDuration;
    [SerializeField] private int loadingDuration;
    private int depth;
    private elevatorState state;
    private enum elevatorState
    {
        atTop,
        atBottom,
        inTravel
    }

    private Vector3 topPosition;
    private Vector3 bottomPosition;

    private Transform player;

    private void Awake()
    {
        topPosition = transform.position;
        bottomPosition = transform.position + Vector3.down * depth;
        state = elevatorState.atTop;
        help.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && state != elevatorState.inTravel)
        {
            help.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.A))
            {
                player = other.transform;
                StartCoroutine("StartTravel");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && state != elevatorState.inTravel)
        {
            help.gameObject.SetActive(false);
        }
    }

    private IEnumerator StartTravel()
    {
        // disable movement for the travel
        player.GetComponent<PlayerMovement>().enabled = false;

        // move player manually to the elevator
        Vector3 playerStartPos = player.position;
        Vector3 playerDestination = new Vector3(transform.position.x, player.position.y, 0);
        player.GetComponentInChildren<Animator>().SetBool("IsRunning", true);

        float timer = 0;
        while(timer < loadingDuration)
        {
            timer += Time.deltaTime;
            player.transform.position = Vector3.Lerp(playerStartPos, playerDestination, timer / loadingDuration);
            yield return null; 
        }

        player.GetComponentInChildren<Animator>().SetBool("IsRunning", false);

        // player is loaded, elevator can move

        Vector3 start;
        Vector3 finish;
        Vector3 playerStart = player.position;
        Vector3 playerFinish;
        elevatorState endState;

        if (state == elevatorState.atTop)
        {
            start = topPosition;
            finish = bottomPosition;
            playerFinish = player.position + Vector3.down * depth;
            endState = elevatorState.atBottom;
        } else // elevator state cannot be "intravel" anyways
        {
            start = bottomPosition;
            finish = topPosition;
            playerFinish = player.position + Vector3.up * depth;
            endState = elevatorState.atTop;
        }

        state = elevatorState.inTravel;
        timer = 0;
        while (timer < travelDuration)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(start, finish, timer/travelDuration);
            player.position = Vector3.Lerp(playerStart, playerFinish, timer / travelDuration);
            yield return null;
        }

        state = endState;

        player.GetComponent<PlayerMovement>().enabled = true;
        
        yield return null;
    }

    public void SetDepth(int d)
    {
        depth = d;
    }
}
