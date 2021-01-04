using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using NewCore3xMVC.Data;
using NewCore3xMVC.Models;
using NewCore3xMVC.Models.Views;
using Newtonsoft.Json;

namespace NewCore3xMVC.Controllers
{
    public class PeopleController : Controller
    {
        private readonly NewCore3xMVCContext _context;
        private IMemoryCache _memoryCache;
        private IDistributedCache _distributedCache;

        public PeopleController(NewCore3xMVCContext context, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
            _context = context;

        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            return View(await _context.Person.Include(d => d.Address).ToListAsync());
        }

        public IActionResult RawQuery(int id)
        {
            var sql = "SELECT * FROM People WHERE AutoID = {0}";

            var data = _context.Person.FromSqlRaw(sql, id).FirstOrDefault();

            return View(data);
        }


        public IActionResult RawQueryComplex(int id)
        {
            IList<PersonAddressView> list = new List<PersonAddressView>();
            using (var conn = _context.Database.GetDbConnection())
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    var sql = "SELECT P.AutoId, P.FirstName, P.LastName, P.Age, P.Active, P.AddressId, A.AddressLine, A.City, A.Pin FROM People P, Addresses A " +
                        " WHERE P.AddressId = A.AddressId";
                    command.CommandText = sql;
                    using(var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var record = new PersonAddressView()
                                {
                                    Active = reader.GetBoolean(4),
                                    AddressId = reader.GetInt32(5),
                                    AddressLine = reader.GetString(6),
                                    Age = reader.GetInt32(3),
                                    AutoId = reader.GetInt32(0),
                                    City = reader.GetString(7),
                                    FirstName = reader.GetString(1),
                                    LastName = reader.GetString(2),
                                    Pin = reader.GetString(8)
                                };
                                list.Add(record);
                            }
                        }
                    } // reader
                } // command
                conn.Close();
            } // connection

            return View(list);
        }


        #region InMemory Caching options

        public async Task<IActionResult> CacheIndex()
        {
            var cacheData = _memoryCache.Get("PeopleData");

            IList<Person> data = new List<Person>();

            if (cacheData == null)
            {
                data = await _context.Person.ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));

                _memoryCache.Set("PeopleData", data, cacheOptions);
            }
            else
            {
                data = (IList<Person>)cacheData;
            }
            return View(data);
        }

        public IActionResult RemoveCache()
        {
            _memoryCache.Remove("PeopleData");

            return RedirectToAction("CacheIndex");
        }

        #endregion InMemory Caching options

        #region Distributed Cache

        public async Task<IActionResult> RedisCache()
        {
            IList<Person> data = new List<Person>();

            var isCacheString = _distributedCache.GetString("RedisData");
            if (string.IsNullOrEmpty(isCacheString))
            {
                data = await _context.Person.ToListAsync();

                // convert collection into serialized stringn
                var dataString = JsonConvert.SerializeObject(data);

                var distributedCacheOptions = new DistributedCacheEntryOptions();
                distributedCacheOptions.SetSlidingExpiration(TimeSpan.FromSeconds(5000));

                await _distributedCache.SetStringAsync("RedisData", dataString, distributedCacheOptions);
            }
            else
            {

                data = JsonConvert.DeserializeObject<IList<Person>>(isCacheString);
            }

            return View(data);
        }

        public IActionResult RemoveRedisCache()
        {
            _distributedCache.Remove("RedisData");

            return RedirectToAction("RedisCache");
        }



        #endregion

        #region Response Caching Options
        // TRY uncommenting ResponseCache related code from ConfigureService and Configure method from Startup.cs and 
        // run RemoveRedisCache method and you won't see data getting removed or break point hitting.

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [ResponseCache(Duration = 30000)]
        public async Task<IActionResult> ResponseCaching()
        {
            return View("Index", await _context.Person.ToListAsync());
        }

        #endregion Response Caching Options



        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.AutoId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutoId,FirstName,LastName,Age,Active")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AutoId,FirstName,LastName,Age,Active")] Person person)
        {
            if (id != person.AutoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.AutoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        public async Task<ActionResult> Search()
        {
            return View(new List<Person>());
        }

        [HttpPost]
        public async Task<ActionResult> Search(IFormCollection form)
        {
            var keyword = form["keyword"];
            var data = await _context.Person.Where(d => d.FirstName.Contains(keyword) || d.LastName.Contains(keyword)).ToListAsync();
            ViewBag.Keyword = keyword;
            return View(data);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.AutoId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Person.FindAsync(id);
            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.AutoId == id);
        }
    }
}
