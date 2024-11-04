namespace DoctorAppointmentSystemAPI.Exceptions
{
    public class ExceptionHandling
    {
        private readonly RequestDelegate _next;

        public ExceptionHandling(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                InvalidOperationException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            var result = new
            {
                error = exception.Message
            };
            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
        }
    }
}
