using System;
using System.Diagnostics.Metrics;

namespace Q1Lab4 
{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<Country> countries = Country.GetCountries();

            // Invokes methods
            Question1_1();
            Question1_2();
            Question1_3();
            Question1_4();
            Question1_5();
            Question1_6();


            // 1.1 List the names of the countries in alphabetical order [0.5 mark]
            void Question1_1()
            {

                //var sortCounrty =
                //    from country in countries
                //    orderby country.Name
                //    select country.Name;

                //foreach(var name in sortCounrty)
                //{
                //    Console.WriteLine($"{name}");
                //}

                // Sorting countries by name
                List<string> sortedCountryNames = countries.Select(c => c.Name).OrderBy(name => name).ToList();

                // Displaying the sorted country names
                Console.WriteLine("Countries in Alphabetical Order:");
                foreach (var countryName in sortedCountryNames)
                {
                    Console.WriteLine(countryName);
                }
                Console.WriteLine();
            }

                // 1.2 List the names of the countries in descending order of number of resources [0.5 mark]
            void Question1_2()
            {


                var sortedCountriesQuery =
                from country in countries
                orderby country.Resources.Count descending
                select country;

                // Displaying the sorted country names
                Console.WriteLine("Countries in Descending Order of Resources:");
                foreach (var country in sortedCountriesQuery)
                {
                    Console.WriteLine($"{country.Name} - {country.Resources.Count} resources");
                }
                Console.WriteLine();


            }

                // 1.3 List the names of the countries that shares a border with Argentina [0.5 mark]
            void Question1_3()
            {
                List<Country> countries = Country.GetCountries();

                // Find the country object for Argentina
                var argentinaQuery =
                    from country in countries
                    where country.Name == "Argentina"
                    select country;

                // Check if Argentina is found
                if (argentinaQuery.Any())
                {
                    // Get the names of countries that share a border with Argentina
                    var argentina = argentinaQuery.First();
                    var borderingCountries = argentina.Borders;

                    // Displaying the names of countries that share a border with Argentina
                    Console.WriteLine("Countries that share a border with Argentina:");
                    foreach (var countryName in borderingCountries)
                    {
                        Console.WriteLine(countryName);
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Argentina not found in the list of countries.");
                }

            }
            

                

            // 1.4 List the names of the countries that has more than 10,000,000 population [0.5 mark]
            void Question1_4()
            {
                Console.WriteLine("Names of the countries that has more than 10,000,000 population");

                var countryName =
                    from country in countries
                    where country.Population > 10000000
                    select country.Name;

                foreach(var country in countryName)
                {
                    Console.WriteLine(country);
                }
                Console.WriteLine();

            }

            // 1.5 List the country with highest population [1 mark]
            void Question1_5()
            {

                Console.WriteLine("Country with highest population");
                var countryName =
                    from country in countries
                    orderby country.Population descending
                select country;

                var highestCountry = countryName.First();
                Console.WriteLine(highestCountry.Name);

                Console.WriteLine();
                

            }

            // 1.6 List all the religion in south America in dictionary order [1 mark]
            void Question1_6()
            {
                List<Country> countries = Country.GetCountries();

                // Extracting all religions in South America
                var southAmericanReligions =
                    (from country in countries
                     where country.Borders.Any(b => b.Equals("Brazil") || b.Equals("Argentina") || b.Equals("Bolivia") || b.Equals("Chile") || b.Equals("Colombia") || b.Equals("Ecuador") || b.Equals("Guyana") || b.Equals("Paraguay") || b.Equals("Peru") || b.Equals("Suriname") || b.Equals("Uruguay") || b.Equals("Venezuela"))
                     from religion in country.Religions
                     select religion)
                    .Distinct()
                    .OrderBy(religion => religion)
                    .ToList();

                // Displaying the religions in dictionary order
                Console.WriteLine("Religions in South America in Dictionary Order:");
                foreach (var religion in southAmericanReligions)
                {
                    Console.WriteLine(religion);
                }
                Console.WriteLine();

            }

            Console.ReadKey();
        }
    }
}
