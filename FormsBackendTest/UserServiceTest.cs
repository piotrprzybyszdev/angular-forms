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
    MockUserRepository userRepository = new();
    MockTaskService taskService = new();

    [Test]
    public void CreateSuccessTest()
    {
        var userService = CreateUserService();

        var userCreate = new UserCreate("Jan", "Kowalski", "jankowalski@gmail.com", "test123");
        userService.CreateUserAsync(userCreate).Wait();

        var operation = userRepository.Operations.Single(operation => operation.OperationType == UserRepositoryOperationType.Insert);
        var applicationUser = (ApplicationUser)operation.Arguments.First();

        Assert.That(applicationUser.FirstName, Is.EqualTo(userCreate.FirstName));
        Assert.That(applicationUser.LastName, Is.EqualTo(userCreate.LastName));
        Assert.That(applicationUser.Email, Is.EqualTo(userCreate.Email));

        Assert.That(userRepository.Operations.Last().OperationType, Is.EqualTo(UserRepositoryOperationType.SaveChanges));
    }

    [Test]
    public void CreateFailTest()
    {
        var userService = CreateUserService();
        userRepository.Users = [
            new ApplicationUser()
            {
                Id = "testid",
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
        userRepository.Users = [
            new ApplicationUser()
            {
                Id = "testid",
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jankowalski@gmail.com",
            }
        ];

        var userUpdate = new UserUpdate("testid", "Janusz", "Kowal", "januszkowal@yahoo.de");
        userService.UpdateUserAsync(userUpdate).Wait();

        var operation = userRepository.Operations.Single(operation => operation.OperationType == UserRepositoryOperationType.Update);
        var applicationUser = (ApplicationUser)operation.Arguments.First();

        Assert.That(applicationUser.Id, Is.EqualTo("testid"));
        Assert.That(applicationUser.FirstName, Is.EqualTo(userUpdate.FirstName));
        Assert.That(applicationUser.LastName, Is.EqualTo(userUpdate.LastName));
        Assert.That(applicationUser.Email, Is.EqualTo(userUpdate.Email));

        Assert.That(userRepository.Operations.Last().OperationType, Is.EqualTo(UserRepositoryOperationType.SaveChanges));
    }

    [Test]
    public void UpdateFailTest()
    {
        var userService = CreateUserService();

        var userUpdate = new UserUpdate("testid", "Janusz", "Kowal", "januszkowal@yahoo.de");
        var task = userService.UpdateUserAsync(userUpdate);

        Assert.Throws<AggregateException>(task.Wait);
        Assert.That(task.Exception?.InnerException, Is.InstanceOf<UserNotFoundException>());
    }

    [Test]
    public void DeleteSuccessTest()
    {
        var userService = CreateUserService();
        userRepository.Users = [
            new ApplicationUser()
            {
                Id = "testid",
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jankowalski@gmail.com",
            }
        ];

        userService.DeleteUserAsync("testid").Wait();

        var operation = userRepository.Operations.Single(operation => operation.OperationType == UserRepositoryOperationType.Delete);
        var applicationUser = (ApplicationUser)operation.Arguments.First();

        Assert.That(applicationUser.Id, Is.EqualTo("testid"));
        Assert.That(taskService.Operations.Count(operation => operation.OperationType == TaskServiceOperationType.DeleteUser), Is.GreaterThan(0));
        Assert.That(userRepository.Operations.Last().OperationType, Is.EqualTo(UserRepositoryOperationType.SaveChanges));
    }

    [Test]
    public void DeleteFailTest()
    {
        var userService = CreateUserService();
        var task = userService.DeleteUserAsync("testid");
        
        Assert.Throws<AggregateException>(task.Wait);
        Assert.That(task.Exception?.InnerException, Is.InstanceOf<UserNotFoundException>());
    }

    [Test]
    public void GetByIdSuccessTest()
    {
        var userService = CreateUserService();
        userRepository.Users = [
            new ApplicationUser()
            {
                Id = "testid",
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jankowalski@gmail.com",
            }
        ];

        userService.GetUserById("testid").Wait();

        var operation = userRepository.Operations.Single(operation => operation.OperationType == UserRepositoryOperationType.GetById);
        Assert.That((string)operation.Arguments.First(), Is.EqualTo("testid"));
    }

    [Test]
    public void GetByIdFailTest()
    {
        var userService = CreateUserService();
        var task = userService.GetUserById("testid");

        Assert.Throws<AggregateException>(task.Wait);
        Assert.That(task.Exception?.InnerException, Is.InstanceOf<UserNotFoundException>());
    }

    [Test]
    public void GetTest()
    {
        var userService = CreateUserService();
        List<ApplicationUser> data = [
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

        foreach (var user in data)
            userRepository.Users.Add(new ApplicationUser()
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