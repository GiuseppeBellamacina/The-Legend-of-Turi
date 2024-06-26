using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public GameObject[] obj;
    public AudioSource audioSource;
    int index = 0;

    void Awake()
    {
        foreach (GameObject o in obj)
            o.SetActive(false);
        ActivateObj();
        StartCoroutine(EndOfSound());
        Cursor.visible = false;
    }

    public void ActivateObj()
    {
        obj[index++].SetActive(true);
    }

    IEnumerator EndOfSound()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        LevelManager.Instance.StartGame();
    }

    void Update()
    {
        if (Input.anyKeyDown)
            LevelManager.Instance.StartGame();
    }
}