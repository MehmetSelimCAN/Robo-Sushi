using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotLine : MonoBehaviour {

    [SerializeField] private MoveDirection moveDirection;
    private Vector2 destination;
    private bool moving;
    private float movingSpeed;

    private Vector3 firstPosition;

    public enum MoveDirection {
        Upwards,
        Downwards,
        Right,
        Left
    }

    private void Awake() {
        movingSpeed = 10f;
        firstPosition = transform.position;
    }

    private void Update() {
        if (moving) {
            transform.position = Vector2.MoveTowards(transform.position, destination, movingSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, destination) < 0.01f) {
                moving = false;
            }
        }
    }

    public void MoveRobots() {
        moving = true;
        if (moveDirection == MoveDirection.Upwards) {
            destination = transform.position + Vector3.up;
        }

        else if (moveDirection == MoveDirection.Downwards) {
            destination = transform.position + Vector3.down;
        }

        else if (moveDirection == MoveDirection.Right) {
            destination = transform.position + Vector3.right;
        }

        else if (moveDirection == MoveDirection.Left) {
            destination = transform.position + Vector3.left;
        }
    }

    public void ResetPosition() {
        moving = false;
        transform.position = firstPosition;
    }

}
