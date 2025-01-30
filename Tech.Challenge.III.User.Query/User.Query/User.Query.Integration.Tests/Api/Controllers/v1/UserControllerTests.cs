using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using User.Query.Communication.Response;
using User.Query.Integration.Tests.Fakes.Request;

namespace User.Query.Integration.Tests.Api.Controllers.v1;
public class UserControllerTests() : BaseTestClient("/api/v1/user")
{
    [Fact]
    public async Task ThereIsUser_ReturnsOk_WhenExistsEmail()
    {
        // Arrange
        var user = Factory.RecoverUser();

        var uri = $"{ControllerUri}/there-is-user/{user.Email}";

        // Act
        var response = await Client.GetAsync(uri);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var loggedUserJson = await DeserializeResponse<Response.Result<ResponseExistsUserJson>>(response);

        loggedUserJson.Data.ThereIsUser.Should().BeTrue();
        loggedUserJson.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ThereIsUser_ReturnsOk_WhenThereIsNoEmail()
    {
        // Arrange
        var email = "new-email@email.com";

        var uri = $"{ControllerUri}/there-is-user/{email}";

        // Act
        var response = await Client.GetAsync(uri);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var loggedUserJson = await DeserializeResponse<Response.Result<ResponseExistsUserJson>>(response);

        loggedUserJson.Data.ThereIsUser.Should().BeFalse();
        loggedUserJson.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task RecoverUser_ReturnsOk_WhenExistsEmail()
    {
        // Arrange
        var user = Factory.RecoverUser();

        var uri = $"{ControllerUri}/{user.Email}";

        // Act
        var response = await Client.GetAsync(uri);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var loggedUserJson = await DeserializeResponse<Response.Result<ResponseUserJson>>(response);

        loggedUserJson.Data.Name.Should().NotBeNullOrWhiteSpace();
        loggedUserJson.Data.Email.Should().NotBeNullOrWhiteSpace();
        loggedUserJson.Error.Should().BeNullOrWhiteSpace();
        loggedUserJson.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task RecoverUser_ReturnsOk_WhenThereIsNoEmail()
    {
        // Arrange
        var email = "non-existent-email@email.com";

        var uri = $"{ControllerUri}/{email}";

        // Act
        var response = await Client.GetAsync(uri);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var loggedUserJson = await DeserializeResponse<Response.Result<ResponseUserJson>>(response);

        loggedUserJson.Data.Should().BeNull();
        loggedUserJson.IsSuccess.Should().BeFalse();
        loggedUserJson.Error.Equals($"No user found with email: {email}");
    }

    [Fact]
    public async Task RecoverByEmailAndPassword_ReturnsOk_WhenCorrectEmailAndPassword()
    {
        // Arrange
        var user = Factory.RecoverUser();

        var request = new RequestEmailPasswordUserJsonBuilder()
            .WithEmail(user.Email)
            .WithPassword(user.Password)
            .Build();

        var uri = string.Concat(ControllerUri, "/recover-email-password");

        // Act
        var response = await Client.PostAsJsonAsync(uri, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var loggedUserJson = await DeserializeResponse<Response.Result<ResponseUserJson>>(response);

        loggedUserJson.Data.Name.Should().NotBeNullOrWhiteSpace();
        loggedUserJson.Data.Email.Should().NotBeNullOrWhiteSpace();
        loggedUserJson.Error.Should().BeNullOrWhiteSpace();
        loggedUserJson.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task RecoverByEmailAndPassword_ReturnsOk_WhenIncorrectEmailAndPassword()
    {
        // Arrange
        var user = Factory.RecoverUser();

        var request = new RequestEmailPasswordUserJsonBuilder()
            .WithEmail("incorrect-email")
            .WithPassword("incorrect-password")
            .Build();

        var uri = string.Concat(ControllerUri, "/recover-email-password");

        // Act
        var response = await Client.PostAsJsonAsync(uri, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var loggedUserJson = await DeserializeResponse<Response.Result<ResponseUserJson>>(response);

        loggedUserJson.Data.Should().BeNull();
        loggedUserJson.IsSuccess.Should().BeFalse();
        loggedUserJson.Error.Equals("Invalid email or username.");
    }
}
