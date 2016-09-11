using UnityEngine;
using System.Collections;

public class ScrollingCamera : MonoBehaviour
{
    public float minXLocation;
    public float maxXLocation;
    public float minYLocation;
    public float maxYLocation;

    void Update()
    {
        float mousePositionX = Input.mousePosition.x / Screen.width;
        float mousePositionY = Input.mousePosition.y / Screen.width;

        if (mousePositionX < 0)
            mousePositionX = 0;
        else if (mousePositionX > 1)
            mousePositionX = 1;

        if (mousePositionY < 0)
            mousePositionY = 0;
        else if (mousePositionY > 1)
            mousePositionY = 1;

        float differenceX = this.maxXLocation - this.minXLocation;
        float differenceY = this.maxYLocation - this.minYLocation;

        Vector3 position = this.transform.position;
        position.x = minXLocation + differenceX * mousePositionX;
        position.y = minYLocation + differenceY * mousePositionY;

        this.transform.position = position;
    }
}
