using System.Collections.Generic;
using System.Web.Http;

namespace Web.Areas.Api.v2
{

    [Route("api/v2/index")]
    public class IndexController : ApiController
    {


        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        /// 
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

    }
}
