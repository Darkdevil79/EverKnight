using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerLifeBar : MonoBehaviour {

    public Text PotionsCount;
    public Slider PlayerLifeBar;
    public Text PlayerLifeBarValue;

    public LivingEntity LinkedLE;

    int lastHPValue;
	
	void FixedUpdate () {

        if (LinkedLE != null)
        {
            if (lastHPValue != LinkedLE.HitPoints)
            {
                if (LinkedLE.HitPoints == PlayerLifeBar.maxValue || LinkedLE.HitPoints == 0)
                {
                    PlayerLifeBar.maxValue = LinkedLE.maxHitpoint;
                    PlayerLifeBar.value = LinkedLE.HitPoints;
                }
                else
                {
                    PlayerLifeBar.maxValue = LinkedLE.maxHitpoint;
                    PlayerLifeBar.value = Mathf.Lerp(LinkedLE.HitPoints, lastHPValue, Time.deltaTime * 1f);
                }



                if (PlayerLifeBarValue != null)
                {
                    PlayerLifeBarValue.text = string.Format("{0} / {1}", PlayerLifeBar.value.ToString("###"), PlayerLifeBar.maxValue.ToString("###"));
                }

                lastHPValue = LinkedLE.HitPoints;
                Debug.Log("Updated HP");
            }
        }

	}
}
