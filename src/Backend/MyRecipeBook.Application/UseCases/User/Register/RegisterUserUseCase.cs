using MyRecipeBook.Communication;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserUseCase
    {
        public ResponseRegisteredUserJson Execute(RequestRegisterUserJson request)
        {

            Validate(request);
            // Validar a request

            // Mapear a request em uma entidade

            // Criptografia da senha

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
                var errorMessages = result.Errors.Select(e => e.ErrorMessage);

                throw new Exception();
           }
        }
    }
}