using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] PlayerScore PlayerScore;
    public static ScoreManager Instance;
    public static float Score;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if(PlayerScore != null)
        Score = PlayerScore.likes;
    }
}
