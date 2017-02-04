using System.Threading.Tasks;
using System.Web.Mvc;
using NUnit.Framework;
using System;

namespace Web.Controllers.Tests
{
    [TestFixture]
    public class AccountControllerTests
    {
        [OneTimeSetUp]
        public void Initialize()
        {
            Console.WriteLine("初始化信息");
           
        }


        [Test]
        public async Task LoginTestAsync()
        {


           // var action = await new AccountController().Login(new Models.LoginViewModel() { UserName = "TestUser", Password = "test@123" }, null);



        }

        [Test]
        public void LoginTest()
        {
            var controller = new AccountController();
            var result = controller.Login(null) as ViewResult;
            Assert.IsNotNull(result);
        }
     


    }
}