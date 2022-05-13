using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTypeHolder : MonoBehaviour {

    [SerializeField] private ColorType colorType; 

    public enum ColorType {
        Yellow,
        Red,
        Green
    }

    public ColorType GetColorType() {
        return colorType;
    }
}
