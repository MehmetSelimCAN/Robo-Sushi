using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    [SerializeField] private float maxTimer;
    private float timer;
    private Image fill;

    private void Awake() {
        fill = transform.Find("fill").GetComponent<Image>();
        timer = maxTimer;
    }

    private void Update() {
        if (GameManager.isGamePaused) {
            return;
        }

        timer -= Time.deltaTime;
        fill.fillAmount = timer / maxTimer;

        if (timer < 0f) {
            GameManager.Reset();
        }
    }

    public void ResetTimer() {
        timer = maxTimer;
    }
}
