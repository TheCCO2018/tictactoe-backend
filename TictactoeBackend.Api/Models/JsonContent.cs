using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TictactoeBackend.Api.Models
{
    public class JsonContent<T> where T: class
    {
        public string Result { get; set; }
        public IList<T> Data { get; set; }
    }
}