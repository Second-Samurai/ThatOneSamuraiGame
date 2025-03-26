using UnityEngine;

public class CinematicBarsView : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private GameObject m_Content;
    [SerializeField, RequiredField] private RectTransform m_TopBar;
    [SerializeField, RequiredField] private RectTransform m_BottomBar;
    [SerializeField] private float m_BarHeight;

    #endregion Fields

    #region - - - - - - MyRegion - - - - - -

    public void UpdateBarHeight(float normalizedFillAmount)
    {
        this.m_TopBar.sizeDelta =  new Vector2(this.m_TopBar.sizeDelta.x, normalizedFillAmount * this.m_BarHeight);
        this.m_BottomBar.sizeDelta = new Vector2(this.m_BottomBar.sizeDelta.x, normalizedFillAmount * this.m_BarHeight);
    }

    public void ShowGUI()
        => this.m_Content.SetActive(true);

    public void HideGUI()
        => this.m_Content.SetActive(false);

    #endregion MyRegion

}
