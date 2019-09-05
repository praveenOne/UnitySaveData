using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
    [SerializeField] Text m_BtnText;

    public void SetData(string name)
    {
        m_BtnText.text = name;
    }

    public void OnClickLoad()
    {
        SaveLoadPanel.Instance.LoadData(m_BtnText.text);
    }
}
