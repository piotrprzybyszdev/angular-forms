using AutoMapper;
using FormBackendTest.Mock;
using FormsBackendBusiness;
using FormsBackendBusiness.Exceptions;
using FormsBackendBusiness.Services;
using FormsBackendBusiness.Validation;
using FormsBackendCommon.Dtos.User;
using FormsBackendCommon.Model;

namespace FormBackendTest;

[TestFixture]
public class UserServiceTest
{
    readonly Mapper mapper = (Mapper)DtoEntityMapperProfile.GetConfiguration().CreateMapper();
    MockGenericRepository<UserModel> userRepository = new();
    MockTaskService taskService = new();

    [Test]
    public void CreateSuccessTest()
    {
        var userService = CreateUserService();

        var userCreate = new UserCreate("Jan", "Kowalski", "jankowalski@gmail.com", "test123");
        userService.CreateUserAsync(userCreate).Wait();

        var operation = userRepository.Operations.Single(operation => operation.OperationType == RepositoryOperationType.Insert);
        var applicationUser = (UserModel)operation.Arguments.First();

        Assert.That(applicationUser.FirstName, Is.EqualTo(userCreate.FirstName));
        Assert.That(applicationUser.LastName, Is.EqualTo(userCreate.LastName));
        Assert.That(applicationUser.Email, Is.EqualTo(userCreate.Email));

        Assert.That(userRepository.Operations.Last().OperationType, Is.EqualTo(RepositoryOperationType.SaveChanges));
    }

    [Test]
    public void CreateFailTest()
    {
        var userService = CreateUserService();
        userRepository.Models = [
            new UserModel()
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jankowalski@gmail.com",
            }
        ];

        var userCreate = new UserCreate("Jan", "Kowalski", "jankowalski@gmail.com", "test123");
        var task = userService.CreateUserAsync(userCreate);
        Assert.Throws<AggregateException>(task.Wait);
        Assert.That(task.Exception?.InnerException, Is.InstanceOf<ValidationFailedException>());
    }

    [Test]
    public void UpdateSuccessTest()
    {
        var userService = CreateUserService();
        userRepository.Models = [
            new UserModel()
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jankowalski@gmail.com",
            }
        ];

        var userUpdate = new UserUpdate(1, "Janusz", "Kowal", "januszkowal@yahoo.de");
        userService.UpdateUserAsync(userUpdate).Wait();

        var operation = userRepository.Operations.Single(operation => operation.OperationType == RepositoryOperationType.Update);
        var applicationUser = (UserModel)operation.Arguments.First();

        Assert.That(applicationUser.Id, Is.EqualTo(1));
        Assert.That(applicationUser.FirstName, Is.EqualTo(userUpdate.FirstName));
        Assert.That(applicationUser.LastName, Is.EqualTo(userUpdate.LastName));
        Assert.That(applicationUser.Email, Is.EqualTo(userUpdate.Email));

        Assert.That(userRepository.Operations.Last().OperationType, Is.EqualTo(RepositoryOperationType.SaveChanges));
    }

    [Test]
    public void UpdateFailTest()
    {
        var userService = CreateUserService();

        var userUpdate = new UserUpdate(1, "Janusz", "Kowal", "januszkowal@yahoo.de");
        var task = userService.UpdateUserAsync(userUpdate);

        Assert.Throws<AggregateException>(task.Wait);
        Assert.That(task.Exception?.InnerException, Is.InstanceOf<UserNotFoundException>());
    }

    [Test]
    public void DeleteSuccessTest()
    {
        var userService = CreateUserService();
        userRepository.Models = [
            new UserModel()
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jankowalski@gmail.com",
            }
        ];

        userService.DeleteUserAsync(1).Wait();

        var operation = userRepository.Operations.Single(operation => operation.OperationType == RepositoryOperationType.Delete);
        var applicationUser = (UserModel)operation.Arguments.First();

        Assert.That(applicationUser.Id, Is.EqualTo(1));
        Assert.That(taskService.Operations.Count(operation => operation.OperationType == TaskServiceOperationType.DeleteUser), Is.GreaterThan(0));
        Assert.That(userRepository.Operations.Last().OperationType, Is.EqualTo(RepositoryOperationType.SaveChanges));
    }

    [Test]
    public void DeleteFailTest()
    {
        var userService = CreateUserService();
        var task = userService.DeleteUserAsync(1);
        
        Assert.Throws<AggregateException>(task.Wait);
        Assert.That(task.Exception?.InnerException, Is.InstanceOf<UserNotFoundException>());
    }

    [Test]
    public void GetByIdSuccessTest()
    {
        var userService = CreateUserService();
        userRepository.Models = [
            new UserModel()
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jankowalski@gmail.com",
            }
        ];

        userService.GetUserById(1).Wait();

        var operation = userRepository.Operations.Single(operation => operation.OperationType == RepositoryOperationType.GetById);
        Assert.That((int)operation.Arguments.First(), Is.EqualTo(1));
    }

    [Test]
    public void GetByIdFailTest()
    {
        var userService = CreateUserService();
        var task = userService.GetUserById(1);

        Assert.Throws<AggregateException>(task.Wait);
        Assert.That(task.Exception?.InnerException, Is.InstanceOf<UserNotFoundException>());
    }

    [Test]
    public void GetTest()
    {
        var userService = CreateUserService();
        List<UserModel> data = [
            new UserModel()
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jankowalski@gmail.com",
            },
            new UserModel()
            {
                Id = 2,
                FirstName = "Adam",
                LastName = "Nowak",
                Email = "adamnowak@gmail.com",
            }
        ];

        foreach (var user in data)
            userRepository.Models.Add(new UserModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            });

        var task = userService.GetUsersAsync();
        task.Wait();
        var users = task.Result;

        foreach (var user in data)
            Assert.That(users.Count(u =>
                user.Id == u.Id && user.Email == u.Email && 
                user.FirstName == u.FirstName && user.LastName == u.LastName
            ), Is.EqualTo(1));
    }

    UserService CreateUserService()
    {
        userRepository = new();
        taskService = new();

        return new(
            userRepository, taskService, mapper,
            new UserCreateValidator(), new UserUpdateValidator()
        );
    }
}