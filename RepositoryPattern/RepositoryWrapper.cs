using RepositoryPattern.Data;
using RepositoryPattern.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryPattern
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        public IPeopleRepository _people;
        public IEmployeeRepository _employee;

        public RepositoryContext _repositoryContext;

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IPeopleRepository People
        {
            get
            {
                if (_people == null)
                {
                    _people = new PeopleRepository(_repositoryContext);
                }
                return _people;
            }
        }
        

        public IEmployeeRepository Employees
        {
            get
            {
                if (_employee == null)
                {
                    _employee = new EmployeeRepository(_repositoryContext);
                }
                return _employee;
            }
        }

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }
    }
}
