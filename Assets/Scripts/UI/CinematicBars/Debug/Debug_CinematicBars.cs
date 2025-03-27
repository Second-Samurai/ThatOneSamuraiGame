using ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager;

public class Debug_CinematicBars : IDebugCommandRegistrater
{

    #region - - - - - - Methods - - - - - -

    public void RegisterCommand(IDebugCommandSystem debugCommandSystem)
    {
        DebugCommand _RevealCinematicBars = new DebugCommand(
            "ui_showbars",
            "Show Cinematic Bars",
            "ui_showbars",
            this.RevealBars);
        DebugCommand _HideCinematicBars = new DebugCommand(
            "ui_hidebars",
            "Hide Cinematic Bars",
            "ui_hidebars",
            this.HideBars);
        
        debugCommandSystem.RegisterCommand(_RevealCinematicBars);
        debugCommandSystem.RegisterCommand(_HideCinematicBars);
    }

    private void RevealBars()
    {
        IUIEventMediator _EventMediator = UserInterfaceManager.Instance.UIEventMediator;
        _EventMediator.Dispatch(CinematicBarsUIEvents.ShowCinematicBars);
    }

    private void HideBars()
    {
        IUIEventMediator _EventMediator = UserInterfaceManager.Instance.UIEventMediator;
        _EventMediator.Dispatch(CinematicBarsUIEvents.HideCinematicBars);
    }
    
    #endregion Methods
  
}
