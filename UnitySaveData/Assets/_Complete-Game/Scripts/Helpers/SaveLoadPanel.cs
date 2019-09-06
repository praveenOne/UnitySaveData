using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadPanel : MonoBehaviour
{
    #region member fields
    GameObject m_PausePanel;
    #endregion

    #region serialized fields
    [SerializeField] InputField m_Input;
    [SerializeField] GameObject m_SavePanel;
    [SerializeField] GameObject m_ContentParent;
    [SerializeField] GameObject m_SaveContentPrefab;
    [SerializeField] PauseManager m_PauseManager;
    [SerializeField] Canvas m_MenuCanvas;
    [SerializeField] Transform[] EnemyParent;
    [SerializeField] GameObject[] Enemies;
    #endregion

    #region singleton stuff
    private static SaveLoadPanel m_Instance;

    public static SaveLoadPanel Instance
    {
        get { return m_Instance; }
    }
    #endregion

    private void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        ListSaveFiles();
    }

    /// <summary>
    /// Creates Save file list
    /// </summary>
    private void ListSaveFiles()
    {
        string[] files = Directory.GetFiles(SaveDataManager.GetSavePath(), "*"+SaveDataManager.GetExtension());

        foreach (var file in files)
        {
            GameObject saveContent = Instantiate(m_SaveContentPrefab);
            saveContent.transform.SetParent(m_ContentParent.transform);
            saveContent.GetComponent<LoadButton>().SetData(Path.GetFileName(file));
        }
    }

    #region Panel Specific Methods
    /// <summary>
    /// Initialized Save Load Panel
    /// </summary>
    /// <param name="pausePanel"></param>
    public void SetData(GameObject pausePanel)
    {
        gameObject.SetActive(true);
        m_PausePanel = pausePanel;
    }

    /// <summary>
    /// On Back Button Clieck
    /// </summary>
    public void OnClieckBackButton()
    {
        m_PausePanel.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// On Cancel Button Click
    /// </summary>
    public void OnclickCancelButton()
    {
        m_SavePanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Bring In Save Panel
    /// </summary>
    public void SavePanelBringIn()
    {
        m_SavePanel.gameObject.SetActive(true);
    }


    /// <summary>
    /// On Click Save Button on Save Panel
    /// </summary>
    public void OnClickSaveButton()
    {
        if (!string.IsNullOrEmpty(m_Input.text))
        {
            Save(m_Input.text);
            m_SavePanel.gameObject.SetActive(false);
        }
    }
    #endregion


    #region Save Data specific
    /// <summary>
    /// Save Data
    /// </summary>
    /// <param name="saveName"></param>
    void Save(string saveName)
    {
        MetaData metaData = new MetaData(GetCurrentPlayerData(), GetCurrentEnemyData(0), GetCurrentEnemyData(1), GetCurrentEnemyData(2));
        SaveDataManager.SaveData(metaData,saveName,(bool obj) =>
        { if (obj)
            {
                CleanData(m_ContentParent.transform);
                ListSaveFiles();

            }
            else
            {
                Debug.LogError("Unable to save the data");
            }
        });

        
    }

    /// <summary>
    /// Collects Player data to save
    /// </summary>
    /// <returns></returns>
    MyCharacter GetCurrentPlayerData()
    {
        GameObject player = GameObject.FindWithTag("Player");

        MyTransform playerPos = new MyTransform(player.transform.position.x,
                    player.transform.position.y,
                    player.transform.position.z);

        MyTransform playerRotation = new MyTransform(player.transform.eulerAngles.x,
                    player.transform.eulerAngles.y,
                    player.transform.eulerAngles.z);

        return new MyCharacter(player.GetComponent<CompleteProject.PlayerHealth>().currentHealth,
                                playerPos, playerRotation);
    }

    /// <summary>
    /// Collect Enemy Data to save
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    MyCharacter[] GetCurrentEnemyData(int index)
    {
        List<MyCharacter> enemyList = new List<MyCharacter>();
        Transform parent = EnemyParent[index];

        foreach (Transform enemy in parent)
        {
            //position
            MyTransform enemyPos        = new MyTransform(enemy.position.x, enemy.position.y, enemy.position.z);

            //rotation
            MyTransform enemyRotation   = new MyTransform(enemy.eulerAngles.x, enemy.eulerAngles.y, enemy.eulerAngles.z);

            MyCharacter enemyCharacter  = new MyCharacter(enemy.gameObject.GetComponent<CompleteProject.EnemyHealth>().currentHealth,
                                         enemyPos, enemyRotation);

            enemyList.Add(enemyCharacter);

        }

        return enemyList.ToArray();
    }
    #endregion


    #region Load Data specific
    /// <summary>
    /// Get Saved data by save name
    /// </summary>
    /// <param name="saveName"></param>
    public void LoadData(string saveName)
    {
        MetaData data = SaveDataManager.LoadData(saveName);
        if (data != null)
        {
            LoadPlayerData(data.Player);

            LoadEnemyData(data.ZomBunny, 0);
            LoadEnemyData(data.ZomBear, 1);
            LoadEnemyData(data.Helliphant, 2);

            m_PauseManager.Pause();
            OnClieckBackButton();
            m_MenuCanvas.enabled = false;
        }
    }

    /// <summary>
    /// Load Player Data
    /// </summary>
    /// <param name="player"></param>
    void LoadPlayerData(MyCharacter player)
    {
        GameObject thisPlayer = GameObject.FindWithTag("Player");

        Vector3 pos = new Vector3(player.Position.X, player.Position.Y, player.Position.Z);

        thisPlayer.transform.position = pos;
        thisPlayer.GetComponent<CompleteProject.PlayerHealth>().currentHealth = player.Helth;
        thisPlayer.transform.rotation = Quaternion.Euler(player.Rotation.X, player.Rotation.Y, player.Rotation.Z);
    }

    /// <summary>
    /// Load Enemy Data
    /// </summary>
    /// <param name="characterData"></param>
    /// <param name="enemyIndex"></param>
    void LoadEnemyData(MyCharacter[] characterData, int enemyIndex)
    {
        Transform parent = EnemyParent[enemyIndex];

        CleanData(parent);

        foreach (var enemy in characterData)
        {
            GameObject thisEnemy = Instantiate(Enemies[enemyIndex]);
            thisEnemy.transform.SetParent(parent);

            Vector3 pos = new Vector3(enemy.Position.X, enemy.Position.Y, enemy.Position.Z);
            thisEnemy.transform.position = pos;
            thisEnemy.transform.rotation = Quaternion.Euler(enemy.Rotation.X, enemy.Rotation.Y, enemy.Rotation.Z);
            thisEnemy.GetComponent<CompleteProject.EnemyHealth>().currentHealth = enemy.Helth;

        }
    }

    #endregion


    /// <summary>
    /// Remove all children of specific parent
    /// </summary>
    /// <param name="parent"></param>
    void CleanData(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
