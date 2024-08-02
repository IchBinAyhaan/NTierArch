using Application.Services.Abstract;
using Core.Constants;
using Core.Entities;
using Core.Extensions;
using Data.Repositories.Concrete;
using Data.UnitOfWork.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Concrete
{
    public class StudentService : IStudentService
    {
        private readonly UnitOfWork _unitOfWork;
        public StudentService()
        {
            _unitOfWork = new UnitOfWork();
        }
        public void GetAllStudents()
        {
            foreach (var student in _unitOfWork.Students.GetAll().Where(s => !s.IsDeleted).ToList())
            {
                Console.WriteLine($"id:{student.Id}, Name:{student.Name}, Surname:{student.Surname}");
            }
        }

        public void AddStudent()
        {
        StudentNameInput: Messages.InputMessage("student name");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInputMessage("student name");
                goto StudentNameInput;
            }

        StudentSurnameInput: Messages.InputMessage("student surname");
            string surname = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(surname))
            {
                Messages.InvalidInputMessage("student surname");
                goto StudentSurnameInput;
            }

            var student = new Student
            {
                Name = name,
                Surname = surname
            };

            _unitOfWork.Students.Add(student);
            try
            {
                _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                Messages.ErrorOccuredMessage();
            }

            Messages.SuccessMessage("student", "added");
        }

        public void UpdateStudent()
        {
            GetAllStudents();

        StudentIdInput: Messages.InputMessage("Student id");
            var idInput = Console.ReadLine();
            if (!int.TryParse(idInput, out int studentId))
            {
                Messages.InvalidInputMessage("Student id");
                goto StudentIdInput;
            }

            var student = _unitOfWork.Students.GetById(studentId);
            if (student == null)
            {
                Messages.NotFoundMessage("Student");
                return;
            }

        StudentNameInput: Messages.WantToChangeMessage("name");
            var choiceInput = Console.ReadLine();
            if (!char.TryParse(choiceInput, out char choice) || !choice.IsvalidChoice())
            {
                Messages.InvalidInputMessage("choice");
                goto StudentNameInput;
            }

            string newName = string.Empty;
            if (choice == 'y')
            {
            NewNameInput: Messages.InputMessage("new name");
                newName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newName))
                {
                    goto NewNameInput;
                }
            }

        StudentSurnameInput: Messages.WantToChangeMessage("surname");
            choiceInput = Console.ReadLine();
            if (!char.TryParse(choiceInput, out choice) || !choice.IsvalidChoice())
            {
                Messages.InvalidInputMessage("choice");
                goto StudentSurnameInput;
            }

            string newSurname = string.Empty;
            if (choice == 'y')
            {
            NewSurnameInput: Messages.InputMessage("new surname");
                newSurname = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newSurname))
                {
                    goto NewSurnameInput;
                }
            }

            if (!string.IsNullOrEmpty(newName)) student.Name = newName;
            if (!string.IsNullOrEmpty(newSurname)) student.Surname = newSurname;

            _unitOfWork.Students.Update(student);

            try
            {
                _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                Messages.ErrorOccuredMessage();
            }

            Messages.SuccessMessage("Student", "updated");
        }

        public void DeleteStudent()
        {
            GetAllStudents();

        StudentIdInput: Messages.InputMessage("Student id");
            var idInput = Console.ReadLine();
            if (!int.TryParse(idInput, out int id))
            {
                Messages.InvalidInputMessage("Student id");
                goto StudentIdInput;
            }

            var student = _unitOfWork.Students.GetById(id);
            if (student == null)
            {
                Messages.NotFoundMessage("Student");
                return;
            }

            student.IsDeleted = true;
            _unitOfWork.Students.Update(student);

            try
            {
                _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                Messages.ErrorOccuredMessage();
            }

            Messages.SuccessMessage("Student", "deleted");
        }

        public void GetStudentDetails()
        {
            GetAllStudents();

        StudentIdInput: Messages.InputMessage("Student id");
            var idInput = Console.ReadLine();
            if (!int.TryParse(idInput, out int id))
            {
                Messages.InvalidInputMessage("Student id");
                goto StudentIdInput;
            }

            var student = _unitOfWork.Students.GetById(id);
            if (student == null)
            {
                Messages.NotFoundMessage("Student");
                return;
            }

            Console.WriteLine($"id:{student.Id}, Name:{student.Name}, Surname: {student.Surname}");
            if (student.Group != null)
            {
                Console.WriteLine($"Group: {student.Group.Name}");
            }
        }
    }
}
    
    