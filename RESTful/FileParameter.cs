using System;
using System.Collections.Generic;
using System.Text;

namespace RESTful
{
    public class FileParameter
    {
        public string FileName { get; set; }
        public string ParamName { get; set; }
        public byte[] File { get; set; }
    }
}
