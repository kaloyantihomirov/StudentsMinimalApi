namespace StudentsMinimalApi
{
    public class ValidationHelper
    {
        public static async ValueTask<object?> ValidateId(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            string id = context.GetArgument<string>(0);

            if (string.IsNullOrEmpty(id) || !id.StartsWith('s'))
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                     { "id", new[] {" Id cannot be null or empty and must start with 's'" } }
                });
            }

            return await next(context);
        }
    }
}
