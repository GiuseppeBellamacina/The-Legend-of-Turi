using UnityEngine;
using UnityEngine.TextCore.Text;
using System.IO;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    private static bool _isInitialized;
    private static int _index = 0;
    public BoolValue[] bools;
    public IntValue[] ints;
    public FloatValue[] floats;
    public VectorValue[] vectors;
    public CharacterData[] characters;
    public Dialog[] dialogs;
    public Status[] statuses;
    public Item[] items;
    public Inventory inventory;
    public GameStatus gameStatus;

    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DataManager>();
                if (_instance == null)
                    Debug.LogError("No DataManager found in the scene.");
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
        
        if (!_isInitialized)
        {
            _isInitialized = true;
            Reset();
        }
    }

    public void ResetIndexes()
    {
        _index = 0;

        foreach (BoolValue b in bools)
            b.dataIndex = _index;
        foreach (IntValue i in ints)
            i.dataIndex = _index;
        foreach (FloatValue f in floats)
            f.dataIndex = _index;
        foreach (VectorValue v in vectors)
            v.dataIndex = _index;
        foreach (CharacterData c in characters)
            c.dataIndex = _index;
        foreach (Dialog d in dialogs)
            d.dataIndex = _index;
        foreach (Status s in statuses)
            s.dataIndex = _index;
        foreach (Item item in items)
            item.dataIndex = _index;
        inventory.dataIndex = _index;
        gameStatus.dataIndex = _index;
    }

    public void InitializeIndexes()
    {
        if (bools[0].dataIndex != 0)
            return;

        _index = 1;

        foreach (BoolValue b in bools)
            b.dataIndex = _index++;
        foreach (IntValue i in ints)
            i.dataIndex = _index++;
        foreach (FloatValue f in floats)
            f.dataIndex = _index++;
        foreach (VectorValue v in vectors)
            v.dataIndex = _index++;
        foreach (CharacterData c in characters)
            c.dataIndex = _index++;
        foreach (Dialog d in dialogs)
            d.dataIndex = _index++;
        foreach (Status s in statuses)
            s.dataIndex = _index++;
        foreach (Item item in items)
            item.dataIndex = _index++;
        inventory.dataIndex = _index++;
        gameStatus.dataIndex = _index;
    }

    public void Reset()
    {
        foreach (BoolValue b in bools)
            b.Reset();
        foreach (IntValue i in ints)
            i.Reset();
        foreach (FloatValue f in floats)
            f.Reset();
        foreach (VectorValue v in vectors)
            v.Reset();
        foreach (CharacterData c in characters)
            c.Reset();
        foreach (Dialog d in dialogs)
            d.Reset();
        foreach (Status s in statuses)
            s.Reset();
        foreach (Item item in items)
            item.Reset();
        inventory.Reset();
        gameStatus.Reset();
    }

    public void SaveData()
    {
        foreach (BoolValue b in bools)
            b.Save();
        foreach (IntValue i in ints)
            i.Save();
        foreach (FloatValue f in floats)
            f.Save();
        foreach (VectorValue v in vectors)
            v.Save();
        foreach (CharacterData c in characters)
            c.Save();
        foreach (Dialog d in dialogs)
            d.Save();
        foreach (Status s in statuses)
            s.Save();
        foreach (Item item in items)
            item.Save();
        inventory.Save();
        gameStatus.Save();
    }

    public bool LoadData()
    {
        if (!IsDataSaved())
            return false;
        
        foreach (BoolValue b in bools)
            b.Load();
        foreach (IntValue i in ints)
            i.Load();
        foreach (FloatValue f in floats)
            f.Load();
        foreach (VectorValue v in vectors)
            v.Load();
        foreach (CharacterData c in characters)
            c.Load();
        foreach (Dialog d in dialogs)
            d.Load();
        foreach (Status s in statuses)
            s.Load();
        foreach (Item item in items)
            item.Load();
        inventory.Load();
        gameStatus.Load();

        return true;
    }

    public void DeleteData()
    {
        string path = SaveSystem.path;
        if (Directory.Exists(path))
            Directory.Delete(path, true);
        
        inventory.currentItem = null;
        inventory.items.Clear();
    }

    public bool IsDataSaved()
    {
        string path = SaveSystem.path;
        return Directory.Exists(path);
    }
}