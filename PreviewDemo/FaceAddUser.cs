using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreviewDemo
{
    public class AddUserLocation
    {
        public double left { get; set; }
        public double top { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public Int64 rotation { get; set; }
    }

    public class AddUserResult
    {
        public string face_token { get; set; }
        public AddUserLocation location { get; set; }
    }

    public class FaceAddUser
    {
        public int error_code { get; set; }
        public string error_msg { get; set; }
        public string log_id { get; set; }
        public string timestamp { get; set; }
        public int cached { get; set; }
        public AddUserResult result { get; set; }
    }
}
