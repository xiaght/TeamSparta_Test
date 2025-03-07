using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 10f)] float scrollSpeed = 0.5f;

    // Quad�� Material�� ������ ����
    private Material material;

    void Start()
    {
        // Renderer���� Material ��������
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        // X�� �������� ��ũ�� ������ ���
        Vector2 offset = new Vector2(scrollSpeed * Time.deltaTime, 0);

        // Material�� ���� �ؽ�ó ������ ���� �����Ͽ� ��ũ�� ȿ�� ����
        material.mainTextureOffset += offset;
    }
}
