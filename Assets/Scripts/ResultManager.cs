using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public GameObject scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //リザルト画面のScoreTextオブジェクトがもつTextMeshPro(UGUI)のText欄に
        //GameManagerのstatic変数であるtotalscoreを代入する
        //ただしString型に型変換が必要
        scoreText.GetComponent<TextMeshProUGUI>().text = GameManager.totalScore.ToString();
    }

}
