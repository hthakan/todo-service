using System;

namespace ErrorHandler.Models
{
    public class BaseCustomException : Exception
    {
        private string _customcode;
        private int _code;
        private string _description;
        private string _extraparameter;
        private bool _islogged;
        private string _errortrackingid;

        public string CustomCode
        {
            get => _customcode;
        }
        public int Code
        {
            get => _code;
        }
        public string Description
        {
            get => _description;
        }
        public string Extraparameter
        {
            get => _extraparameter;
        }
        public bool isLogged
        {
            get => _islogged;
        }
        public string ErrorTrackingId
        {
            get => _errortrackingid;
        }

        public BaseCustomException(string customCode, string message, string description, int code, string extraParameter, bool islogged, string errorTrackingId) : base(message)
        {
            _customcode = customCode;
            _code = code;
            _description = description;
            _extraparameter = extraParameter;
            _errortrackingid = errorTrackingId;
            _islogged = islogged;
        }
    }
}
