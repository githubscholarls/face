using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace PreviewDemo
{
    public class FaceGetUsersResult
    {
        public List<string> user_id_list { get; set; }
    }
    
    public class FaceGetGroupUsers
    {
        public FaceGetUsersResult result { get; set; }
    }
}
