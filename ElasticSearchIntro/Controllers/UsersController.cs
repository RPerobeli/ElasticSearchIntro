using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElasticSearchIntro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IElasticClient _elasticClient;

        public UsersController(IElasticClient elasticClient)
        {
            this._elasticClient = elasticClient;
        }
        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<User> Get(string id)
        {
            var response = await _elasticClient.SearchAsync<User>(s => s
              .Index("users")
              .Query( q=>q.Term(t=>t.Name, id) ||  (q.Match(m=>m.Field(f=>f.Name).Query(id))) )
              );
            return response?.Documents?.FirstOrDefault();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<string> Post([FromBody] User value)
        {
            var response = await _elasticClient.IndexAsync<User>(value, x => x.Index("users"));
            return response.Id;
        }
    }
}
