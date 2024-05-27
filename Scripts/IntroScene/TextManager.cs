using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public GameObject[] texts;
    public float timeToWait;
    float timer;
    int index = 0;

    void Awake()
    {
        foreach (GameObject text in texts)
        {
            text.SetActive(false);
        }
    }

    public void ShowText()
    {
        texts[index].SetActive(true);
        StartCoroutine(Wait(timeToWait, index));
    }

    public void HideText(int index)
    {
        texts[index].SetActive(false);
    }

    IEnumerator Wait(float showTime, int index)
    {
        yield return new WaitForSeconds(showTime);
        HideText(index);
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= timeToWait)
        {
            timer = 0;
            ShowText();
            index++;
        }

        if (index == texts.Length)
        {
            Destroy(gameObject);
        }
    }
}