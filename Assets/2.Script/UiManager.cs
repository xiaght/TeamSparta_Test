using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI timeScale;

    public TextMeshProUGUI timeScale_1;
    public GameObject dieUi;


    public Button attackSpeedBoostButton; // ✅ 버튼 연결
    public Image cooldownOverlay; // ✅ 쿨타임 표시용 이미지
    public float boostDuration = 3f; // ✅ 공격 속도 증가 지속 시간
    public float cooldownDuration = 5f; // ✅ 쿨타임 지속 시간
    private bool isCooldown = false; // ✅ 쿨타임 여부 체크

    private SpriteRenderer spriteRenderer;




    public void TimeSpeedToogle() {
        if (Time.timeScale == 1f)
        {

            Time.timeScale = 2;
            timeScale.text = "x2";
        }
        else if (Time.timeScale == 2f) {
            Time.timeScale = 3;

            timeScale.text = "x3";
        }
        else
        {
            Time.timeScale = 1f;

            timeScale.text = "x1";
        }

    }
    public void ActivateAttackSpeedBoost()
    {
        if (!isCooldown)
        {
            StartCoroutine(AttackSpeedBoostRoutine());

            StartCoroutine(StartCooldown());
        }
    }

    IEnumerator AttackSpeedBoostRoutine()
    {
        // ✅ 공격 속도 2배 증가

        SingletonManager.Instance.player.shotspeed /= 2f;
        Debug.Log("공격 속도 증가!");

        yield return new WaitForSeconds(boostDuration); // ✅ 3초 동안 유지

        // ✅ 원래 속도로 복구
        SingletonManager.Instance.player.shotspeed *= 2f;
        Debug.Log("공격 속도 복구");

        // ✅ 5초 동안 쿨타임 적용
    }

    IEnumerator StartCooldown()
    {
        isCooldown = true;
        attackSpeedBoostButton.interactable = false; // ✅ 버튼 비활성화
        float elapsedTime = 0f;

        while (elapsedTime < cooldownDuration)
        {
            elapsedTime += Time.deltaTime;
            cooldownOverlay.fillAmount = 1 - (elapsedTime / cooldownDuration); // ✅ 쿨타임 진행 표시
            yield return null;
        }

        isCooldown = false;
        attackSpeedBoostButton.interactable = true; // ✅ 버튼 다시 활성화
        cooldownOverlay.fillAmount = 0; // ✅ 쿨타임 UI 초기화
    }


    public void OnDieUi() {
        dieUi.SetActive(true);
        Time.timeScale = 0;
    }

    public void ExitGame() {
        UnityEditor.EditorApplication.isPlaying = false;

    }

    public void ReStart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
