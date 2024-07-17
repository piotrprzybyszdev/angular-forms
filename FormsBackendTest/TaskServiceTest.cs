using AutoMapper;
using FormBackendTest.Mock;
using FormsBackendBusiness;
using FormsBackendBusiness.Exceptions;
using FormsBackendBusiness.Services;
using FormsBackendBusiness.Validation;
using FormsBackendCommon.Dtos.Task;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using System.Threading.Tasks.Sources;

namespace FormBackendTest;

[TestFixture]
public class TaskServiceTest
{
    readonly Mapper mapper = (Mapper)DtoEntityMapperProfile.GetConfiguration().CreateMapper();
    MockTaskRepository taskRepository = new();
    MockUserRepository userRepository = new();

    TaskService CreateTaskService()
    {
        taskRepository = new();
        userRepository = new();

        return new(
            taskRepository, userRepository, mapper,
            new TaskCreateValidator(), new TaskUpdateValidator()
        );
    }

    [Test]
    public void CreateSuccessTest()
    {
        var taskService = CreateTaskService();
        userRepository.Users = [
            new ApplicationUser()
            {
                Id = "testid",
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jankowalski@gmail.com",
            }
        ];

        var taskCreate = new TaskCreate("testid", "testing", "testing creation method", DateTime.Now.AddDays(1));

        taskService.CreateTaskAsync(taskCreate).Wait();

        var operation = taskRepository.Operations.Single(operation => operation.OperationType == TaskRepositoryOperationType.Insert);
        var taskModel = (TaskModel)operation.Arguments.First();

        Assert.That(taskModel.Title, Is.EqualTo(taskCreate.Title));
        Assert.That(taskModel.Description, Is.EqualTo(taskCreate.Description));
        Assert.That(taskModel.DueDate, Is.EqualTo(taskCreate.DueDate));
        Assert.That(taskModel.Account.Id, Is.EqualTo("testid"));
        Assert.That(taskModel.CreationDate, Is.LessThan(DateTime.Now));
        Assert.That(taskModel.CreationDate, Is.GreaterThan(DateTime.Now.AddMinutes(-1)));
        Assert.That(taskModel.ModificationDate, Is.LessThan(taskModel.CreationDate.AddMilliseconds(100)));
        Assert.That(taskModel.ModificationDate, Is.GreaterThan(taskModel.CreationDate.AddMilliseconds(-100)));

        Assert.That(taskRepository.Operations.Last().OperationType, Is.EqualTo(TaskRepositoryOperationType.SaveChanges));
        Assert.IsFalse(userRepository.Operations.Exists(operation => operation.OperationType == UserRepositoryOperationType.SaveChanges));
    }

    [Test]
    public void CreateFailTest()
    {
        var taskService = CreateTaskService();
        var taskCreate = new TaskCreate("testid", "testing", "testing creation method", DateTime.Now.AddDays(1));

        var task = taskService.CreateTaskAsync(taskCreate);
        Assert.Throws<AggregateException>(task.Wait);
        Assert.That(task.Exception?.InnerException, Is.InstanceOf<UserNotFoundException>());
    }

    [Test]
    public void UpdateSuccessTest()
    {
        var taskService = CreateTaskService();
        userRepository.Users = [
            new ApplicationUser()
            {
                Id = "testid",
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jankowalski@gmail.com",
            }
        ];
        var dt = DateTime.Now;
        taskRepository.Tasks = [
            new TaskModel()
            {
                Id = 1,
                Account = userRepository.Users[0],
                Title = "testing",
                Description = "testing update method",
                DueDate = dt.AddDays(1),
                CreationDate = dt,
                ModificationDate = dt
            }
        ];

        var dt2 = DateTime.Now.AddDays(2);
        var taskUpdate = new TaskUpdate(1, "testing", "done testing update method", dt2);
        taskService.UpdateTaskAsync(taskUpdate).Wait();

        var operation = taskRepository.Operations.Single(operation => operation.OperationType == TaskRepositoryOperationType.Update);
        var taskModel = (TaskModel)operation.Arguments.First();

        Assert.That(taskModel.Id, Is.EqualTo(1));
        Assert.That(taskModel.Account.Id, Is.EqualTo("testid"));
        Assert.That(taskModel.Title, Is.EqualTo("testing"));
        Assert.That(taskModel.Description, Is.EqualTo("done testing update method"));
        Assert.That(taskModel.DueDate, Is.EqualTo(dt2));
        Assert.That(taskModel.CreationDate, Is.EqualTo(dt));
        Assert.That(taskModel.ModificationDate, Is.LessThan(DateTime.Now.AddMilliseconds(100)));
        Assert.That(taskModel.ModificationDate, Is.GreaterThan(DateTime.Now.AddMilliseconds(-100)));

        Assert.That(taskRepository.Operations.Last().OperationType, Is.EqualTo(TaskRepositoryOperationType.SaveChanges));
    }

    [Test]
    public void UpdateFailTest()
    {
        var taskService = CreateTaskService();

        var taskUpdate = new TaskUpdate(1, "testing", "testing update method", DateTime.Now);
        var task = taskService.UpdateTaskAsync(taskUpdate);

        Assert.Throws<AggregateException>(task.Wait);
        Assert.That(task.Exception?.InnerException, Is.InstanceOf<TaskNotFoundException>());
    }

    [Test]
    public void DeleteSuccessTest()
    {
        var taskService = CreateTaskService();
        userRepository.Users = [
            new ApplicationUser()
            {
                Id = "testid",
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jankowalski@gmail.com",
            }
        ];
        var dt = DateTime.Now;
        taskRepository.Tasks = [
            new TaskModel()
            {
                Id = 1,
                Account = userRepository.Users[0],
                Title = "testing",
                Description = "testing update method",
                DueDate = dt.AddDays(1),
                CreationDate = dt,
                ModificationDate = dt
            }
        ];

        taskService.DeleteTaskAsync(1).Wait();

        var operation = taskRepository.Operations.Single(operation => operation.OperationType == TaskRepositoryOperationType.Delete);
        var taskModel = (TaskModel)operation.Arguments.First();

        Assert.That(taskModel.Id, Is.EqualTo(1));
        Assert.That(taskRepository.Operations.Last().OperationType, Is.EqualTo(TaskRepositoryOperationType.SaveChanges));
    }

    [Test]
    public void DeleteFailTest()
    {
        var taskService = CreateTaskService();

        var task = taskService.DeleteTaskAsync(1);

        Assert.Throws<AggregateException>(task.Wait);
        Assert.That(task.Exception?.InnerException, Is.InstanceOf<TaskNotFoundException>());
    }

    [Test]
    public void GetByUserIdTest()
    {
        var dt = DateTime.Now.AddDays(0.5);
        var dt2 = DateTime.Now.AddDays(1);

        var taskService = CreateTaskService();
        userRepository.Users = [
            new ApplicationUser()
            {
                Id = "testid",
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jankowalski@gmail.com",
            },
            new ApplicationUser()
            {
                Id = "testid2",
                FirstName = "Adam",
                LastName = "Nowak",
                Email = "adamnowak@gmail.com",
            }
        ];
        taskRepository.Tasks = [
            new TaskModel()
            {
                Id = 1,
                Account = userRepository.Users[0],
                Title = "testing",
                Description = "testing get",
                DueDate = dt2,
                CreationDate = dt,
                ModificationDate = dt
            },
            new TaskModel()
            {
                Id = 2,
                Account = userRepository.Users[1],
                Title = "testing",
                Description = "testing get 2",
                DueDate = dt2,
                CreationDate = dt,
                ModificationDate = dt
            }
        ];

        var task = taskService.GetTasksByUserIdAsync("testid");
        task.Wait();
        var tasks = task.Result;
        
        Assert.That(tasks.Count, Is.EqualTo(1));
        var taskModel = tasks[0];
        Assert.That(taskModel.Id == 1 && taskModel.Title == "testing" && taskModel.Description == "testing get"
            && taskModel.DueDate == dt2 && taskModel.ModificationDate == dt && taskModel.CreationDate == dt);
    }
}
