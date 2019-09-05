using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadPanel : MonoBehaviour
{
    GameObject m_PausePanel;

    [SerializeField] InputField m_Input;
    [SerializeField] GameObject m_SavePanel;
    [SerializeField] GameObject m_ContentParent;
    [SerializeField] GameObject m_SaveContentPrefab;

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

    private void ListSaveFiles()
    {
        string[] files = Directory.GetFiles(SaveDataManager.GetSavePath());

        foreach (var file in files)
        {
            GameObject saveContent = Instantiate(m_SaveContentPrefab);
            saveContent.transform.SetParent(m_ContentParent.transform);
            saveContent.GetComponent<LoadButton>().SetData(Path.GetFileName(file));
        }
    }

    public void SetData(GameObject pausePanel)
    {
        gameObject.SetActive(true);
        m_PausePanel = pausePanel;
    }


    public void OnClieckBackButton()
    {
        m_PausePanel.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OnclickCancelButton()
    {
        m_SavePanel.gameObject.SetActive(false);
    }


    public void SavePanelBringIn()
    {
        m_SavePanel.gameObject.SetActive(true);
    }

    public void OnClickSaveButton()
    {
        if (!string.IsNullOrEmpty(m_Input.text))
        {
            Save(m_Input.text);
            m_SavePanel.gameObject.SetActive(false);
        }
    }

    

    void Save(string saveName)
    {

        GameObject go = GameObject.FindWithTag("Player");
        if (go == null)
            Debug.LogError("NULL");

        float[] position = new float[3];
        position[0] = go.transform.position.x;
        position[1] = go.transform.position.y;
        position[2] = go.transform.position.z;


        float[] rotation = new float[3];
        rotation[0] = go.transform.eulerAngles.x;
        rotation[1] = go.transform.eulerAngles.y;
        rotation[2] = go.transform.eulerAngles.z;

        MetaData metaData = new MetaData(go.GetComponent<CompleteProject.PlayerHealth>().currentHealth, position, rotation);
        SaveDataManager.SaveData(metaData,saveName);

        foreach (Transform child in m_ContentParent.transform)
        {
            Destroy(child.gameObject);
        }

        ListSaveFiles();
    }

    public void LoadData(string saveName)
    {
        MetaData data = SaveDataManager.LoadData(saveName);
        if (data != null)
        {
            GameObject go = GameObject.FindWithTag("Player");

            Vector3 pos = new Vector3(data.PlayerPosition[0], data.PlayerPosition[1], data.PlayerPosition[2]);
            Vector3 rot = new Vector3(data.PlayerRotation[0], data.PlayerRotation[1], data.PlayerRotation[2]);

            go.transform.position = pos;
            go.GetComponent<CompleteProject.PlayerHealth>().currentHealth = data.PlayerHelth;
            go.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }
    }
}
