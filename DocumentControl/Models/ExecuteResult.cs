using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentControl.Models
{
    public class ExecuteResult
    {
        public bool isError { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
    }
}