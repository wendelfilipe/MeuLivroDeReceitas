using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Criptography;
using MyRecipeBook.Communication;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserUseCase
    {
        private readonly IUserWriteOnlyRepository _writeOnlyRepository;
        private readonly IUserReadOnlyRepository _readOnlyRepository;
        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {

            // Criptografia da senha
            var criptografiaDeSenha = new PasswordEncripter();

            // Mapear a request em uma entidade
            var autoMapper = new AutoMapper.MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapping());
            }).CreateMapper();

            var user = autoMapper.Map<Domain.Entities.User>(request);

            user.Password = criptografiaDeSenha.Encrypt(request.Password);

           await _writeOnlyRepository.Add(user);

            // Validar a request
            Validate(request);



            // Salvar no banco de dados

            return new ResponseRegisteredUserJson
            {
                Name = request.Name
            };

        }

        public void Validate(RequestRegisterUserJson request)
        {
           var validator = new RegisterUserValidator();

           var result = validator.Validate(request);

           if(!result.IsValid)
           {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
           }
        }
    }
}