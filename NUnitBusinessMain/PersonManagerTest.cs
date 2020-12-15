using Business.Main.DbContextSample;
using Business.Main.Modulo01;
using Domain.Main.Wraper;
using NUnit.Framework;

namespace NUnitBusinessMain
{
    public class PersonManagerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PersonRegisterTest()
        {
            PersonManager personManager = new PersonManager();
            Person person = new Person { PersonId = 4, Name = "Dario", LastName = "Chalco" };
            var resul = personManager.RegisterPerson(person);
            Assert.AreEqual(resul.State, ResponseType.Success);
        }

        [Test]
        public void PersonRemoveTest()
        {
            PersonManager personManager = new PersonManager();
            Person person = new Person { PersonId = 2};
            var resul = personManager.DeletePerson(person);
            Assert.AreEqual(resul.State, ResponseType.Success);
        }

        [Test]
        public void GetPersonsTest()
        {
            PersonManager personManager = new PersonManager();
            var resul = personManager.GetPersons("ruben");
            Assert.AreEqual(resul.State, ResponseType.Success);
        }

        
    }
}