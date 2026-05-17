//using System.Reflection;
//using BookService.Application.DTOs;
//using BookService.Application.MediaUnits.Handlers; // Direct reference to grab the assembly
//using NetArchTest.Rules;
//using Xunit;

//namespace BookService.Tests;

//public class ArchitectureTests
//{
//    [Fact]
//    public void Handlers_ShouldNotHaveManualMappingDependencies()
//    {
//        // Arrange: Grab the assembly where all your MediatR Handlers live
//        var applicationAssembly = Assembly.GetAssembly(typeof(GetAllMediaUnitsHandler));

//        // Act: Enforce that Handlers should rely on Mapster for translation,
//        // rather than manually building and copying data directly inside logic blocks.
//        var result = Types.InAssembly(applicationAssembly)
//            .That()
//            .HaveNameEndingWith("Handler")
//            .Or()
//            .HaveNameEndingWith("Service")
//            .ShouldNot()
//            .HaveDependencyOn("BookService.Application.DTOs.Manual") // If you put manual mappers in a folder
//            .GetResult();

//        // Assert
//        Assert.True(result.IsSuccessful, "Found structural mapping code violating clean architecture guidelines!");
//    }

//    [Fact]
//    public void Handlers_Should_Reside_In_Handlers_Namespace()
//    {
//        var applicationAssembly = Assembly.GetAssembly(typeof(GetAllMediaUnitsHandler));

//        var result = Types.InAssembly(applicationAssembly)
//            .That()
//            .HaveNameEndingWith("Handler")
//            .Should()
//            .ResideInNamespaceEndingWith("Handlers")
//            .GetResult();

//        Assert.True(result.IsSuccessful, "All MediatR Handlers must reside cleanly inside a '.Handlers' namespace.");
//    }
//}