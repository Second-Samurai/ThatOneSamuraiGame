namespace ThatOneSamuraiGame.Scripts.UI.Pause.PauseMenu
{

    public interface IPauseMenuController
    {
        // Methods below are only based on existing Pause Menu buttons
        // Ticket: #<ticket number> - Redesign Pause menu UI to undertake an MVC design.

        #region - - - - - - Methods - - - - - -

        void DisplayPauseScreen();

        void ExitButton();

        void ResumeButton();

        void OptionsMenu();

        void DisableOptionsMenu();

        #endregion Methods

    }

}