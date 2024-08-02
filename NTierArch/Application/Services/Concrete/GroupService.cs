using Application.Services.Abstract;
using Core.Constants;
using Core.Entities;
using Core.Extensions;
using Data.Contexts;
using Data.Repositories.Concrete;
using Data.UnitOfWork.Abstract;
using Data.UnitOfWork.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.Services.Concrete
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentService _studentService;

        public GroupService(IUnitOfWork unitOfWork, IStudentService studentService)
        {
            _unitOfWork = unitOfWork;
            _studentService = studentService;
        }

        public void GetAllGroups()
        {
            var groups = _unitOfWork.Groups.GetAll().Where(g => !g.IsDeleted).ToList();
            foreach (var group in groups)
            {
                Console.WriteLine($"Id: {group.Id}, Name: {group.Name}, Limit: {group.Limit}, StudentId: {group.StudentId}");
            }
        }

        public void AddGroup()
        {
        GroupNameInput:
            Messages.InputMessage("Group name");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name) || _unitOfWork.Groups.GetAll().Any(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                Messages.AlreadyExistMessage($"Group name - {name}");
                goto GroupNameInput;
            }

        GroupLimitInput:
            Messages.InputMessage("Group limit");
            if (!int.TryParse(Console.ReadLine(), out int limit) || limit <= 0)
            {
                Messages.InvalidInputMessage("Group limit");
                goto GroupLimitInput;
            }

            var group = new Group
            {
                Name = name,
                Limit = limit,
                IsDeleted = false
            };

            _unitOfWork.Groups.Add(group);
            try
            {
                _unitOfWork.SaveChanges();
                Messages.SuccessMessage("Group", "added");
            }
            catch (Exception)
            {
                Messages.ErrorOccuredMessage();
            }
        }

        public void UpdateGroup()
        {
            GetAllGroups();

        GroupIdInput:
            Messages.InputMessage("Group id");
            if (!int.TryParse(Console.ReadLine(), out int groupId))
            {
                Messages.InvalidInputMessage("Group id");
                goto GroupIdInput;
            }

            var group = _unitOfWork.Groups.GetAll().FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                Messages.NotFoundMessage("Group");
                return;
            }

            UpdateGroupDetails(group);
        }

        private void UpdateGroupDetails(Group group)
        {
        ChangeName:
            Messages.WantToChangeMessage("name");
            if (!char.TryParse(Console.ReadLine(), out char choice) || (choice != 'y' && choice != 'n'))
            {
                Messages.InvalidInputMessage("choice");
                goto ChangeName;
            }

            if (choice == 'y')
            {
                Messages.InputMessage("new name");
                string newName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newName) && !_unitOfWork.Groups.GetAll().Any(g => g.Name.Equals(newName, StringComparison.OrdinalIgnoreCase) && g.Id != group.Id))
                {
                    group.Name = newName;
                }
                else
                {
                    Messages.AlreadyExistMessage("Group name");
                    goto ChangeName;
                }
            }

        ChangeLimit:
            Messages.WantToChangeMessage("limit");
            if (!char.TryParse(Console.ReadLine(), out char limitChoice) || (limitChoice != 'y' && limitChoice != 'n'))
            {
                Messages.InvalidInputMessage("choice");
                goto ChangeLimit;
            }

            if (limitChoice == 'y')
            {
                Messages.InputMessage("new limit");
                if (int.TryParse(Console.ReadLine(), out int newLimit) && newLimit > 0)
                {
                    int studentCountInGroup = _unitOfWork.Groups.GetAll().Count(g => g.Id == group.Id);
                    if (studentCountInGroup <= newLimit)
                    {
                        group.Limit = newLimit;
                    }
                    else
                    {
                        Messages.LimitMessage("Limit", studentCountInGroup);
                        goto ChangeLimit;
                    }
                }
                else
                {
                    Messages.InvalidInputMessage("Group limit");
                    goto ChangeLimit;
                }
            }

        ChangeStudent:
            Messages.WantToChangeMessage("student");
            if (!char.TryParse(Console.ReadLine(), out char studentChoice) || (studentChoice != 'y' && studentChoice != 'n'))
            {
                Messages.InvalidInputMessage("choice");
                goto ChangeStudent;
            }

            if (studentChoice == 'y')
            {
                _studentService.GetAllStudents();
            SelectStudentId:
                Messages.InputMessage("Student id");
                if (int.TryParse(Console.ReadLine(), out int newStudentId) &&
                    _studentService.GetAllStudents().Any(s => s.Id == newStudentId))
                {
                    group.StudentId = newStudentId;
                }
                else
                {
                    Messages.InvalidInputMessage("Student id");
                    goto SelectStudentId;
                }
            }

            _unitOfWork.Groups.Update(group);
            try
            {
                _unitOfWork.SaveChanges();
                Messages.SuccessMessage("Group", "updated");
            }
            catch (Exception)
            {
                Messages.ErrorOccuredMessage();
            }
        }

        public void DeleteGroup()
        {
            GetAllGroups();

        GroupIdInput:
            Messages.InputMessage("Group id");
            if (!int.TryParse(Console.ReadLine(), out int groupId))
            {
                Messages.InvalidInputMessage("Group id");
                goto GroupIdInput;
            }

            var group = _unitOfWork.Groups.GetAll().FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                Messages.NotFoundMessage("Group");
                return;
            }

            group.IsDeleted = true;
            _unitOfWork.Groups.Update(group);
            try
            {
                _unitOfWork.SaveChanges();
                Messages.SuccessMessage("Group", "deleted");
            }
            catch (Exception)
            {
                Messages.ErrorOccuredMessage();
            }
        }

        public void GetGroupDetails()
        {
            GetAllGroups();

        GroupIdInput:
            Messages.InputMessage("Group id");
            if (!int.TryParse(Console.ReadLine(), out int groupId))
            {
                Messages.InvalidInputMessage("Group id");
                goto GroupIdInput;
            }

            var group = _unitOfWork.Groups.GetAll().FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                Messages.NotFoundMessage("Group");
                return;
            }

            Console.WriteLine($"Id: {group.Id}, Name: {group.Name}, Limit: {group.Limit}");

            var studentsInGroup = _studentService.GetAllStudents().Where(s => s.GroupId == groupId).ToList();
            foreach (var student in studentsInGroup)
            {
                Console.WriteLine($"Student: {student.Name} {student.Surname}");
            }
        }
    }
}
