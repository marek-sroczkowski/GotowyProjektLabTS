using AutoMapper;
using LabAuthorizationTS.Mapping.Interfaces;
using System.Reflection;

namespace LabAuthorizationTS.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(x => typeof(IMap).IsAssignableFrom(x) && !x.IsInterface)
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod(nameof(IMap.Mapping));
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}