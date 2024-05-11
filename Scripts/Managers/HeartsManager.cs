using UnityEngine;
using UnityEngine.UI;

public class HeartsManager : MonoBehaviour
{
    public Image[] hearts;
    public Sprite heart100, heart75, heart50, heart25, heart0;
    public CharacterData player;

    void Start()
    {
        SetHearts();
    }

    void InitHearts()
    {
        for (int i = 0; i < player.maxHealth; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = heart0;
        }
    }

    public void SetHearts(){
        InitHearts();
        for (int i = 0; i < player.maxHealth; i++)
        {
            if (i < player.health)
            {
                if (player.health - i >= 1)
                    hearts[i].sprite = heart100;
                else if (player.health - i >= 0.75)
                    hearts[i].sprite = heart75;
                else if (player.health - i >= 0.5)
                    hearts[i].sprite = heart50;
                else if (player.health - i >= 0.25)
                    hearts[i].sprite = heart25;
                else
                    hearts[i].sprite = heart0;
            }
        }
    }
}
