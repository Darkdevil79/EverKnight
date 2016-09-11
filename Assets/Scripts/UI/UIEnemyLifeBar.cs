using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIEnemyLifeBar : MonoBehaviour {

    public Slider EnemyLifeBar;

    public LivingEntity LinkedLE;

    int lastHPValue;
    RectTransform CanvasRect;
    RectTransform rtEnemyLifeBar;

    void Start()
    {
        CanvasRect = UIManager.Instance.GameUICanvas.GetComponent<RectTransform>();
        rtEnemyLifeBar = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (LinkedLE != null)
        {
            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(LinkedLE.transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

            rtEnemyLifeBar.anchoredPosition = WorldObject_ScreenPosition + UIManager.Instance.DisplayOffset;
        }

    }

    void FixedUpdate()
    {
        if (LinkedLE != null)
        {
            EnemyLifeBar.gameObject.SetActive(Vector2.Distance(LinkedLE.transform.position, GameManager.Instance.MainPlayer.position) <= UIManager.Instance.DisplayEnemyLBAfter);

            if (lastHPValue != LinkedLE.HitPoints)
            {
                if (LinkedLE.HitPoints == EnemyLifeBar.maxValue || LinkedLE.HitPoints == 0)
                {
                    EnemyLifeBar.maxValue = LinkedLE.maxHitpoint;
                    EnemyLifeBar.value = LinkedLE.HitPoints;
                }
                else
                {
                    EnemyLifeBar.maxValue = LinkedLE.maxHitpoint;
                    EnemyLifeBar.value = Mathf.Lerp(LinkedLE.HitPoints, lastHPValue, Time.deltaTime * 1f);
                }

                lastHPValue = LinkedLE.HitPoints;
            }

            if (LinkedLE.isDead)
            {
                Destroy(this.gameObject);
            }

           
        }

    }
}
