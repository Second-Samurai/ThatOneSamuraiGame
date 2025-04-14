public interface IInitialize<TData>
{

    #region - - - - - - Methods - - - - - -

    void Initialize(TData initializationData);

    #endregion Methods

}

public interface IInitialize
{

    #region - - - - - - Methods - - - - - -

    void Initialize();

    #endregion Methods

}
