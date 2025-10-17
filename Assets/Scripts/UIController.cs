using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject mainImage;   //アナウンスをする画像
    public GameObject buttonPanel; //ボタンをグループ化しているパネル

    public GameObject retryButton; //リトライボタン
    public GameObject nextButton;  //ネクストボタン

    public Sprite gameClearSprite; //ゲームクリアの絵
    public Sprite gameOverSprite;  //ゲームオーバーの絵

    TimeController timeCnt;
    public GameObject timeText;　　//タイマー

    public GameObject scoreText;　//スコアテキスト

    AudioSource audio;
    SoundController soundController;  //自作したスクリプト


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeCnt = GetComponent<TimeController>();

        buttonPanel.SetActive(false); //存在を非表示

        //時間差でメソッドを発動
        Invoke("InactiveImage", 1.0f);

        UpdateScore(); //トータルスコアが出るように更新

        //AudioSourceとSoundControllerの取得
        audio = GetComponent<AudioSource>();
        soundController = GetComponent<SoundController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState == "gameclear") 
        {
            buttonPanel.SetActive(true); //ボタンパネルの復活
            mainImage.SetActive(true);　 //メイン画像の復活
            //メイン画像オブジェクトのImageコンポーネントが所持している変数spriteにステージクリアの絵を代入
            mainImage.GetComponent<Image>().sprite = gameClearSprite;
            //リトライボタンオブジェクトのButtonコンポーネントが所持している変数interactibleを無効（ボタン機能を無効）
            retryButton.GetComponent<Button>().interactable = false;

            //ステージクリアによってスコアが確定したのでトータルスコアに加算
            GameManager.totalScore += GameManager.stageScore;
            GameManager.stageScore = 0; //ステージスコアはリセット

            timeCnt.isTimeOver = true; //タイムカウントを停止

            //いったんdisplayTimeの数字を変数timesに渡す
            float times = timeCnt.displayTime;

            if (timeCnt.isCountDown) //カウントダウン
            {
                //残時間をタイムボーナスとしてトータルスコアに加算する
                GameManager.totalScore += (int)times * 10;
            }
            else　//カウントアップ 
            {
                float gameTime = timeCnt.gameTime;
                GameManager.totalScore += (int)(gameTime - times) * 10;
            }

            UpdateScore(); //UIに最終的な数字を反映

            //サウンドをストップする
            audio.Stop();
            //SoundControllerの変数に指名したゲームクリアの音を選択して鳴らす
            audio.PlayOneShot(soundController.bgm_GameClear);

            //二重三重にスコアを加算しないようにgameclearのフラグは早々に変化
            GameManager.gameState = "gameend";
        }

        else if (GameManager.gameState == "gameover")
        {
            buttonPanel.SetActive(true); //ボタンパネルの復活
            mainImage.SetActive(true);　 //メイン画像の復活
            //メイン画像オブジェクトのImageコンポーネントが所持している変数spriteにゲームオ－バーの絵を代入
            mainImage.GetComponent<Image>().sprite = gameOverSprite;
            //ネクストボタンオブジェクトのButtonコンポーネントが所持している変数interactibleを無効（ボタン機能を無効）
            nextButton.GetComponent<Button>().interactable = false;

            //カウントを止める
            timeCnt.isTimeOver = true;

            //サウンドをストップする
            audio.Stop();
            //SoundControllerの変数に指名したゲームクリアの音を選択して鳴らす
            audio.PlayOneShot(soundController.bgm_GameClear);


            GameManager.gameState = "gameend";
        }
        else if(GameManager.gameState == "playing")
        {
            //いったんdisplayTimeの数字を変数timesに渡す
            float times = timeCnt.displayTime;
            timeText.GetComponent<TextMeshProUGUI>().text = Mathf.Ceil(times).ToString();

            if (timeCnt.isCountDown)
            {
                if(timeCnt.displayTime <= 0)
                {
                    //プレイヤーを見つけてきてそのPlayerControllerコンポーネントのGameOverメソッドをやらせる
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GameOver();
                    GameManager.gameState = "gameover";
                }
            }
            else
            {
                if(timeCnt.displayTime >= timeCnt.gameTime)
                {
                    //プレイヤーを見つけてきてそのPlayerControllerコンポーネントのGameOverメソッドをやらせる
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GameOver();
                    GameManager.gameState = "gameover";
                }
            }

                //スコアもリアルタイムに更新
                UpdateScore();
        }
    }

    //メイン画像を非表示にするためだけのメソッド
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    //スコアボードを更新
    void UpdateScore()
    {
        int score = GameManager.stageScore + GameManager.totalScore;
        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }
}
