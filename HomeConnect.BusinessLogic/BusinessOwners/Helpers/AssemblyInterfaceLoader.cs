using System.Reflection;

namespace BusinessLogic.BusinessOwners.Helpers;

public sealed class AssemblyInterfaceLoader<TInterface>
    where TInterface : class
{
    public AssemblyInterfaceLoader(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        _directory = new DirectoryInfo(path);
    }

    private readonly DirectoryInfo _directory = null!;
    private List<Type> _implementations = [];

    public List<string> GetImplementationsList()
    {
        var files = _directory
            .GetFiles("*.dll")
            .ToList();

        _implementations = [];
        files.ForEach(file =>
        {
            Assembly assemblyLoaded = Assembly.LoadFile(file.FullName);
            var loadedTypes = assemblyLoaded
                .GetTypes()
                .Where(t => t.IsClass && typeof(TInterface).IsAssignableFrom(t))
                .ToList();

            if (loadedTypes.Count == 0)
            {
                throw new InvalidOperationException(
                    $"No implementation found for interface {typeof(TInterface).Name}");
            }

            _implementations = _implementations
                .Union(loadedTypes)
                .ToList();
        });

        return _implementations.ConvertAll(t => t.Name);
    }

    public TInterface GetImplementation(string implementationName, params object[] args)
    {
        GetImplementationsList();
        var index = _implementations.FindIndex(t => t.Name == implementationName);

        if (index == -1)
        {
            throw new InvalidOperationException(
                $"No implementation found for interface {typeof(TInterface).Name} with name {implementationName}");
        }

        return GetImplementationByIndex(index, args);
    }

    private TInterface GetImplementationByIndex(int index, params object[] args)
    {
        var type = _implementations.ElementAt(index);

        return Activator.CreateInstance(type, args) as TInterface;
    }
}
