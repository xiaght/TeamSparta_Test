using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Image hpSlider;



    public void SetHealth(float currenthp, float maxhp)
    {
        hpSlider.fillAmount = currenthp / maxhp; 
    }
}
