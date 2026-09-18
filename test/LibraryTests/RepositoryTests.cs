using System;
using System.IO;
using NUnit.Framework;

namespace Ucu.Poo.Repositories.Tests
{
    [TestFixture]
    public class RepositoryTests
    {
        private Repository<Movie> catalog;
        private Repository<Car> database;

        [SetUp]
        public void SetUp()
        {
            this.catalog = new Repository<Movie>();
            this.database = new Repository<Car>();
        }

        [Test]
        public void Add_ValidMovie_MovieIsFound()
        {
            Movie movie = new Movie("Inception", 2010);

            this.catalog.Add(movie);

            Movie found = this.catalog.Find("Name", "Inception");
            Assert.That(found, Is.SameAs(movie));
        }

        [Test]
        public void Add_NullMovie_MovieIsNotAdded()
        {
            this.catalog.Add(null);

            Movie found = this.catalog.Find("Name", "Inception");
            Assert.That(found, Is.Null);
        }

        [Test]
        public void Remove_ExistingMovie_MovieIsNoLongerFound()
        {
            Movie movie = new Movie("The Matrix", 1999);
            this.catalog.Add(movie);

            this.catalog.Remove(movie);

            Movie found = this.catalog.Find("Name", "The Matrix");
            Assert.That(found, Is.Null);
        }

        [Test]
        public void Find_MatchingCriteria_ReturnsMovie()
        {
            Movie movie = new Movie("Interstellar", 2014);
            this.catalog.Add(movie);

            Movie found = this.catalog.Find("Year", "2014");

            Assert.That(found, Is.SameAs(movie));
        }

        [Test]
        public void Find_NoMatchingCriteria_ReturnsNull()
        {
            Movie movie = new Movie("Dunkirk", 2017);
            this.catalog.Add(movie);

            Movie found = this.catalog.Find("Name", "Tenet");

            Assert.That(found, Is.Null);
        }

        [Test]
        public void Find_EmptyCatalog_ReturnsNull()
        {
            Movie found = this.catalog.Find("Name", "Inception");

            Assert.That(found, Is.Null);
        }

        [Test]
        public void ConvertToJson_CatalogWithMovie_ReturnsMovieAsJson()
        {
            this.catalog.Add(new Movie("Inception", 2010));

            string json = this.catalog.ConvertToJson();

            Assert.That(json, Is.EqualTo("[{\"Name\":\"Inception\",\"Year\":2010}]"));
        }

        [Test]
        public void LoadFromJson_ValidJson_MovieIsLoaded()
        {
            this.catalog.LoadFromJson("[{\"Name\":\"The Matrix\",\"Year\":1999}]");

            Movie found = this.catalog.Find("Name", "The Matrix");

            Assert.That(found, Is.Not.Null);
            Assert.That(found.Year, Is.EqualTo(1999));
        }

        [Test]
        public void SaveToFile_CatalogWithMovie_FileContainsCatalogAsJson()
        {
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
            this.catalog.Add(new Movie("Interstellar", 2014));

            try
            {
                this.catalog.SaveToFile(filePath);

                Assert.That(File.ReadAllText(filePath), Is.EqualTo("[{\"Name\":\"Interstellar\",\"Year\":2014}]"));
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [Test]
        public void LoadFromFile_ExistingFile_MovieIsLoaded()
        {
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
            File.WriteAllText(filePath, "[{\"Name\":\"Dunkirk\",\"Year\":2017}]");

            try
            {
                bool loaded = this.catalog.LoadFromFile(filePath);

                Assert.That(loaded, Is.True);
                Assert.That(this.catalog.Find("Name", "Dunkirk"), Is.Not.Null);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [Test]
        public void LoadFromFile_NonExistingFile_ReturnsFalse()
        {
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");

            bool loadedMovies = this.catalog.LoadFromFile(filePath);

            bool loadedCars = this.database.LoadFromFile(filePath);

            Assert.That(loadedMovies, Is.False);

            Assert.That(loadedCars, Is.False);
        }
        [Test]
        public void AddCar_ValidCar_CarIsFound()
        {
            Car car = new Car("Jimny", "Suzuki", 2024);

            this.database.Add(car);

            Car found = this.database.Find("Model", "Jimny");
            Assert.That(found, Is.SameAs(car));
        }

        [Test]
        public void AddCar_NullCar_CarIsNotAdded()
        {
            this.database.Add(null);

            Car found = this.database.Find("Model", "Jimny");
            Assert.That(found, Is.Null);
        }

        [Test]
        public void RemoveCar_ExistingCar_CarIsNoLongerFound()
        {
            Car car = new Car("Focus", "Ford", 2018);
            this.database.Add(car);

            this.database.Remove(car);

            Car found = this.database.Find("Maker", "Ford");
            Assert.That(found, Is.Null);
        }

        [Test]
        public void FindCar_MatchingCriteria_ReturnsCar()
        {
            Car car = new Car("Onix", "Chevrolet", 2022);
            this.database.Add(car);

            Car found = this.database.Find("Year", "2022");

            Assert.That(found, Is.SameAs(car));
        }

        [Test]
        public void FindCar_NoMatchingCriteria_ReturnsNull()
        {
            Car car = new Car("Sandero", "Renault", 2015);
            this.database.Add(car);

            Car found = this.database.Find("Maker", "Ford");

            Assert.That(found, Is.Null);
        }

        [Test]
        public void FindCar_EmptyDatabase_ReturnsNull()
        {
            Car found = this.database.Find("Model", "Jimny");

            Assert.That(found, Is.Null);
        }

        [Test]
        public void ConvertToJson_DatabaseWithCar_ReturnsCarAsJson()
        {
            this.database.Add(new Car("Jimny", "Suzuki", 2024));

            string json = this.database.ConvertToJson();

            Assert.That(json, Is.EqualTo("[{\"Model\":\"Jimny\",\"Maker\":\"Suzuki\",\"Year\":2024}]"));
        }

        [Test]
        public void LoadFromJson_ValidJson_CarIsLoaded()
        {
            this.database.LoadFromJson("[{\"Model\":\"Focus\",\"Maker\":\"Ford\",\"Year\":2018}]");

            Car found = this.database.Find("Model", "Focus");

            Assert.That(found, Is.Not.Null);
            Assert.That(found.Maker, Is.EqualTo("Ford"));
        }

        [Test]
        public void SaveToFile_DatabaseWithCar_FileContainsDatabaseAsJson()
        {
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
            this.database.Add(new Car("Onix", "Chevrolet", 2022));

            try
            {
                this.database.SaveToFile(filePath);

                Assert.That(File.ReadAllText(filePath), Is.EqualTo("[{\"Model\":\"Onix\",\"Maker\":\"Chevrolet\",\"Year\":2022}]"));
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [Test]
        public void LoadFromFile_ExistingFile_CarIsLoaded()
        {
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
            File.WriteAllText(filePath, "[{\"Model\":\"Sandero\",\"Maker\":\"Renault\",\"Year\":2015}]");

            try
            {
                bool loaded = this.database.LoadFromFile(filePath);

                Assert.That(loaded, Is.True);
                Assert.That(this.database.Find("Model", "Sandero"), Is.Not.Null);
            }
            finally
            {
                File.Delete(filePath);
            }
        }
    }
}
