using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RepositoryPattern;
using RepositoryPattern.Data;
using RepositoryPattern.Models;
using X.PagedList;

namespace NewCore3xMVC.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly RepositoryContext _context;
        private IRepositoryWrapper _repositoryWrapper;

        public EmployeesController(RepositoryContext context, IRepositoryWrapper repository)
        {
            _context = context;
            _repositoryWrapper = repository;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        public IActionResult Search()
        {
            return View(new List<Employee>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(IFormCollection form)
        {
            var fieldName = form["FieldName"].ToString();
            var keyword = form["Keyword"].ToString();

            IList<Employee> employees = new List<Employee>();
            switch (fieldName)
            {
                case "ID":
                    var id = int.Parse(keyword);
                    employees = _context.Employees.Where(d => d.ID.Equals(id)).ToList();
                    break;
                case "Name":
                    employees = _context.Employees.Where(d => d.FullName.StartsWith(keyword)).OrderBy(d => d.FullName).ToList();
                    break;
                case "Age":
                    var age = int.Parse(keyword);
                    employees = _context.Employees.Where(d => d.Age.Equals(age)).ToList();
                    break;
                case "DOJ":
                    var doj = DateTime.Parse(keyword);
                    employees = _context.Employees.Where(d => d.DOJ.Equals(doj)).ToList();
                    break;
            }

            return View(employees);
        }

        public async Task<IActionResult> Paging(int? page, int? pagesize)
        {
            if (!page.HasValue)
            {
                page = 1;
            }

            if (!pagesize.HasValue)
            {
                pagesize = 5;
            }

            var data = await _context.Employees.ToPagedListAsync(page.Value, pagesize.Value);
            return View(data);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var employee = await _context.Employees.FirstOrDefaultAsync(m => m.ID == id);
            var employee = _repositoryWrapper.Employees.Find(id.Value);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FullName,Age,DOJ,FileName")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FullName,Age,DOJ,FileName")] Employee employee)
        {
            if (id != employee.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.ID))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.ID == id);
        }
    }
}
