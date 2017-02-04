using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Web.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        [TestMethod()]
        public void IndexTest()
        {
            var controller = new HomeController();
            var result = controller.Index() as RedirectToRouteResult;
            Assert.IsNotNull(result);
        }
    }
}