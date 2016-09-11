using UnityEngine;

public static class Common {

    public static float CheckDistanceFromPlayer(Transform checkObj)
    {
        Debug.Log(Vector2.Distance(checkObj.position, GameManager.Instance.MainPlayer.position));
        return Vector2.Distance(checkObj.position, GameManager.Instance.MainPlayer.position);
    }
	

}
