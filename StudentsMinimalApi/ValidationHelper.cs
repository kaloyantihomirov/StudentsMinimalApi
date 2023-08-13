namespace StudentsMinimalApi
{
    public class ValidationHelper 
    {
        public static async ValueTask<object?> ValidateId(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            string id = context.GetArgument<string>(0);

            return null;
        }


    }
}
