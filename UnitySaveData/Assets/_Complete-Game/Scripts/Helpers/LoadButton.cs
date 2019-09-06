using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
    [SerializeField] Text m_BtnText;

    /// <summary>
    /// Initialize Button
    /// </summary>
    /// <param name="name"></param>
    public void SetData(string name)
    {
        m_BtnText.text = name;
    }

    /// <summary>
    /// Button On Click
    /// </summary>
    public void OnClickLoad()
    {
        SaveLoadPanel.Instance.LoadData(m_BtnText.text);
    }
}
