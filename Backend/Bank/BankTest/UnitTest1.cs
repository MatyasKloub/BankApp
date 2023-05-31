using Bank.Core.Authorization;
using Bank.Core.Database;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.IO;

namespace BankTest;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }


    [Test]
    public void CreateUserTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;

        var user = new User("testName", "testPassword", "test@email.com");
        var result = DbAction.CreateUser(user, options);

        if (result)
        {
            Assert.True(result, "All g");
        }
        else
        {
            Assert.False(result, "Test failed, why?");
        }

    }

    [Test]
    public void CreateUserNevalidniTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;

        var user = new User("testName", "testPassword", "muginari@seznam.cz");
        var result = DbAction.CreateUser(user, options);

        Assert.IsFalse(result);
    }


    [Test]
    public void userExistsTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;

        var result = DbAction.userExists("muginari@seznam.cz", options);

        Assert.IsTrue(result);
    }

    [Test]
    public void userExistsNevalidniTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;

        var result = DbAction.userExists("muginari@ddddddseznam.cz", options);

        Assert.IsFalse(result);
    }

    [Test]
    public void userExistsAndRightNevalidniTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var user = new User("testName", "testPassword", "muginari@seznam.cz");
        var result = DbAction.UserExistsAndRight(user, options);

        Assert.IsFalse(result);
    }

    [Test]
    public void userExistsAndRightTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var user = new User("el testo", "f3ba18f5143ee1651cb91e9802b87ff3abfadcda0fd1d3e78c6cb22dc336883f", "muginari@seznam.cz");
        var result = DbAction.UserExistsAndRight(user, options);

        Assert.IsTrue(result);
    }


    [Test]
    public void returnUserDataTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var user = new User("testName", "testPassword", "muginari@seznam.cz");
        var result = DbAction.ReturnUserData(user, options);


        Assert.NotNull(result);
    }


    [Test]
    public void returnUserDataNevalidniTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var user = new User("testName", "testPassword", "mugindddsdsddddari@seznam.cz");
        var result = DbAction.ReturnUserData(user, options);


        Assert.IsNull(result);
    }



    [Test]
    public void returnUserDataObjNevalidniTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var user = new User("testName", "testPassword", "muginari@seznam.cz");
        var result = DbAction.ReturnUserDataObj("muginddddddddari@seznam.cz", options);
        Assert.IsNull(result);
    }


    [Test]
    public void returnUserDataObjTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var user = new User("testName", "testPassword", "muginari@seznam.cz");
        var result = DbAction.ReturnUserDataObj("muginari@seznam.cz", options);
        Assert.NotNull(result);
    }


    [Test]
    public void createUcetTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var user = new User("testName", "testPassword", "muginari@seznam.cz");
        var result = DbAction.CreateUcet(666, "CZK", "12345", options);


        Assert.NotNull(result);
    }

    [Test]
    public void getUctyTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var result = DbAction.GetUcty("muginari@seznam.cz", options);


        Assert.NotNull(result);
    }

    [Test]
    public void getUctyNevalidniTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var result = DbAction.GetUcty("mugidddddsdsdsdsdnari@seznam.cz", options);


        Assert.IsNull(result);
    }


    [Test]
    public void getUctyObjTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var result = DbAction.GetUctyObj("muginari@seznam.cz", options);


        Assert.NotNull(result);
    }

    [Test]
    public void getUctyObjNevalidniTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var result = DbAction.GetUctyObj("muginarsddsdsdsdsdsi@seznam.cz", options);


        Assert.IsNull(result);
    }

    [Test]
    public void getMenyTest()
    {
        var result = DbAction.GetMeny();

        Assert.NotNull(result);
    }

    [Test]
    public void getKurzTest()
    {
        var result = DbAction.GetKurz("HUF");
        Assert.NotNull(result);
    }
    [Test]
    public void getKurzCZKTest()
    {
        var result = DbAction.GetKurz("CZK");
        Assert.IsNull(result);
    }

    [Test]
    public void isPaymentPossibleTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var addMoney = DbAction.doPayment("CZK", 300, "maty.kloub@gmail.com", "prichozi", options);

        var result = DbAction.isPaymentPossible("EUR",10,"maty.kloub@gmail.com", options);

        Assert.IsTrue(result);
    }

    [Test]
    public void isPaymentPossibleInCZKTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var result = DbAction.isPaymentPossible("CZK", 1, "muginari@seznam.cz", options);

        Assert.IsTrue(result);
    }

    [Test]
    public void isPaymentPossibleCZKNotPossibleTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var result = DbAction.isPaymentPossible("CZK", 10000000, "muginari@seznam.cz", options);

        Assert.IsFalse(result);
    }

    [Test]
    public void isPaymentPossibleHUFNotPossibleTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var result = DbAction.isPaymentPossible("HUF", 10000000, "muginari@seznam.cz", options);

        Assert.IsFalse(result);
    }

    [Test]
    public void doPaymentPrichoziTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var result = DbAction.doPayment("USD", 1, "muginari@seznam.cz","prichozi", options);

        Assert.NotNull(result);
    }

    [Test]
    public void doPaymentOdchoziTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var addMoney = DbAction.doPayment("EUR", 30, "maty.kloub@gmail.com", "prichozi", options);
        var result = DbAction.doPayment("EUR", 10, "maty.kloub@gmail.com", "odchozi", options);

        Assert.IsTrue(result);
    }

    [Test]
    public void doPaymentOdchoziNevalidniCiziMenaTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var result = DbAction.doPayment("USD", 1, "muginaaaaaaaaaari@seznam.cz", "odchozi", options);

        Assert.IsFalse(result);
    }

    [Test]
    public void doPaymentOdchoziNevalidniCZKTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var result = DbAction.doPayment("CZK", 1, "muginaaaaaaaaaari@seznam.cz", "odchozi", options);

        Assert.IsFalse(result);
    }

    [Test]
    public void doPaymentOdchoziNedostatekFinanciTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var result = DbAction.doPayment("CZK", 100000000, "muginari@seznam.cz", "odchozi", options);

        Assert.IsFalse(result);
    }

    [Test]
    public void doPaymentKontokorent()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var result = DbAction.doPayment("CZK", 1040, "maty.kloub@gmail.com", "odchozi", options);

        Assert.IsTrue(result);
    }

    [Test]
    public void doPaymentKontokorentCiziMena()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var addMoney = DbAction.doPayment("EUR", 39, "maty.kloub@gmail.com", "prichozi", options);

        var result = DbAction.doPayment("EUR", 40, "maty.kloub@gmail.com", "odchozi", options);

        Assert.IsTrue(result);
    }

    [Test]
    public void doPaymentOdchoziNedostatekFinanciNeCZKTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var result = DbAction.doPayment("HUF", 100000000, "muginari@seznam.cz", "odchozi", options);

        Assert.IsFalse(result);
    }




    [Test]
    public void createPlatbaOdchoziTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var result = DbAction.createPlatba("muginari@seznam.cz", 1, "CZK", "odchozi", options);

        Assert.NotNull(result);
    }

    [Test]
    public void createPlatbaPrichoziTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var result = DbAction.createPlatba("muginari@seznam.cz", 1, "CZK", "prichozi", options);

        Assert.NotNull(result);
    }

    [Test]
    public void getPlatbyTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var result = DbAction.getPlatby("muginari@seznam.cz", options);

        Assert.IsNotEmpty(result);
    }

    [Test]
    public void getPlatbyIsInvalidTest()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite("Data Source=testDatabase.db").Options;
        var result = DbAction.getPlatby("AAADSDS", options);
        Assert.IsNull(result);
    }


}