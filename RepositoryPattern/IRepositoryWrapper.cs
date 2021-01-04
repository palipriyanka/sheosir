using RepositoryPattern.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryPattern
{
    public interface IRepositoryWrapper
    {
        IPeopleRepository People { get; }

        IEmployeeRepository Employees { get; }

        void Save();
    }
}
