using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;

namespace RelatedCompanies
{
    public class Company
    {
        public string Name { get; set; }

        public ICollection<Company> Parents { get; set; } = new List<Company>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<Company> companies = SeedData();
            Company selectedCompany = companies.First(c => c.Name == "A");
            Company searchedCompany = companies.First(c => c.Name == "E");
            var visited = new HashSet<string>();
            bool result = HaveReference(selectedCompany, searchedCompany, visited) ? true : HaveReference(searchedCompany, selectedCompany, visited);
            Console.WriteLine(result ? "Yes" : "No");
        }

        static bool HaveReference(Company childCompany, Company parentCompany, HashSet<string> visited)
        {
            var queue = new Queue<Company>();
            queue.Enqueue(childCompany);
            while (queue.Any())
            {
                var comp = queue.Dequeue();
                if (comp.Name == parentCompany.Name)
                    return true;

                if (visited.Contains(comp.Name))
                    continue;
                else
                    visited.Add(comp.Name);

                foreach (var parent in comp.Parents)
                    queue.Enqueue(parent);
            }

            return false;
        }

        static IEnumerable<Company> SeedData()
        {
            var cA = new Company() { Name = "A" };
            var cB = new Company() { Name = "B" };
            var cC = new Company() { Name = "C" };
            var cD = new Company() { Name = "D" };
            var cE = new Company() { Name = "E" };

            var companies = new List<Company>() { cA, cB, cC, cD, cE };

            cC.Parents.Add(cB); // B -> C
            cB.Parents.Add(cA); // A -> B
            cA.Parents.Add(cC); // C -> A
            cE.Parents.Add(cA); // A -> E
            
            return companies;
        }
    }
}
