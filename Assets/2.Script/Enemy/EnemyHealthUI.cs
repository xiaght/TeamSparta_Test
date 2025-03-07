using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Image hpSlider;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }


    public void SetHealth(float currentHealth, float maxHealth)
    {
        hpSlider.fillAmount = currentHealth / maxHealth; 
    }
}
