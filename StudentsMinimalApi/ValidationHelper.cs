using System.Reflection;

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
                     { "id", new[] { "Id cannot be null or empty and must start with 's'" } }
                });
            }

            return await next(context);
        }
        /// <summary>
        /// The filter factory is a function that takes an EndpointFilterFactoryContext 
        /// and a filter (represented by EndpointFilterDelegate) and returns a new filter. A filter is a function 
        /// (represented by EndpointDelegate).
        /// </summary>
        /// <param name="context">A context object that exposes information about the route handler being intercepted.</param>
        /// <param name="next">A delegate type that represents invocation of the filter chain.</param>
        /// <returns>The actual filter</returns>
        public static EndpointFilterDelegate ValidateIdFactory(
            EndpointFilterFactoryContext context,
            EndpointFilterDelegate next)
        {
            ParameterInfo[] parameters = context.MethodInfo.GetParameters();

            int? idPosition = null;

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].Name == "id" && parameters[i].ParameterType == typeof(string))
                {
                    idPosition = i;
                    break;
                }
            }

            if (!idPosition.HasValue)
            {
                return next;
            }

            ///<remarks>Here we are actually defining the Filter that we're returning</remarks>
            async ValueTask<object?> Filter(EndpointFilterInvocationContext invocationContext)
            {
                string id = invocationContext.GetArgument<string>(idPosition.Value);

                if (string.IsNullOrEmpty(id) || !id.StartsWith('s'))
                {
                    return Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        { "id", new[] { "Id cannot be null or empty and must start with 's'" } }
                    });
                }

                return await next(invocationContext);
            };

            return Filter;

            //return async (invocationContext) =>
            //{
            //    string id = invocationContext.GetArgument<string>(idPosition.Value);

            //    if (string.IsNullOrEmpty(id) || !id.StartsWith('s'))
            //    {
            //        return Results.ValidationProblem(new Dictionary<string, string[]>
            //        {
            //{ "id", new[] { "Id cannot be null or empty and must start with 's'" } }
            //        });
            //    }

            //    return await next(invocationContext);
            //};


        }
    }
}
