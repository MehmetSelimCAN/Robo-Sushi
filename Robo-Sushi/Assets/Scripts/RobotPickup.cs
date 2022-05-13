using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPickup : MonoBehaviour {

    public static SushiMovement[] sushis;
    [SerializeField] private RobotColor robotColor;

    private Vector3 firstPosition;
    private Transform parent;

    private bool pickedUp;

    public enum RobotColor {
        Yellow,
        Red,
        Green
    }

    private void Awake() {
        sushis = FindObjectsOfType<SushiMovement>();
        firstPosition = transform.localPosition;
        if (transform.parent != null ) {
            parent = transform.parent;
        }
    }

    private void Update() {
        for (int i = 0; i < sushis.Length; i++) {
            if (!pickedUp && Vector2.Distance(transform.position, sushis[i].transform.position) < 1.01f && sushis[i].tag == (robotColor.ToString() + "Sushi")) {
                if (sushis[i].GetComponent<SushiMovement>().canMove) {
                    pickedUp = true;
                    GetComponent<Animator>().Play("RobotDissolve");

                    sushis[i].gameObject.GetComponent<Animator>().Play("SushiDissolve");
                    sushis[i].GetComponent<SushiMovement>().canMove = false;
                    sushis[i].GetComponent<SushiMovement>().nextWayPointIndex = -1;
                    sushis[i].GetComponent<SushiMovement>().currentWayPointIndex = -1;
                    GameManager.IncreaseNumberOfSushi();
                }
            }
        }
    }

    public void ResetPosition() {
        pickedUp = false;

        GetComponent<Animator>().Play("Idle");

        transform.localPosition = firstPosition;
        if (parent != null) {
            transform.parent = parent;
        }
    }

    public void MoveRobots() {
        if (transform.parent != null) {
            transform.parent.GetComponent<RobotLine>().MoveRobots();
        }
    }
}
