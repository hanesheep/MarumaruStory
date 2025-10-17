using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    //扱う曲たち
    public AudioClip bgm_Title;
    public AudioClip bgm_Stage;
    public AudioClip bgm_Result;
    public AudioClip bgm_GameClear;
    public AudioClip bgm_GameOver;

    AudioSource audio; //キャンバスについているAudioSourceの情報

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audio = GetComponent<AudioSource>();　//AudioSourceの情報取得

        //現在シーン情報の取得
        string currentSceneName = SceneManager.GetActiveScene().name;

        if(currentSceneName == "Title")
        {
            //タイトルの曲を鳴らす処理
            PlayBGM(bgm_Title);
        }
        else if(currentSceneName == "Result")
        {
            //リザルトの曲を鳴らす処理
            PlayBGM(bgm_Result);
        }
        else
        {
            //ステージの曲を鳴らす処理
            PlayBGM(bgm_Stage);
        }
    }

    // Update is called once per frame
    void PlayBGM(AudioClip clip)
    {
        audio.clip = clip;
        audio.loop = true;　//BGMはループする
        audio.Play();　　　//再生する
    }
}
