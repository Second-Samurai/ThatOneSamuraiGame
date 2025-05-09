using System.Collections;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager;
using UnityEngine;

public class CinematicBarsController : PausableMonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private AnimationCurve m_OpeningAnimationCurve;
    [SerializeField] private AnimationCurve m_ClosingAnimationCurve;
    [SerializeField] private float m_Duration;

    private CinematicBarsView m_View;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_View = this.GetComponent<CinematicBarsView>();

        IUIEventCollection _EventCollection = UserInterfaceManager.Instance.UIEventCollection;
        _EventCollection.RegisterEvent(CinematicBarsUIEvents.ShowCinematicBars, this.ShowCinematicBars);
        _EventCollection.RegisterEvent(
            CinematicBarsUIEvents.HideCinematicBars, 
            () =>
            {
                this.StopAllCoroutines();
                this.StartCoroutine(this.HideCinematicBars());
            });
    }

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    private void ShowCinematicBars()
    {
        this.m_View.ShowGUI();
        this.StopAllCoroutines();
        this.StartCoroutine(this.AnimateBarHeight(this.m_OpeningAnimationCurve));
    }

    private IEnumerator HideCinematicBars()
    {
        yield return this.AnimateBarHeight(this.m_ClosingAnimationCurve);
        this.m_View.HideGUI();
    }

    private IEnumerator AnimateBarHeight(AnimationCurve selectedCurve)
    {
        float _ElapsedTime = 0f;
        while (_ElapsedTime < this.m_Duration)
        {
            _ElapsedTime += Time.deltaTime;
            float _T = _ElapsedTime / this.m_Duration;
            
            // Note: Curves should always be between 0 and 1
            float _CurveValue = selectedCurve.Evaluate(_T);
            
            this.m_View.UpdateBarHeight(_CurveValue);
            yield return null;
        }
    }
    
    #endregion Methods

}
