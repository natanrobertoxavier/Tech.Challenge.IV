﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Region.Query.Communication.Response;
using Region.Query.Domain.Services;
using Region.Query.Exceptions;
using Region.Query.Exceptions.ExceptionBase;
using System.ComponentModel.DataAnnotations;
using TokenService.Manager.Controller;

namespace Region.Query.Api.Filters;

public class AuthenticatedUserAttribute(
    TokenController tokenController,
    IUserQueryServiceApi userQueryServiceApi) : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly TokenController _tokenController = tokenController;
    private readonly IUserQueryServiceApi _userQueryServiceApi = userQueryServiceApi;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenInRequest(context);
            var userEmail = _tokenController.RecoverEmail(token);

            var user = await _userQueryServiceApi.RecoverByEmailAsync(userEmail);

            if (!user.IsSuccess)
            {
                throw new ValidationException($"An error occurred while authenticating the user by token. Error: {user.Error}");
            }
        }
        catch (SecurityTokenExpiredException)
        {
            ExpiredToken(context);
        }
        catch (ValidationException ex)
        {
            ApiCalledError(context, ex.Message);
        }
        catch
        {
            UserWithoutPermission(context);
        }
    }

    private static string TokenInRequest(AuthorizationFilterContext context)
    {
        var authorization = context.HttpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrWhiteSpace(authorization))
        {
            throw new TechChallengeException(string.Empty);
        }

        return authorization["Bearer".Length..].Trim();
    }

    private static void ExpiredToken(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new ErrorResponseJson(ErrorsMessages.ExpiredToken));
    }

    private static void ApiCalledError(AuthorizationFilterContext context, string errorMessage)
    {
        context.Result = new UnauthorizedObjectResult(new ErrorResponseJson(errorMessage));
    }

    private static void UserWithoutPermission(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new ErrorResponseJson(ErrorsMessages.UserWithoutPermission));
    }
}
