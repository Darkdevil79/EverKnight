using UnityEngine;
using System.Collections;

public class UIManager : MonoSingleton<UIManager> {

    public UIPlayerLifeBar playerLifeBar;

    public Canvas GameUICanvas;
    public Vector2 DisplayOffset = Vector2.zero;

    public float DisplayEnemyLBAfter = 20f;

    void Start () {

        playerLifeBar.LinkedLE = GameManager.Instance.MainPlayer.GetComponent<LivingEntity>();


    }

    void Update () {
	
	}

}
