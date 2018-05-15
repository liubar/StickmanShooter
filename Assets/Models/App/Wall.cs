using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool isLeft = false;

    /// <summary>
    ///     Init wall position relative to the screen
    /// </summary>
    void Start()
    {
        var newPos = new Vector3(isLeft ? 0 : Screen.width, Screen.height / 2);
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(newPos);
        transform.position = worldPoint;
    }
}
