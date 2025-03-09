using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    private static SingletonManager _instance;

    public Player player;
    public EnemySpawner enemyspwa;
    public BackgroundScrolling background;
    public UiManager ui;


    public static SingletonManager Instance
    {
        get
        {


            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(SingletonManager)) as SingletonManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }


        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }



}
