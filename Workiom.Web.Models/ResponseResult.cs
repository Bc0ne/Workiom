namespace Workiom.Web.Models
{
    using System;
    using System.Collections.Generic;

    public class ResponseResult
    {
        public ResponseResult()
        {
            ValidationErrors = new List<string>();
        }

        public object Data { get; private set; }

        public string DeveloperMessage { get; private set; }

        public string ErrorMessage { get; private set; }

        public string ErrorCode { get; private set; }

        public List<string> ValidationErrors { get; private set; }

        public static ResponseResult SucceededWithData(object result)
        {
            return new ResponseResult
            {
                Data = result,
            };
        }

        public static ResponseResult Succeeded()
        {
            return new ResponseResult();
        }

        public static ResponseResult Failed(
            ErrorCode errorCode = Models.ErrorCode.Error,
            params string[] errors)
        {
            var result = new ResponseResult
            {
                ErrorCode = errorCode.ToString(),
                ErrorMessage = MapMessage(errorCode),
            };

            if (errors != null)
            {
                result.ValidationErrors.AddRange(errors);
            }

            return result;
        }

        private static string MapMessage(ErrorCode errorCode)
        {
            switch (errorCode)
            {
                case Models.ErrorCode.Error:
                    return "No error details available.";

                case Models.ErrorCode.ValidationError:
                    return "A validation error occurred.";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
