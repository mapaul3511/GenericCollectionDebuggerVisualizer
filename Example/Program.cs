using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Dictionary<Department, List<Employee>>> dicCompany = 
                new Dictionary<string, Dictionary<Department, List<Employee>>>();

            List<Employee> lstEmployees = new List<Employee>()
            {
                new Employee()
                {
                    Id = "S001",
                    Name = "Paul",
                    Age = 42,
                    Gender = Gender.Male
                },
                new Employee()
                {
                    Id = "S002",
                    Name = "Kevin",
                    Age = 35,
                    Gender = Gender.Male
                },
                new Employee()
                {
                    Id = "S003",
                    Name = "Sally",
                    Age = 45,
                    Gender = Gender.Female
                }
            };

            Dictionary<Department, List<Employee>> dicDept = new Dictionary<Department, List<Employee>>()
            {
                {
                    new Department()
                    {
                        Id = "DEPT1",
                        Name = "Sales",
                        CreateDate = new DateTime(2018, 2, 19),
                        MemberCount = 3
                    },
                    lstEmployees
                }
            };

            dicCompany.Add("Sales", dicDept);

            lstEmployees = new List<Employee>()
            {
                new Employee()
                {
                    Id = "R001",
                    Name = "John",
                    Age = 40,
                    Gender = Gender.Male
                },
                new Employee()
                {
                    Id = "R002",
                    Name = "AMy",
                    Age = 35,
                    Gender = Gender.Female
                },
                new Employee()
                {
                    Id = "R003",
                    Name = "Jack",
                    Age = 28,
                    Gender = Gender.Male
                }
            };

            dicDept = new Dictionary<Department, List<Employee>>()
            {
                {
                    new Department()
                    {
                        Id = "DEPT2",
                        Name = "Development",
                        CreateDate = new DateTime(2018, 2, 19),
                        MemberCount = 3
                    },
                    lstEmployees
                }
            };

            dicCompany.Add("Development", dicDept);

            Console.WriteLine(dicCompany);
        }
    }

    [Serializable]
    class Department
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public int MemberCount { get; set; }
    }

    [Serializable]
    class Employee
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
    }

    [Serializable]
    enum Gender
    {
        Male,
        Female
    }
}
