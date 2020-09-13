using System.Linq;
using DownTimeAlerter.Notification;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceWorkerCronJobDemo.Controllers;
using ServiceWorkerCronJobDemo.Services;

namespace DownTimeAlerter.Tests
{
    [TestClass]
    public class DownTimeAlerter : TestBase
    {
        [TestMethod]
        public void Create_ShouldNotValidate_WhenCreateWorkerModelIsNull(){
            //Arrange¬
            var workerMock = new Mock<IWorkerService>();
            workerMock.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<WorkerDto>()));

            var controller = new DownTimeAlerterController(workerMock.Object);
            controller.ModelState.AddModelError("test", "test error");

            //Act
            controller.Create(new DownTimeAlerterViewModel());

            //Assert
            workerMock.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<WorkerDto>()), Times.Never);
            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [TestMethod]
        public void Create_ShouldValidate_WhenCreateWorker(){
            //Arrange
            var provider = base.BuildServiceProvider();
            var workerService = provider.GetService<IWorkerService>();
            var controller = new DownTimeAlerterController(workerService);

            var model = new DownTimeAlerterViewModel()
            {
                Name = "test",
                Email = "test@test.com",
                Interval = 1000,
                Url = "google.com",
                NotificationType = (int) NotificationType.Email
            };

            //Act
            controller.Create(model);

            //Assert
            Assert.AreEqual(1, workerService.List().Count());
        }

        [TestMethod]
        public void Update_ShouldNotValidate_WhenWorkerNameIsNull(){
            //Arrange
            var workerMock = new Mock<IWorkerService>();
            workerMock.Setup(x => x.Update(It.IsAny<string>(), It.IsAny<WorkerDto>()));
            var controller = new DownTimeAlerterController(workerMock.Object);
            controller.ModelState.AddModelError("test", "test error");

            //Act
            controller.Update(It.IsAny<string>(), new DownTimeAlerterViewModel());

            //Assert
            workerMock.Verify(x => x.Update(It.IsAny<string>(), It.IsAny<WorkerDto>()), Times.Never);
            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [TestMethod]
        public void Update_ShouldValidate_WhenUpdateWorker(){
            //Arrange
            var provider = base.BuildServiceProvider();
            var workerService = provider.GetService<IWorkerService>();
            var controller = new DownTimeAlerterController(workerService);

            var createModel = new DownTimeAlerterViewModel()
            {
                Name = "test",
                Email = "test@test.com",
                Interval = 1000,
                Url = "google.com",
                NotificationType = (int) NotificationType.Email
            };

            var updateModel = new DownTimeAlerterViewModel()
            {
                Name = "test2",
                Email = "test@test.com",
                Interval = 2000,
                Url = "google.com",
                NotificationType = (int) NotificationType.Email
            };

            //Act
            controller.Create(createModel);
            controller.Update(createModel.Name, updateModel);

            //Assert
            Assert.AreEqual(1, workerService.List().Count());
            Assert.AreEqual(updateModel.Name, workerService.Get(updateModel.Name).Name);
        }

        [TestMethod]
        public void Delete_ShouldNotValidate_WhenWorkerNameIsNull(){
            //Arrange
            var workerMock = new Mock<IWorkerService>();
            workerMock.Setup(x => x.Remove(It.IsAny<string>()));
            var controller = new DownTimeAlerterController(workerMock.Object);

            //Act
            controller.Delete(It.IsAny<string>());

            //Assert
            workerMock.Verify(x => x.Remove(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Delete_ShouldValidate_WhenDeleteWorker(){
            //Arrange
            var provider = base.BuildServiceProvider();
            var workerService = provider.GetService<IWorkerService>();
            var controller = new DownTimeAlerterController(workerService);

            var createModel = new DownTimeAlerterViewModel()
            {
                Name = "test",
                Email = "test@test.com",
                Interval = 1000,
                Url = "google.com",
                NotificationType = (int) NotificationType.Email
            };


            //Act
            controller.Create(createModel);
            controller.Delete(createModel.Name);

            //Assert
            Assert.IsNull(workerService.Get(createModel.Name));
        }
    }

    public class TestBase
    {
        protected ServiceProvider BuildServiceProvider(){
            var services = new ServiceCollection();
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddScoped<IScheduler, Scheduler>();
            services.AddSingleton<IWorkerService, WorkerService>();
            services.AddScoped<IUrlStatusChecker, UrlStatusChecker>();
            services.AddScoped<NotificationFactory>();
            services.AddHttpClient();

            return services.BuildServiceProvider();
        }
    }
}