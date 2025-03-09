using UnityEngine;
using UnityEngine.UI;

public class EnemyGuardView : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private GameObject m_ContentGroup;
    [SerializeField] private RectTransform m_GuardPanelTransform;
    [SerializeField] private Slider m_GuardSlider;
    // [SerializeField] private Canvas m_FinisherCanvas; // Change so that it's not a canvas within a canvas

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public bool IsVisible
        => this.m_ContentGroup.activeSelf;

    #endregion Properties
  
    #region - - - - - - Methods - - - - - -

    public void UpdateEnemyGuardSlider(float guardPercentage) 
        => this.m_GuardSlider.value = guardPercentage;

    public void UpdateEnemyGuardScreenPosition(Vector2 screenPosition) 
        => this.m_GuardPanelTransform.anchoredPosition = screenPosition;

    public void ShowEnemyGuard()
        => this.m_ContentGroup.SetActive(true);

    public void HideEnemyGuard()
        => this.m_ContentGroup.SetActive(false);

    #endregion Methods

}
