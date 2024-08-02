using Application.Services;
using Application.Services.Concrete;
using Core.Constants;

namespace NTierArch
{
    public static class Program
    {
        private static readonly GroupService _groupService;
        private static readonly StudentService _studentService;

        static Program()
        {
            _groupService = new GroupService();
            _studentService = new StudentService();
        }

        public static void Main()
        {
            while (true)
            {
                ShowMenu();
                Messages.InputMessage("choice");
                string choiceInput = Console.ReadLine();
                int choice;
                bool isSucceeded = int.TryParse(choiceInput, out choice);

                if (isSucceeded)
                {
                    switch ((Operation)choice)
                    {
                        case Operation.GetAllGroups:
                            _groupService.GetAllGroups();
                            break;
                        case Operation.AddGroup:
                            _groupService.AddGroup();
                            break;
                        case Operation.UpdateGroup:
                            _groupService.UpdateGroup();
                            break;
                        case Operation.DeleteGroup:
                            _groupService.DeleteGroup();
                            break;
                        case Operation.GetGroupDetails:
                            _groupService.GetGroupDetails();
                            break;
                        case Operation.GetAllStudents:
                            _studentService.GetAllStudents();
                            break;
                        case Operation.AddStudent:
                            _studentService.AddStudent();
                            break;
                        case Operation.UpdateStudent:
                            _studentService.UpdateStudent();
                            break;
                        case Operation.DeleteStudent:
                            _studentService.DeleteStudent();
                            break;
                        case Operation.GetStudentDetails:
                            _studentService.GetStudentDetails();
                            break;
                        case Operation.Exit:
                            return;
                        default:
                            Messages.InvalidInputMessage("Choice");
                            break;
                    }
                }
                else
                {
                    Messages.InvalidInputMessage("Choice");
                }
            }
        }

        public static void ShowMenu()
        {
            Console.WriteLine("---MENU----");
            Console.WriteLine("1. All groups");
            Console.WriteLine("2. Add group");
            Console.WriteLine("3. Update group");
            Console.WriteLine("4. Delete group");
            Console.WriteLine("5. Details of group");
            Console.WriteLine("6. All students");
            Console.WriteLine("7. Add student");
            Console.WriteLine("8. Update student");
            Console.WriteLine("9. Delete student");
            Console.WriteLine("10. Details of student");
            Console.WriteLine("0. Exit");
        }
    }
}
