using System.Collections;
using TMPro;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    private static RespawnManager _instance;
    public GameObject deathEffectPrefab;

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
    }

    public void Respawn()
    {
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
        yield return new WaitForSeconds(0.1f);
        GameObject effect = Instantiate(deathEffectPrefab, Vector3.zero, Quaternion.identity);
        effect.GetComponentInChildren<TMP_Text>().text = GetRandomDeathMessage();
        yield return new WaitForSeconds(3f);
        // Distruggo gli oggetti che voglio ricreare
        Destroy(CanvasSingleton.Instance.gameObject);
        Destroy(PlayerController.Instance.gameObject);
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
        yield return new WaitForSeconds(0.1f);
        GameObject effect = Instantiate(deathEffectPrefab, Vector3.zero, Quaternion.identity);
        effect.GetComponentInChildren<TMP_Text>().text = GetRandomDeathMessage();
        yield return new WaitForSeconds(3f);
        // Distruggo gli oggetti che voglio ricreare
        Destroy(CanvasSingleton.Instance.gameObject);
        Destroy(PlayerController.Instance.gameObject);
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