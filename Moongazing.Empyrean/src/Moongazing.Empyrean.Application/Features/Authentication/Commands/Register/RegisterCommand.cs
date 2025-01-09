using Doing.Retail.Application.Features.Authentication.Rules;
using MediatR;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Application.Services.Auth;
using Moongazing.Kernel.Application.Dtos;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Security.Hashing;
using Moongazing.Kernel.Security.Jwt;
using Moongazing.Kernel.Security.Models;
using Moongazing.Kernel.Security.Models.Enums;

namespace Moongazing.Empyrean.Application.Features.Authentication.Commands.Register;

public class RegisterCommand : IRequest<RegisterResponse>, ILoggableRequest, ITransactionalRequest, IIntervalRequest
{
    public UserRegisterDto UserRegisterDto { get; set; } = default!;
    public string IpAddress { get; set; } = default!;
    public int Interval => 15;


    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IAuthService authService;
        private readonly AuthBusinessRules authBusinessRules;

        public RegisterCommandHandler(IUserRepository userRepository,
                                      IAuthService authService,
                                      AuthBusinessRules authBusinessRules)
        {
            this.userRepository = userRepository;
            this.authService = authService;
            this.authBusinessRules = authBusinessRules;
        }

        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await authBusinessRules.UserEmailShouldBeNotExists(request.UserRegisterDto.Email);


            await authBusinessRules.UserPasswordAndConfirmPasswordShouldBeMatch(request.UserRegisterDto.Password,
                                                                                request.UserRegisterDto.RepeatPassword);

            HashingHelper.CreateHash(request.UserRegisterDto.Password,
                                     inputHash: out byte[] passwordHash,
                                     inputSalt: out byte[] passwordSalt);

            UserEntity newUser = new()
            {
                Email = request.UserRegisterDto.Email,
                FirstName = request.UserRegisterDto.FirstName,
                LastName = request.UserRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = UserStatus.Active,
            };

            UserEntity createdUser = await userRepository.AddAsync(newUser, cancellationToken);
            AccessToken createdAccessToken = await authService.CreateAccessToken(createdUser);

            RefreshTokenEntity createdRefreshToken = await authService.CreateRefreshToken(createdUser,
                                                                                          request.IpAddress);

            RefreshTokenEntity addedRefreshToken = await authService.AddRefreshToken(createdRefreshToken);

            RegisterResponse registeredResponse = new()
            {
                AccessToken = createdAccessToken,
                RefreshToken = addedRefreshToken
            };
            return registeredResponse;
        }
    }
}