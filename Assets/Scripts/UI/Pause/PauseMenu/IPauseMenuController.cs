namespace ThatOneSamuraiGame.Scripts.UI.Pause.PauseMenu
{

    public interface IPauseMenuController
    {
        // Methods below are only based on existing Pause Menu buttons
        // Ticket: #46 - Redesign Pause menu UI to undertake an MVC design.

        #region - - - - - - Methods - - - - - -

        void DisplayPauseScreen();

        void ExitButton();

        void ResumeButton();

        void OptionsMenu();

        void DisableOptionsMenu();

        void HidePauseScreen();

        #endregion Methods

    }

}