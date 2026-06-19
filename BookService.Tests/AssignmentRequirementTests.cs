using System;
using System.Linq;
using System.Reflection;
using BookService.Application.MediaUnits.Handlers; // Using this to locate your Application assembly
using BookService.Application.DTOs;
using Xunit;

namespace BookService.Tests;

public class AssignmentRequirementTests
{
    [Fact]
    public void HandlersAndServices_ShouldNotContainManualDTOMapping()
    {
        // Arrange: Get the assembly where your MediatR Handlers live
        var applicationAssembly = Assembly.GetAssembly(typeof(GetMediaUnitByIdHandler));

        // Find all types that are Handlers or Services
        // Fix: Use ?. and ?? to safely handle potential nulls
        var targetTypes = applicationAssembly?.GetTypes()
            .Where(t => t.Name.EndsWith("Handler") || t.Name.EndsWith("Service"))
            .ToList() ?? new List<Type>();

        // Act & Assert
        foreach (var type in targetTypes)
        {
            // Get all methods declared directly inside the class
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                .Where(m => m.DeclaringType == type);

            foreach (var method in methods)
            {
                var methodBody = method.GetMethodBody();
                if (methodBody == null) continue;

                // Scan the local variables initialized inside the method
                foreach (var localVariable in methodBody.LocalVariables)
                {
                    var localType = localVariable.LocalType;

                    // REQUIREMENT CHECK: If a handler manually instantiates a Response DTO, 
                    // it violates the "Must use Mapster" rule.
                    if (localType.Name.EndsWith("ResponseDTO"))
                    {
                        Assert.Fail(
                            $"Requirement Violation! Method '{method.Name}' in '{type.Name}' " +
                            $"manually initializes or utilizes a local '{localType.Name}'. " +
                            $"You must use Mapster's '.Adapt<T>()' or '.ProjectToType<T>()' instead."
                        );
                    }
                }
            }
        }
    }
}