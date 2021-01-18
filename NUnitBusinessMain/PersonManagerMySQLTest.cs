using Business.Main.DbContextMySQL;
using Business.Main.ModuloMySQL;
using Domain.Main.Wraper;
using NUnit.Framework;

namespace NUnitBusinessMain
{
    public class PersonManagerMySQLTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PersonRegisterTest()
        {
            PersonManager personManager = new PersonManager();
            Person person = new Person { Idperson = 0, Name = "Dario", Lastname = "Chalco" };
            var resul = personManager.RegisterPerson(person);
            Assert.AreEqual(resul.State, ResponseType.Success);
        }

        [Test]
        public void PersonRemoveTest()
        {
            PersonManager personManager = new PersonManager();
            Person person = new Person { Idperson = 2};
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