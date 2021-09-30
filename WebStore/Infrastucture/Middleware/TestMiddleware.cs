using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebStore.Infrastucture.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<TestMiddleware> _Logger;

        public TestMiddleware(RequestDelegate next, ILogger<TestMiddleware> Logger)
        {
            _Next = next;
            _Logger = Logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            // Предобработка

            var processing = _Next(context); // Запуск следующего  слоя промеж. ПО

            // обработка параллельно

            await processing; // Ожидание завершения обработки следующей частью конвейра

            // постобработка данных
        }

    }

}
