using System;
using System.Collections.Generic;
using System.Text;

namespace PreviewDemo
{
    public class multiLocation
    {
        /// <summary>
        /// 
        /// </summary>
        public double left { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double top { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int width { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int height { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int rotation { get; set; }
    }

    public class multiUser_listItem
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

    public class multiFace_listItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string face_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public multiLocation location { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<multiUser_listItem> user_list { get; set; }
    }

    public class multiResult
    {
        /// <summary>
        /// 
        /// </summary>
        public int face_num { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<multiFace_listItem> face_list { get; set; }
    }

    public class FaceMultiSeach
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
        public int log_id { get; set; }
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
        public multiResult result { get; set; }
    }
}
