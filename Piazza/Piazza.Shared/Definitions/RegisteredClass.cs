using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piazza.Definitions
{
    public class RegisteredClass
    {
        private String _title;
        public String Title
        {
            get
            {
                return Source + " : " + CourseName;
            }
        }
        public String CourseName { get; set; }
        public String CourseID { get; set; }
        public String Term { get; set; }
        public String Source { get; set; }
        public String Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class RegisteredClassGroup : List<RegisteredClass>
    {
        public RegisteredClassGroup()
        { }
    }
}
