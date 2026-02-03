using YASHOP.DAL.DTO.Response;

namespace YASHOP.PL.Middleware
{
    public class GlobalExeptionHandling
    {
        private readonly RequestDelegate next;

        public GlobalExeptionHandling(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);

            }
            catch (Exception ex)
            {
                var errorDeatils = new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message ="Server Error ...",
                    StackTrace = ex.InnerException.Message
                };
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(errorDeatils);
            }
        }
    }
}
