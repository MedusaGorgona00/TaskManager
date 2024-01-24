using System.Collections.Generic;

namespace Application.Models
{
    public class ErrorResponse
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ErrorResponse()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="property"></param>
        /// <param name="messages"></param>
        public ErrorResponse(List<string> messages, string? property = null)
        {
            Errors = new Dictionary<string, List<string>> { { property ?? string.Empty, messages } };
        }

        /// <summary>
        /// </summary>
        /// <param name="errors"></param>
        public ErrorResponse(Dictionary<string, List<string>> errors)
        {
            Errors = errors;
        }

        /// <summary>
        ///     Error Dictionary
        /// </summary>
        public Dictionary<string, List<string>>? Errors { get; set; } = null;
    }
}
