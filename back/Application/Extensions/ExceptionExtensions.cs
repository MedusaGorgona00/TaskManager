using Application.Exceptions;
using FluentValidation;

namespace Application.Extensions
{
    /// <summary>
    ///     Extensions for Exceptions
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        ///     Fluent validation exception map to Dictionary
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> MapException(this ValidationException ex)
        {
            var model = new Dictionary<string, List<string>>();
            foreach (var item in ex.Errors)
            {
                if (!model.ContainsKey(item.PropertyName))
                {
                    model.Add(item.PropertyName, new List<string>());
                }

                model[item.PropertyName].Add(item.ErrorMessage);
            }

            return model;
        }

        /// <summary>
        ///     Exception map to Dictionary
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> MapException(this Exception ex)
        {
            return new Dictionary<string, List<string>> { { string.Empty, new List<string> { ex.Message } } };
        }

        /// <summary>
        ///     Http exception map to Dictionary
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> MapException(this HttpException ex)
        {
            return new Dictionary<string, List<string>> { { ex.Property, ex.Errors } };
        }
    }
}
