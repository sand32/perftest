using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore.Controllers
{
    [Route("")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public IActionResult Get([FromQuery]Filter filter, [FromQuery]Dictionary<string, string> search)
        {
            return Ok(Data.Collection
                .Search(search)
                .Sort(filter)
                .Paginate(filter)
                .SelectFields(filter)
                .IdToUri($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}{HttpContext.Request.Path}"));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Datum result = Data.Collection.FirstOrDefault(x => x.Id == id);
            if(result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Datum value)
        {
            value.Id = Data.NextId++;
            Data.Collection.Add(value);
            return Created($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}{HttpContext.Request.Path}/{Data.Collection.Count - 1}", value);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Datum value)
        {
            value.Id = id;
            Datum result = Data.Collection.FirstOrDefault(x => x.Id == id);
            if(result == null) return NotFound();
            result = value;
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int count = Data.Collection.RemoveAll(x => x.Id == id);
            if(count == 0) return NotFound();
            return NoContent();
        }
    }
}
