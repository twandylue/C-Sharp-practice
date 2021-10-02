using System.Collections.Generic;
using System;
using WebAPI.Models;

namespace WebAPI.EmployeeData
{
    public interface IEmployeeData
    {
        List<Employee> GetEmployees();
        Employee GetEmployee(Guid id);
        Employee AddEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
        Employee EditEmployee(Employee employee);
    }
}