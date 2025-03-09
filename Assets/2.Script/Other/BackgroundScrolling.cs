using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 10f)] float scrollSpeed = 0.5f;

    private Material material;

    void Start()
    {
        scrollSpeed = 0.1f;
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        Vector2 offset = new Vector2(scrollSpeed * Time.deltaTime, 0);
        material.mainTextureOffset += offset;
    }

    public void StopScrolling() {
        scrollSpeed = 0;
        Debug.Log("∏ÿ√„");

    }
}
