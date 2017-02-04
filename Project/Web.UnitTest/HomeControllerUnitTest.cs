using System;

using NUnit.Framework;
using Web.Controllers;
using System.Web.Mvc;

namespace Web.UnitTest
{
    [TestFixture]
    public class HomeControllerUnitTest
    {

        // usage NUnit attributes
        // 标识测试类   TestFixture
        // 标识测试用例（TestCase）	Test
        // 标识测试类初始化函数  TestFixtureSetup
        // 标识测试类资源释放函数 TestFixtureTearDown
        // 标识测试用例初始化函数 Setup
        // 标识测试用例资源释放函数    TearDown
        // 标识测试用例说明    N / A
        // 标识忽略该测试用例 Ignore
        // 标识该用例所期望抛出的异常 ExpectedException
        // 标识测试用例是否需要显式执行 Explicit
        // 标识测试用例的分类 Category


        [Test]
        public void TestMethod1()
        {
            var homeController = new HomeController();

            var viewResult = homeController.Index() as RedirectToRouteResult;

            Assert.That(viewResult, Is.Not.Null);

            int expected = 5;

            int actual = 2 + 3;

            Assert.AreEqual(expected, actual);

        }
    }
}
