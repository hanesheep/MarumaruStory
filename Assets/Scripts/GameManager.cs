using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static string gameState; //�ÓI�����o
    public static int totalScore; //�Q�[���S�ʂ�ʂ��ẴX�R�A
    public static int stageScore; //���̃X�e�[�W�Ɋl�������X�R�A

    void Awake()
    {
        //�Q�[���̏�����Ԃ�Playing�Ɛݒ�
        gameState = "playing";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
     // Update is called once per frame
    void Update()
    {
        Debug.Log(gameState);
    }
}
