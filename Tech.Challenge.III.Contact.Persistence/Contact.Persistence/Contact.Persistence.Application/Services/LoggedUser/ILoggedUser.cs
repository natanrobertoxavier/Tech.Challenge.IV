﻿namespace Contact.Persistence.Application.Services.LoggedUser;
public interface ILoggedUser
{
    Task<Domain.Entities.User> RecoverUser();
}
