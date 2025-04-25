using System.Collections;

namespace ThatOneSamuraiGame.Scripts.General.Services
{

    public interface ICommand
    {

        #region - - - - - - Methods - - - - - -

        void Execute();
        
        #endregion Methods

    }

    public interface IEnumeratorCommand
    {

        #region - - - - - - Methods - - - - - -

        IEnumerator Execute();

        #endregion Methods

    }

}