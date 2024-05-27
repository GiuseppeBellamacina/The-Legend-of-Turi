using System.Collections;
using UnityEngine;

public class ArrowIntro : MonoBehaviour
{
    public GameObject arrow;
    public GameObject[] arrows;
    public Signals signals;
    public int numArrows;
    public float speed;
    public float timer;

    void Start()
    {
        float offset = 0.5f;
        arrows = new GameObject[numArrows];
        for (int i = 0; i < numArrows; i++)
        {
            Vector3 position = transform.position + new Vector3(offset * i, -offset * i, 0);
            arrows[i] = Instantiate(arrow, position, Quaternion.identity);
        }
        signals.Raise();
        StartCoroutine(Timer());
    }

    void FixedUpdate()
    {
        for (int i = 0; i < numArrows; i++)
        {
            arrows[i].transform.Rotate(1, 0, 0);
            arrows[i].transform.position += new Vector3(speed * Time.deltaTime, Mathf.Sin(Time.time), 0) * 0.01f;
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(timer);
        foreach (GameObject a in arrows)
            Destroy(a);
        Destroy(gameObject);
    }

}