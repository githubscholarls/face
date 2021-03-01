using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreviewDemo
{
    public class Occlusion
    {
        public double left_eye { get; set; }
        public double right_eye { get; set; }
        public double nose { get; set; }
        public double mouth { get; set; }
        public double left_cheek { get; set; }
        public double right_cheek { get; set; }
        public double chin { get; set; }
    }
    public class Quality
    {
        public double blur { get; set; }
        public double illumination { get; set; }
        public int completeness { get; set; }
    }
 public class Location
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

    public class Angle
    {
        /// <summary>
        /// 
        /// </summary>
        public double yaw { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double pitch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double roll { get; set; }
    }

    public class Liveness
    {
        /// <summary>
        /// 
        /// </summary>
        public double livemapscore { get; set; }
    }

    public class Gender
    {
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int probability { get; set; }
    }

    public class Expression
    {
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int probability { get; set; }
    }

    public class Face_list
    {
        /// <summary>
        /// 
        /// </summary>
        public string face_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Location location { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double face_probability { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Angle angle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Liveness liveness { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int age { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Gender gender { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Expression expression { get; set; }
        public Quality quality { get; set; }
    }

    public class Result
    {
        /// <summary>
        /// 
        /// </summary>
        public int face_num { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Face_list> face_list { get; set; }
    }

    public class FaceidentifyInfo
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
        public long log_id { get; set; }
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
        public Result result { get; set; }



    }


}
