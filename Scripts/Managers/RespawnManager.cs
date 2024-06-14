using System.Collections;
using TMPro;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    private static RespawnManager _instance;
    public GameObject deathEffectPrefab;
    public GameObject soundtrack;
    public AudioClip deathSound;

    public static RespawnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RespawnManager>();
                if (_instance == null)
                    Debug.LogError("No RespawnManager found in the scene.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        
        if (soundtrack == null)
            soundtrack = GameObject.Find("SoundTrack");
    }

    public void Respawn()
    {
        if (soundtrack == null)
            soundtrack = GameObject.Find("SoundTrack");
            
        AudioManager.Instance.musicSource.Stop();
        soundtrack.GetComponent<AudioSource>().Stop();
        AudioManager.Instance.PlaySFX(deathSound);
        if (DataManager.Instance.LoadData())
            StartCoroutine(RespawnFromDataCo());
        else
            StartCoroutine(RespawnFromBeginningCo());
    }

    IEnumerator RespawnFromDataCo()
    {
        // Tolgo le azioni al menu e al giocatore
        CanvasSingleton.Instance.transform.Find("Menu").GetComponent<MenuController>().RemoveActions();
        PlayerController.Instance.RemoveActions();
        // Death effect
        yield return null;
        Destroy(PlayerController.Instance.gameObject);
        GameObject effect = Instantiate(deathEffectPrefab, Vector3.zero, Quaternion.identity);
        effect.GetComponentInChildren<TMP_Text>().text = GetRandomDeathMessage();
        yield return new WaitForSeconds(6f);
        // Distruggo gli oggetti che voglio ricreare
        Destroy(CanvasSingleton.Instance.gameObject);
        Destroy(CameraMovement.Instance.gameObject);
        Destroy(RoomLocator.Instance.gameObject);
        // Ricarico i dati
        DataManager.Instance.Reset();
        DataManager.Instance.LoadData();
        // Ricarico la scena
        LevelManager.Instance.RespawnPlayer(true);
    }

    IEnumerator RespawnFromBeginningCo()
    {
        // Tolgo le azioni al menu e al giocatore
        CanvasSingleton.Instance.transform.Find("Menu").GetComponent<MenuController>().RemoveActions();
        PlayerController.Instance.RemoveActions();
        // Death effect
        yield return null;
        Destroy(PlayerController.Instance.gameObject);
        GameObject effect = Instantiate(deathEffectPrefab, Vector3.zero, Quaternion.identity);
        effect.GetComponentInChildren<TMP_Text>().text = GetRandomDeathMessage();
        yield return new WaitForSeconds(6f);
        // Distruggo gli oggetti che voglio ricreare
        Destroy(CanvasSingleton.Instance.gameObject);
        Destroy(CameraMovement.Instance.gameObject);
        Destroy(RoomLocator.Instance.gameObject);
        // Cancello i dati
        DataManager.Instance.DeleteData();
        DataManager.Instance.Reset();
        DataManager.Instance.ResetIndexes();
        DataManager.Instance.InitializeIndexes();
        // Ricarico la scena
        LevelManager.Instance.RespawnPlayer(false);
    }

    string GetRandomDeathMessage()
    {
        string[] messages = new string[]
        {
            "Sei Morto",
            "Ta Quagghiasti",
            "RIP",
            "Sei Scarso",
            "F",
            "Catania ha avuto la meglio",
            "Avaia",
            "Mbare..."
        };
        
        return messages[Random.Range(0, messages.Length)];
    }
}