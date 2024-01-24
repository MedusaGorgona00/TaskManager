using System.Reflection;

namespace Domain.Common.Mappings
{
    public interface IMappingProfile
    {
        /// <summary>
        /// Apply mappings from assembly
        /// </summary>
        /// <param name="assembly"></param>
        void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var mappedTypes = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType
                    && new[] { typeof(IMapFrom<>), typeof(IMapTo<>) }.Contains(i.GetGenericTypeDefinition()))
                )
                .ToArray();

            foreach (var type in mappedTypes)
            {
                var instance = Activator.CreateInstance(type);
                var methods = type.GetMethods().Where(x => x.Name.Equals("Mapping")).ToList();
                if (!methods.Any())
                {
                    methods = type.GetInterfaces()
                        .Where(x => x.Name == "IMapFrom`1" || x.Name == "IMapTo`1")
                        .Select(x => x.GetMethod("Mapping")!).ToList();
                }

                foreach (var method in methods)
                {
                    method.Invoke(instance, new[] { this });
                }
            }
        }
    }
}
