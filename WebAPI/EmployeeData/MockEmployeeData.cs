using System.Collections.Generic;
using System;
using System.Linq;
using WebAPI.Models;

namespace WebAPI.EmployeeData
{
    public class MockEmployeeData : IEmployeeData
    {
        private List<Employee> employees = new List<Employee>()
        {
            new Employee() {
                Id = Guid.NewGuid(),
                Name = "Employee One"
            },
            new Employee() {
                Id = Guid.NewGuid(),
                Name = "Employee Two"
            }
        };

        public List<Employee> GetEmployees()
        {
            return employees;
        }
        public Employee GetEmployee(Guid id)
        {
            return employees.SingleOrDefault(x => x.Id == id);
        }
        public Employee AddEmployee(Employee employee)
        {
            employee.Id = Guid.NewGuid();
            this.employees.Add(employee);
            return employee;
        }
        public void DeleteEmployee(Employee employee)
        {
            this.employees.Remove(employee);
        }
        public Employee EditEmployee(Employee employee)
        {
            var existingEmployee = GetEmployee(employee.Id);
            existingEmployee.Name = employee.Name;
            return existingEmployee;
        }
    }
}