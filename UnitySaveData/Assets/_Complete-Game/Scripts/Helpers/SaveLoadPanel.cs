using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadPanel : MonoBehaviour
{
    GameObject m_PausePanel;

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

    public void OnClickSaveButton()
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
        SaveDataManager.SaveData(metaData);
    }

    void LoadData()
    {
        MetaData data = SaveDataManager.LoadData();
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
