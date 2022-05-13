using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiMovement : MonoBehaviour {

    private Vector3 firstPosition;
    private string firstTag;
    private Sprite firstSprite;

    private Transform wayPoints;
    private int firstWayPointIndex;
    [HideInInspector] public int nextWayPointIndex;
    [HideInInspector] public int currentWayPointIndex;
    private bool rotateClockWise;
    public static SushiMovement[] sushis;
    private float speed;

    private SpriteRenderer spriteRenderer;
    private Sprite yellowSushi;
    private Sprite redSushi;
    private Sprite greenSushi;

    [HideInInspector] public bool canMove;
    private bool switchTriggered;

    private void Awake() {
        canMove = true;

        spriteRenderer = transform.Find("sushi").GetComponent<SpriteRenderer>();
        yellowSushi = Resources.Load<Sprite>("Sprites/Sushis/YellowSushi");
        redSushi = Resources.Load<Sprite>("Sprites/Sushis/RedSushi");
        greenSushi = Resources.Load<Sprite>("Sprites/Sushis/GreenSushi");

        firstTag = transform.tag;
        firstSprite = spriteRenderer.sprite;
        firstPosition = transform.position;

        speed = 2f;
        rotateClockWise = true;

        wayPoints = GameObject.Find("Waypoints").transform;
        float distance = Mathf.Infinity;
        for (int i = 0; i < wayPoints.childCount; i++) {
            if (Vector2.Distance(transform.position, wayPoints.GetChild(i).transform.position) < distance) {
                distance = Vector2.Distance(transform.position, wayPoints.GetChild(i).transform.position);
                firstWayPointIndex = i;
            }
        }

        currentWayPointIndex = firstWayPointIndex;

        if (firstWayPointIndex == wayPoints.childCount - 1) {
            nextWayPointIndex = 0;
        }
        else {
            nextWayPointIndex = firstWayPointIndex + 1;
        }

        sushis = FindObjectsOfType<SushiMovement>();
    }

    private void Update() {
        if (GameManager.isGamePaused || !canMove) {
            return;
        }

        for (int i = 0; i < sushis.Length; i++) {
            //If there is another sushi on the next waypoint, it waits.
            if (sushis[i].currentWayPointIndex == nextWayPointIndex) {
                return;
            }

            //If two sushis are on their way at the same time to the same waypoint...
            //...there is a priority, Green -> Red -> Yellow
            if (sushis[i].nextWayPointIndex == nextWayPointIndex && sushis[i].gameObject != transform.gameObject) {
                if (sushis[i].tag == "YellowSushi" && transform.tag == "RedSushi") {

                }

                else if (sushis[i].tag == "YellowSushi" && transform.tag == "GreenSushi") {

                }

                else if (sushis[i].tag == "RedSushi" && transform.tag == "GreenSushi") {

                }

                //If two same color sushis are on the same way, the rotating clockwise sushi is prior
                else if (sushis[i].tag == transform.tag) {
                    if (rotateClockWise) {
                        //continue;
                    }
                }

                //If this sushi doesn't have a higher priority than the others, it can't move.
                else {
                    return;
                }
            }
        }

        //moving to the next waypoint
        transform.position = Vector2.MoveTowards(transform.position, wayPoints.GetChild(nextWayPointIndex).transform.position, speed * Time.deltaTime);

        //if it has arrived at the next waypoint
        if (Vector2.Distance(transform.position, wayPoints.GetChild(nextWayPointIndex).transform.position) < 0.01f) {
            if (switchTriggered) {
                switchTriggered = false;

                rotateClockWise = !rotateClockWise;
                if (!rotateClockWise) {
                    currentWayPointIndex = nextWayPointIndex + 1;
                }
                else {
                    currentWayPointIndex = nextWayPointIndex - 1;
                }
            }

            if (rotateClockWise) {
                if (nextWayPointIndex == wayPoints.childCount - 1) {
                    nextWayPointIndex = 0;
                    currentWayPointIndex = wayPoints.childCount - 1;
                }
                else {
                    nextWayPointIndex++;
                    currentWayPointIndex = nextWayPointIndex - 1;
                }
            }

            else if (!rotateClockWise) {
                if (nextWayPointIndex == 0) {
                    nextWayPointIndex = wayPoints.childCount - 1;
                    currentWayPointIndex = 0;
                }
                else {
                    nextWayPointIndex--;
                    currentWayPointIndex = nextWayPointIndex + 1;
                }
            }
        }
    }

    public void ResetPosition() {
        transform.gameObject.SetActive(true);

        transform.tag = firstTag;
        spriteRenderer.sprite = firstSprite;
        transform.position = firstPosition;
        currentWayPointIndex = firstWayPointIndex;

        rotateClockWise = true;
        canMove = true;


        if (firstWayPointIndex == wayPoints.childCount - 1) {
            nextWayPointIndex = 0;
        }
        else {
            nextWayPointIndex = firstWayPointIndex + 1;
        }

        GetComponent<Animator>().Play("Idle");
    }

    #region Color
    private void ChangeColor(ColorTypeHolder.ColorType colorType) {
        if (colorType == ColorTypeHolder.ColorType.Yellow) {
            PaintYellow();
        }

        else if (colorType == ColorTypeHolder.ColorType.Red) {
            PaintRed();
        }

        else if (colorType == ColorTypeHolder.ColorType.Green) {
            PaintGreen();
        }
    }

    private void PaintYellow() {
        transform.tag = "YellowSushi";
        spriteRenderer.sprite = yellowSushi;
    }

    private void PaintRed() {
        transform.tag = "RedSushi";
        spriteRenderer.sprite = redSushi;
    }

    private void PaintGreen() {
        transform.tag = "GreenSushi";
        spriteRenderer.sprite = greenSushi;
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!GameManager.isGamePaused) {
            if (collision.tag == "Switch") {
                switchTriggered = true;
            }

            if (collision.tag == "ColorChanger") {
                ChangeColor(collision.GetComponent<ColorTypeHolder>().GetColorType());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!GameManager.isGamePaused) {
            if (collision.tag == "Switch") {
                switchTriggered = false;
            }
        }
    }
}
