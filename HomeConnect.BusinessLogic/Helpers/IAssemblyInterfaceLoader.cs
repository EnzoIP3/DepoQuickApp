namespace BusinessLogic.Helpers;

public interface IAssemblyInterfaceLoader<TInterface>
    where TInterface : class
{
    List<string> GetImplementationsList(string path);
    TInterface GetImplementation(string implementationName, string path, params object[] args);
    Guid? GetImplementationIdByName(string implementationName, string path);
}
