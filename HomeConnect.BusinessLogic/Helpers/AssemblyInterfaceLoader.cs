using System.Reflection;

namespace BusinessLogic.Helpers;

public sealed class AssemblyInterfaceLoader<TInterface> : IAssemblyInterfaceLoader<TInterface>
    where TInterface : class
{
    private DirectoryInfo CreateDirectoryInfo(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return new DirectoryInfo(path);
    }

    private List<Type> _implementations = [];

    public List<string> GetImplementationsList(string path)
    {
        var directory = CreateDirectoryInfo(path);
        var files = directory
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

    public TInterface GetImplementation(string implementationName, string path, params object[] args)
    {
        GetImplementationsList(path);
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
