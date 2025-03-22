using UnityEngine;

public class CinematicBarsView : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [SerializeField, RequiredField] private RectTransform m_TopBar;
    [SerializeField, RequiredField] private RectTransform m_BottomBar;
    [SerializeField] private float m_BarHeight;

    #endregion Fields

    #region - - - - - - MyRegion - - - - - -

    public void UpdateBarHeight(float normalizedFillAmount)
    {
        this.m_TopBar.sizeDelta =  new Vector2(m_TopBar.sizeDelta.x, normalizedFillAmount * m_BarHeight);
        this.m_BottomBar.sizeDelta = new Vector2(this.m_BottomBar.sizeDelta.x, normalizedFillAmount * this.m_BarHeight);
    }

    #endregion MyRegion

}
