using UnityEngine;

public static class Common {

    public static float CheckDistanceFromPlayer(Transform checkObj)
    {
        return Vector2.Distance(checkObj.position, GameManager.Instance.MainPlayer.position);
    }
	

}
