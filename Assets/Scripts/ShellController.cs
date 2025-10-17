using UnityEngine;

public class ShellController : MonoBehaviour
{
    [Header("¶‘¶ŠÔ")]
    public float deleteTime = 3.0f; //íœ‚·‚éŠÔ‚ğw’è

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject,deleteTime);@//íœİ’è
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);@//‰½‚©‚ÉÚG‚µ‚½‚çíœ
    }
    
}
