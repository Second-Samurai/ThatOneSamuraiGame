using UnityEngine;
using UnityEngine.UI;

public class EnemyGuardView : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private RectTransform m_CanvasRectTransform;
    
    [Space]
    [SerializeField] private GameObject m_ContentGroup;
    [SerializeField] private RectTransform m_GuardPanelTransform;
    [SerializeField] private Slider m_GuardSlider;
    [SerializeField] private Vector2 m_GuardPositionOffset;

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public bool IsVisible
        => this.m_ContentGroup.activeSelf;

    #endregion Properties
  
    #region - - - - - - Methods - - - - - -

    public void UpdateEnemyGuardSlider(float guardPercentage) 
        => this.m_GuardSlider.value = guardPercentage;

    public void UpdateEnemyGuardScreenPosition(Vector2 screenPosition, float offsetScale)
    {
        screenPosition.x = Screen.width - screenPosition.x;
        screenPosition.y = Screen.height - screenPosition.y;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            this.m_CanvasRectTransform,
            screenPosition,
            null,
            out Vector2 localPoint);

        this.m_GuardPanelTransform.anchoredPosition = localPoint + this.m_GuardPositionOffset * offsetScale;
    }

    public void UpdateEnemyGuardScale(float scale) 
        => this.m_GuardPanelTransform.transform.localScale = new Vector3(scale, scale, scale);

    public void ShowEnemyGuard()
        => this.m_ContentGroup.SetActive(true);

    public void HideEnemyGuard()
        => this.m_ContentGroup.SetActive(false);

    #endregion Methods

}
