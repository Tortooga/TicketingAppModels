using System;
using Xunit;
using Models.Projects;
public class ProjectTests
{
    private static ProjectType InternalType => new ProjectType("Internal", true);
    private static ProjectType ExternalType => new ProjectType("External", false);

    [Fact]
    public static void Record_ShouldReturnTrue_WhenProjectIsSavedSuccessfully()
    {
        // Arrange
        var project = new Project(
            Name: "TestProject1",
            Type: InternalType,
            Title: "Test Title",
            Description: "Description of test project",
            Status: ProjectStatus.Active,
            StartDate: DateTime.Now
        );

        // Act
        bool result = project.Record();

        // Assert
        Assert.True(result, "Record() should return true when successfully saved.");
    }

    [Fact]
    public static void GetAll_ShouldReturnAtLeastOneProject_AfterRecording()
    {
        // Arrange
        var project = new Project(
            Name: "TestProject2",
            Type: ExternalType,
            Title: "Title 2",
            Description: "Another project",
            Status: ProjectStatus.NotStarted,
            StartDate: DateTime.Now
        );
        project.Record();

        // Act
        var projects = Project.getAll(Project.table);

        // Assert
        Assert.NotNull(projects);
        Assert.NotEmpty(projects);
    }

    [Fact]
    public static void Record_ShouldHandleNullEndDate_WithoutErrors()
    {
        // Arrange
        var project = new Project(
            Name: "NoEndDate",
            Type: ExternalType,
            Title: "Open Project",
            Description: "Project without end date",
            Status: ProjectStatus.Active,
            StartDate: DateTime.Now,
            EndDate: null
        );

        // Act
        bool result = project.Record();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public static void GetAll_ShouldReturnConsistentDataAcrossCalls()
    {
        // Arrange
        var firstCall = Project.getAll(Project.table);
        var secondCall = Project.getAll(Project.table);

        // Assert
        Assert.Equal(firstCall.Count, secondCall.Count);
    }
}
