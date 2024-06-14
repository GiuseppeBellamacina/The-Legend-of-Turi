using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    public GameObject credits;
    public GameObject title;
    public AudioSource audioSource;
    public float speed;
    public float limit;

    void Awake()
    {
        StartCoroutine(EndOfSound());
        Cursor.visible = false;
    }

    IEnumerator EndOfSound()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        LevelManager.Instance.MainMenuScene();
    }

    void FixedUpdate()
    {
        if (Input.anyKeyDown)
            LevelManager.Instance.MainMenuScene();
        
        credits.transform.position += new Vector3(0, speed, 0);
        if (credits.transform.position.y > limit)
            title.SetActive(true);
    }
}