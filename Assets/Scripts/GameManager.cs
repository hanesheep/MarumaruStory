using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static string gameState; //静的メンバ
    public static int totalScore; //ゲーム全般を通してのスコア
    public static int stageScore; //そのステージに獲得したスコア

    void Awake()
    {
        //ゲームの初期状態をPlayingと設定
        gameState = "playing";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
     // Update is called once per frame
    void Update()
    {
        Debug.Log(gameState);
    }
}
