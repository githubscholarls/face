using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PreviewDemo
{

    public class User_list
    {
        /// <summary>
        /// 
        /// </summary>
        public string group_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string user_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string user_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double score { get; set; }
    }

    public class ResultSearch
    {
        /// <summary>
        /// 
        /// </summary>
        public string face_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<User_list> user_list { get; set; }
    }

    public class FaceSearch
    {
        /// <summary>
        /// 
        /// </summary>
        public int error_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string error_msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string log_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int timestamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int cached { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ResultSearch result { get; set; }
    }

}
