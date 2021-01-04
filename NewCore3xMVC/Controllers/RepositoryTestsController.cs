using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryPattern;
using RepositoryPattern.Models;

namespace NewCore3xMVC.Controllers
{
    public class RepositoryTestsController : Controller
    {
        private IRepositoryWrapper _repositoryWrapper;

        public RepositoryTestsController(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public IActionResult Index()
        {
            var people = _repositoryWrapper.People.FindAll();
            return View(people);
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
        public async Task<IActionResult> Create([Bind("AutoId,FirstName,LastName,Age,Active")] People people)
        {
            if (ModelState.IsValid)
            {
                _repositoryWrapper.People.Create(people);
                _repositoryWrapper.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(people);
        }


        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = _repositoryWrapper.People.FindByCondition(d => d.AutoId == id.Value).FirstOrDefault();
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
        public async Task<IActionResult> Edit(int id, [Bind("AutoId,FirstName,LastName,Age,Active")] People people)
        {
            if (id != people.AutoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repositoryWrapper.People.Update(people);


                    // find the employee records and try to update both people and employee records at a time using RepositoryPattern
                    var employee = _repositoryWrapper.Employees.FindByCondition(d => d.ID == id).FirstOrDefault();
                    if (employee != null)
                    {
                        employee.FileName = people.FirstName + "_" + people.LastName + ".jpg";
                        _repositoryWrapper.Employees.Update(employee);
                    }

                    _repositoryWrapper.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(people.AutoId))
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
            return View(people);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = _repositoryWrapper.People.FindByCondition(m => m.AutoId == id).FirstOrDefault();
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
            var person = _repositoryWrapper.People.FindByCondition(d => d.AutoId == id).FirstOrDefault();
            _repositoryWrapper.People.Delete(person);
            _repositoryWrapper.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _repositoryWrapper.People.FindByCondition(e => e.AutoId == id).FirstOrDefault() == null;
        }
    }
}