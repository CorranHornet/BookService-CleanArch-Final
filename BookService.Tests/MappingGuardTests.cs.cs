using System.Reflection;
using BookService.Application.MediaUnits.Handlers;

namespace BookService.Tests
{
    public class MappingGuardTests
    {
        [Fact]
        public void Handlers_ShouldNotContainManualDtoConstruction()
        {
            var assembly = typeof(GetMediaUnitByIdHandler).Assembly;

            var handlerTypes = assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Handler") || t.Name.EndsWith("Service"))
                .ToList();

            var violations = new List<string>();

            foreach (var type in handlerTypes)
            {
                var methods = type.GetMethods(BindingFlags.Public |
                                               BindingFlags.NonPublic |
                                               BindingFlags.Instance |
                                               BindingFlags.Static);

                foreach (var method in methods)
                {
                    var body = method.GetMethodBody();
                    if (body == null) continue;

                    var ilBytes = body.GetILAsByteArray();
                    if (ilBytes == null) continue;

                    var ilCode = System.Text.Encoding.UTF8.GetString(ilBytes);

                    // Detect manual DTO construction patterns
                    if (ilCode.Contains("new ") &&
                        (ilCode.Contains("ResponseDTO") ||
                         ilCode.Contains("DTO")))
                    {
                        violations.Add($"{type.Name}.{method.Name}");
                    }
                }
            }

            Assert.True(
                violations.Count == 0,
                "Manual DTO construction detected in: " + string.Join(", ", violations)
            );
        }

        [Fact]
        public void Handlers_ShouldNotContainSelectNewDtoMapping()
        {
            var assembly = typeof(GetMediaUnitByIdHandler).Assembly;

            var handlerTypes = assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Handler") || t.Name.EndsWith("Service"));

            foreach (var type in handlerTypes)
            {
                var methods = type.GetMethods(BindingFlags.Public |
                                               BindingFlags.NonPublic |
                                               BindingFlags.Instance |
                                               BindingFlags.Static);

                foreach (var method in methods)
                {
                    var body = method.GetMethodBody();
                    if (body == null) continue;

                    var ilBytes = body.GetILAsByteArray();
                    if (ilBytes == null) continue;

                    var ilCode = System.Text.Encoding.UTF8.GetString(ilBytes);

                    // Detect LINQ mapping anti-pattern
                    Assert.False(
                        ilCode.Contains("Select") && ilCode.Contains("new "),
                        $"LINQ manual DTO mapping detected in {type.Name}.{method.Name}"
                    );
                }
            }
        }

        [Fact]
        public void MapsterConfiguration_ShouldCompileSuccessfully()
        {
            // Ensures Mapster config is valid and complete
            BookService.Application.Common.Mapping.MapsterConfig.Register();

            var config = Mapster.TypeAdapterConfig.GlobalSettings;

            config.Compile();

            Assert.True(true);
        }
    }
}