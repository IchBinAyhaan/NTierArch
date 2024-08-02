using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Abstract
{
    public interface IStudentService
    {
        public void AddStudent();
        void GetAllStudents();
        void UpdateStudent();
        void DeleteStudent();
        void GetStudentDetails();

    }
}
