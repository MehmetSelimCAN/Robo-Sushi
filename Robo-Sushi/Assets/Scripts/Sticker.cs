using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticker : MonoBehaviour {

    private Vector3 firstPosition;
    private Transform wayPoints;
    private float maximumAcceptableDistance;
    private int index;
    private LayerMask stickersLayer;
    private LayerMask sushisLayer;

    private void Awake() {
        index = 0;
        maximumAcceptableDistance = 0.75f;
        firstPosition = transform.position;
        wayPoints = GameObject.Find("Waypoints").transform;
        stickersLayer = LayerMask.GetMask("Sticker");
        sushisLayer = LayerMask.GetMask("Sushi");
    }

    private void OnMouseDown() {
        if (GameManager.isGamePaused) {
            transform.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
    }

    private void OnMouseDrag() {
        if (GameManager.isGamePaused) {
            transform.position = GetMousePosition();
        }
    }

    private void OnMouseUp() {
        index = 0;
        for (int i = 0; i < wayPoints.childCount; i++) {
            if (Vector2.Distance(transform.position, wayPoints.GetChild(i).transform.position) < maximumAcceptableDistance) {
                Collider2D[] stickersAround = Physics2D.OverlapCircleAll(transform.position, 0.4f, stickersLayer);
                Collider2D[] sushisAround = Physics2D.OverlapCircleAll(transform.position, 0.4f, sushisLayer);
                if (stickersAround.Length - 1 == 0 && sushisAround.Length == 0) {
                    transform.position = wayPoints.GetChild(i).transform.position;
                    transform.GetComponent<SpriteRenderer>().sortingOrder = -1;
                    transform.SetParent(null);
                    break;
                }
                else {
                    //if there is no available position for sticker, because of the stickers or sushis.
                    transform.position = firstPosition;
                }
            }
            index++;
        }

        //if there is no available position for sticker, because of the range.
        if (index == wayPoints.childCount) {
            transform.position = firstPosition;
            transform.GetComponent<SpriteRenderer>().sortingOrder = 0;
            transform.SetParent(GameObject.Find("Stickers").transform);
        }
    }

    private Vector3 GetMousePosition() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return mousePos;
    }
}
