using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Image hpSlider; // HP 바 (UI)
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // 메인 카메라 찾기
    }

    void Update()
    {
        // ✅ UI가 항상 카메라를 바라보도록 설정 (빌보드 효과)
     //   transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        hpSlider.fillAmount = currentHealth / maxHealth; // ✅ HP 비율 적용
    }
}
