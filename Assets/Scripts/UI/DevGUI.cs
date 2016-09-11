using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DevGUI : MonoBehaviour
{
    public Animator anim;

    void OnGUI()
    {
        //if (Application.loadedLevelName == "Battle") GUI.Label(new Rect(800, 0, 120, 30), BattleManager.Instance.turnState.ToString());
        if (GUI.Button(new Rect(800, 30, 120, 30), "attack1"))
        {
            //if (PlayerControllerOld.Instance.currentAnimation != PlayerAnimations.attack)
            //    PlayerControllerOld.Instance.setAnimation(PlayerAnimations.attack);
        }

    }

}
