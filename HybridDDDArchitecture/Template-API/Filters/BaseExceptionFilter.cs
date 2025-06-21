using Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Filters
{
    /// <summary>
    /// Clase base para el manejo de excepciones. Modificar solo si es requerido
    /// </summary>
    public class BaseExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            //generamos el mensaje de error
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)GetErrorCode(context.Exception.GetType());
            response.ContentType = "application/json";

            string resultMessage = context.Exception.Message;
            string errorCode = Guid.NewGuid().ToString();

            var result = new ObjectResult(new HttpMessageResult()
            {
                Success = false,
                Data = string.Empty,
                Message = IsManagedException(context.Exception) ? resultMessage : "His operation could not be completed. try again later. Error Code (" + errorCode + ").",
                Code = errorCode,
                StatusCode = response.StatusCode
            });
            context.Result = result;
        }

        private HttpStatusCode GetErrorCode(Type exceptionType)
        {
            Exceptions IsException;
            if (Enum.TryParse(exceptionType.Name, out IsException))
            {
                switch (IsException)
                {
                    case Exceptions.ArgumentNullException:
                        return HttpStatusCode.LengthRequired;

                    case Exceptions.BussinessException:
                    case Exceptions.EntityDoesExistException:
                    case Exceptions.InvalidEntityDataException:
                        return HttpStatusCode.BadRequest;

                    case Exceptions.EntityDoesNotExistException:
                        return HttpStatusCode.NotFound;

                    default:
                        return HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                return HttpStatusCode.InternalServerError;
            }   
        }

        private bool IsManagedException(Exception ex)
        {
            return ex is ApplicationException;
        }
    }

    public enum Exceptions
    {
        ArgumentNullException,
        BussinessException,
        EntityDoesExistException,
        EntityDoesNotExistException,
        InvalidEntityDataException
    }
}
