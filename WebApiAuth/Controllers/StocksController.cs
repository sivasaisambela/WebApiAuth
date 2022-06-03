
using WebApiAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace WebApiAuth.Controllers
{
    [Authorize]
    public class StocksController : ApiController
    {
        ApplicationDbContext stkContext = new ApplicationDbContext();
        // GET: api/Stocks

        [CacheOutput(ClientTimeSpan =60,ServerTimeSpan =100)]
        [AllowAnonymous]
        [HttpGet]
        [Route("api/stocks")]
        public IHttpActionResult Get()
        {
            // var stocks = stkContext.Stocks;
            IQueryable<Stock> stocks;
            string sort = "asc";
            switch(sort)
            {
                case "desc":
                    stocks = stkContext.Stocks.OrderByDescending(x => x.PricePerUnit);
                    break;
                case "asc":
                    stocks = stkContext.Stocks.OrderBy(x => x.PricePerUnit);
                    break;

                default:
                    stocks= stkContext.Stocks;
                    break;
            }
            return Ok(stocks);
        }

        [HttpGet]
        [Route("api/Stocks/Paging/{pageNumber=1}/{pageSize=2}")]
        public IHttpActionResult Paging(int pageNumber,int pageSize)
        {
            var Stks = stkContext.Stocks.OrderBy(x => x.Name);
            return Ok(Stks.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }
        // GET: api/Stocks/5

         
        [HttpGet]
        [Route("api/Stocks/{Id}")]
        public IHttpActionResult Get(int id)
        {
            var st = stkContext.Stocks.Find(id);
            if (st == null)
            {
                return NotFound();
            }
            return Ok(st);
        }


        [HttpGet]
        [Route("api/Stocks/SearchStock/{name=}")]
        public IHttpActionResult SearchStock(string name)
        {
            var stocks = stkContext.Stocks.Where(x => x.Name.StartsWith(name));
            return Ok(stocks);
        }
        // POST: api/Stocks


         [HttpPost]
        [Route("api/Stocks")]
        public IHttpActionResult Post([FromBody]Stock stk)
        {
            if(ModelState.IsValid)
            {
                stkContext.Stocks.Add(stk);
                stkContext.SaveChanges();
                return StatusCode(HttpStatusCode.Created);
            }
            return BadRequest(ModelState);
           
        }

        [HttpPut]
        // PUT: api/Stocks/5
        public IHttpActionResult Put(int id, [FromBody]Stock stk)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = stkContext.Stocks.FirstOrDefault(x => x.Id == id);
            if (entity == null)
            {
                return BadRequest("No record found against the Id..");
            }
            entity.Name = stk.Name;
            entity.OpenPrice = stk.OpenPrice;
            entity.High = stk.High;
            entity.Low = stk.Low;
            entity.PricePerUnit = stk.PricePerUnit;
            stkContext.SaveChanges();
            return Ok("Record updated Successfully...");
        }

        [HttpDelete]
        // DELETE: api/Stocks/5
        public IHttpActionResult Delete(int id)
        {
            var entity = stkContext.Stocks.Find(id);
            if(entity==null)
            {
                return BadRequest("No record found against the Id..");
            }
            stkContext.Stocks.Remove(entity);
            stkContext.SaveChanges();
            return Ok("Record deleted Successfully...");
        }
    }
}
