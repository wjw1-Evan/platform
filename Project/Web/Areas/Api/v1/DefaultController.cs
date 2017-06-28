using System.Collections.Generic;
using System.Web.Http;

namespace Web.Areas.Api.v1
{
    /// <summary>
    /// 默认
    /// </summary>
    [Route("api/v1/default")]
    public class DefaultController : ApiController
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">记录编号</param>
        /// <returns></returns>
        // GET: api/Default/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Default
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Default/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Default/5
        public void Delete(int id)
        {
        }
    }
}
