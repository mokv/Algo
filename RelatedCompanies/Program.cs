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

        public ICollection<Company> Related { get; set; } = new List<Company>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<Company> companies = SeedData();
            Company selectedCompany = companies.First(c => c.Name == "B");
            Company searchedCompany = companies.First(c => c.Name == "E");
            bool result = HaveReference(selectedCompany, searchedCompany);
            bool resultRecursive = HaveReferenceRecursive(selectedCompany, searchedCompany, new HashSet<string>(), new Queue<Company>());
            Console.WriteLine(result ? "Yes" : "No");
            Console.WriteLine(resultRecursive ? "Yes" : "No");
        }

        static bool HaveReferenceRecursive(Company startCompany, Company searchedCompany, HashSet<string> visited, Queue<Company> queue)
        {
            if (startCompany.Name == searchedCompany.Name)
                return true;

            if (!visited.Contains(startCompany.Name))
            {
                visited.Add(startCompany.Name);
                queue.Enqueue(startCompany);

                foreach (var company in startCompany.Related)
                {
                    if (HaveReferenceRecursive(company, searchedCompany, visited, queue))
                        return true;
                }
                queue.Dequeue();
            }
            
            return false;
        }

        static bool HaveReference(Company startCompany, Company searchedCompany)
        {
            var visited = new HashSet<string>();
            var queue = new Queue<Company>();
            queue.Enqueue(startCompany);
            while (queue.Any())
            {
                var comp = queue.Dequeue();
                if (comp.Name == searchedCompany.Name)
                    return true;

                if (visited.Contains(comp.Name))
                    continue;
                else
                    visited.Add(comp.Name);

                foreach (var parent in comp.Related)
                    queue.Enqueue(parent);
            }

            return false;
        }

        static IEnumerable<Company> ReadCompanies()
        {
            var companies = new List<Company>();

            string input;
            do
            {
                input = Console.ReadLine();
                companies.Add(new Company() { Name = input });
            }
            while (!string.IsNullOrWhiteSpace(input));

            return companies;
        }

        static void ReadCompaniesConnection(IEnumerable<Company> companies)
        {
            string input;
            do
            {
                input = Console.ReadLine();
                string[] connection = input.Split(':');
                Company compA = companies.First(c => c.Name == connection[0]);
                Company compB = companies.First(c => c.Name == connection[1]);
                compA.Related.Add(compB);
                compB.Related.Add(compA);
            }
            while (!string.IsNullOrWhiteSpace(input));
        }

        static IEnumerable<Company> SeedData()
        {
            var cA = new Company() { Name = "A" };
            var cB = new Company() { Name = "B" };
            var cC = new Company() { Name = "C" };
            var cD = new Company() { Name = "D" };
            var cE = new Company() { Name = "E" };

            var companies = new List<Company>() { cA, cB, cC, cD, cE };

            cC.Related.Add(cB); // B <-> C
            cB.Related.Add(cC);

            cB.Related.Add(cA); // A <-> B
            cA.Related.Add(cB);

            cA.Related.Add(cC); // C <-> A
            cC.Related.Add(cA);

            cE.Related.Add(cA); // A <-> E
            cA.Related.Add(cE);
            
            return companies;
        }
    }
}
