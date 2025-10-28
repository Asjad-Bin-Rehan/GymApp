


using CustomHTTP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System;
using System.Xml.Linq;

namespace CSAPI.Common
{
    public class ApiResult : ObjectResult
    {
        public static int CustomStatusCode = HTTPStatusCode400.UnprocessableEntity;
        public ApiResult(ModelStateDictionary keyValuePairs) : base(new ValidationResultModel(keyValuePairs, customStatusCode: CustomStatusCode))
        {
            StatusCode = CustomStatusCode;
        }
        public ApiResult(ApiResponseModel value) : base(value)
        {
        }
    }


    public class ApiResponseModel : IResult
    {
        public bool IsApiHandled { get; set; }
        public bool IsRequestSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = "";
        public Object Data { get; set; } = new object();
        public Object Exception { get; set; } = new List<string>();

        public async Task ExecuteAsync(HttpContext httpContext)
        {
            var response = httpContext.Response;
            response.StatusCode = StatusCode;
            response.ContentType = "application/json";

            var json = JsonConvert.SerializeObject(this, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            await response.WriteAsync(json);
        }
    }


    public class ApiResponseToken : ApiResponseModel
    {
        public string Token { get; set; } = "";
    }


    public static class ApiResponseHelper
    {
        public static ApiResponseModel Convert(bool IsRequestHandled, bool status, string message, int statusCode, Object data)
        {
            ApiResponseModel model = new ApiResponseModel();
            model.IsApiHandled = IsRequestHandled;
            model.IsRequestSuccess = status;
            model.StatusCode = statusCode;
            model.Message = message;
            model.Data = data;

            return model;
        }

        public static ApiResponseToken Convert(bool IsRequestHandled, bool status, string message, int statusCode, Object data, string token)
        {
            ApiResponseToken model = new ApiResponseToken();
            model.IsApiHandled = IsRequestHandled;
            model.IsRequestSuccess = status;
            model.StatusCode = statusCode;
            model.Message = message;
            model.Data = data;
            model.Token = token;

            return model;
        }
        public static ApiResponseModel Convert(bool IsRequestHandled, bool status, string message, int statusCode, Object data, Object exception)
        {
            ApiResponseModel model = new ApiResponseModel();
            model.IsApiHandled = IsRequestHandled;
            model.IsRequestSuccess = status;
            model.StatusCode = statusCode;
            model.Message = message;
            model.Data = data;
            model.Exception = exception;

            return model;
        }
    }


    public class ValidationResultModel : ApiResponseModel
    {
        public ValidationResultModel(ModelStateDictionary modelState, int customStatusCode)
        {
            IsRequestSuccess = false;
            Data = new object();
            IsApiHandled = true;
            StatusCode = customStatusCode;
            Message = "Invalid Request";


            Exception = modelState.Keys
                    .SelectMany(key => modelState[key]?.Errors?.Select(x => new ValidationError(key, x.ErrorMessage)) ?? new List<ValidationError>())
                    .ToList();

        }
    }


    public class ValidationError
    {
        public string Field { get; }

        public string Message { get; }

        public ValidationError(string field, string message)
        {
            Field = field;
            Message = message;
        }
    }

}