using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Shared.ApiResult
{
    public record class BaseResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the action was successful.
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// On failure, the problem details are parsed and returned in this array.
        /// </summary>
        public string[]? ErrorList { get; set; }

        public BaseResult()
        {
            Succeeded = true;
        }
        public BaseResult(string error)
        {
            Succeeded = false;
            ErrorList = new string[] { error };
        }

        public BaseResult(string[] error)
        {
            Succeeded = false;
            ErrorList = error;
        }
    }

    public record class TResult<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the action was successful.
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// On failure, the problem details are parsed and returned in this array.
        /// </summary>
        public string[] ErrorList { get; set; } = [];

        /// <summary>
        /// Gets or sets the result of the action.
        /// </summary>
        public T? Data { get; set; }

        public TResult(T? data)
        {
            Succeeded = true;
            Data = data;
        }
    }

    public record class JResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the action was successful.
        /// </summary>
        public bool Succeeded { get; set; }
        /// <summary>
        /// On failure, the problem details are parsed and returned in this array.
        /// </summary>
        public string[] ErrorList { get; set; } = [];


        public object? Data { get; set; }

        public JResult(object? data)
        {
            Succeeded = true;
            Data = data;
        }

        public JResult(bool succeeded, object? data)
        {
            Succeeded = succeeded;
            Data = data;
        }

    }
}
