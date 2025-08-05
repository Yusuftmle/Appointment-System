using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class ResultModel<T>
    {
        public bool IsSuccess { get; }
        public T Data { get; }
        public string Error { get; }

        private ResultModel(bool isSuccess, T data, string error)
        {
            IsSuccess = isSuccess;
            Data = data;
            Error = error;
        }

        public static ResultModel<T> Success(T data) => new(true, data, null);
        public static ResultModel<T> Failure(string error) => new(false, default, error);
    }
}
