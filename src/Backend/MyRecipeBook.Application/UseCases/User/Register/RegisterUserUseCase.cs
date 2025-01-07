using AutoMapper;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Criptography;
using MyRecipeBook.Communication;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserWriteOnlyRepository writeOnlyRepository;
        private readonly IUserReadOnlyRepository readOnlyRepository;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly PasswordEncripter passwordEncripter;

        public RegisterUserUseCase(
            IUserReadOnlyRepository readOnlyRepository, 
            IUserWriteOnlyRepository writeOnlyRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            PasswordEncripter passwordEncripter)
        {
            this.writeOnlyRepository = writeOnlyRepository;
            this.readOnlyRepository = readOnlyRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.passwordEncripter = passwordEncripter;
        }
        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            // Validar a request
            await Validate(request);

            // Criptografia da senha

            var user = mapper.Map<Domain.Entities.User>(request);
            user.Password = passwordEncripter.Encrypt(request.Password);

            await writeOnlyRepository.Add(user);

            await unitOfWork.Commit();

            // Salvar no banco de dados

            return new ResponseRegisteredUserJson
            {
                Name = request.Name
            };

        }

        private async Task Validate(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            var emailExist = await readOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (emailExist)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREDY_REGISTED));
            }

            if(!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}