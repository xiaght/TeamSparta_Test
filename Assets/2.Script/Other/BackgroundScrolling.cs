using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 10f)] float scrollSpeed = 0.5f;

    // Quad의 Material을 저장할 변수
    private Material material;

    void Start()
    {
        // Renderer에서 Material 가져오기
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        // X축 방향으로 스크롤 오프셋 계산
        Vector2 offset = new Vector2(scrollSpeed * Time.deltaTime, 0);

        // Material의 메인 텍스처 오프셋 값을 변경하여 스크롤 효과 적용
        material.mainTextureOffset += offset;
    }
}
