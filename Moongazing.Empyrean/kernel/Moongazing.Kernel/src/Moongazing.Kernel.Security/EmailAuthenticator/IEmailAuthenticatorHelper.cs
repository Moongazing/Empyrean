﻿namespace Moongazing.Kernel.Security.EmailAuthenticator;

public interface IEmailAuthenticatorHelper
{
    public Task<string> CreateEmailActivationKey();
    public Task<string> CreateEmailActivationCode();
}