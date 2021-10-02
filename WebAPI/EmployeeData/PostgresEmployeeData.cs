using System.Collections.Generic;
using System;
using System.Linq;
using WebAPI.Models;
using System.Text.Json;

namespace WebAPI.EmployeeData
{
    public class PostgresEmployeeData : IEmployeeData
    {
        private EmployeeContext _employeeContext;
        public PostgresEmployeeData(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }

        public List<Employee> GetEmployees()
        {
            return this._employeeContext.Employees.ToList();
        }
        public Employee GetEmployee(Guid id)
        {
            var employee = this._employeeContext.Employees.Find(id);
            return employee;
        }
        public Employee AddEmployee(Employee employee)
        {
            employee.Id = Guid.NewGuid();
            this._employeeContext.Employees.Add(employee);
            this._employeeContext.SaveChanges();
            return employee;
        }
        public void DeleteEmployee(Employee employee)
        {
            // Console.WriteLine(JsonSerializer.Serialize(employee)); // 觀察用
            this._employeeContext.Employees.Remove(employee);
            this._employeeContext.SaveChanges();
        }
        public Employee EditEmployee(Employee employee)
        {
            var existingEmployee = this._employeeContext.Employees.Find(employee.Id);

            if (existingEmployee != null)
            {
                existingEmployee.Name = employee.Name;

                this._employeeContext.ChangeTracker.DetectChanges(); // 觀察用
                Console.WriteLine(this._employeeContext.ChangeTracker.DebugView.LongView);

                // this._employeeContext.Employees.Update(employee); // 有問題
                
                this._employeeContext.SaveChanges();
            }
            return employee;
        }
    }
}