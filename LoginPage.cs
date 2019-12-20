using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.UI;

// Still needs to be data-driven - to ask in class
namespace Exercise4
{
    [TestClass]
    public class LoginPage
    {

        public LoginPage(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.Name, Using = "userName")]
        public IWebElement UserName { get; set; }

        [FindsBy(How = How.Name, Using = "password")]
        public IWebElement Password { get; set; }

        [FindsBy(How = How.Name, Using = "login")]
        public IWebElement loginButton { get; set; }

        [TestClass]
        public class LoginTest
        {

            [TestMethod]
            public void Test1()
            {
                IWebDriver driver = new FirefoxDriver();
                LoginPage page = new LoginPage(driver);

                driver.Navigate().GoToUrl("http://www.newtours.demoaut.com/");
                driver.Manage().Window.Maximize();

                // Screen 1: Home page/login
                String strUserName = "Vaishali";
                String strPassword = "mercury";

                page.UserName.SendKeys(strUserName);
                page.Password.SendKeys(strPassword);
                page.loginButton.Click();

                // TEST - looking for "CONTINUE" button on page that follows log in
                Assert.IsTrue(driver.FindElement(By.Name("findFlights")).Displayed);


                // Screen 2: Choosing options (city, class)
                SelectElement city = new SelectElement(driver.FindElement(By.Name("fromPort")));
                
                String departingCity = "Frankfurt";
                city.SelectByValue(departingCity);

                IWebElement flightClass = driver.FindElement(By.XPath("//input[@value='Business']"));
                flightClass.Click();

                IWebElement continueButton1 = driver.FindElement(By.Name("findFlights"));
                continueButton1.Click();

                // TEST - looking for "CONTINUE" button on page that follows choosing options
                Assert.IsTrue(driver.FindElement(By.Name("reserveFlights")).Displayed);

                // Screen 3: Selecting the flight itself
                IWebElement departingFlight = driver.FindElement(By.XPath("//input[@value='Blue Skies Airlines$361$271$7:10']"));
                departingFlight.Click();

                IWebElement returnFlight = driver.FindElement(By.XPath("//input[@value='Blue Skies Airlines$631$273$14:30']"));
                returnFlight.Click();

                IWebElement continueButton2 = driver.FindElement(By.Name("reserveFlights"));
                continueButton2.Click();

                // TEST - looking for "SECURE PURCHASE" button on page that follows selecting flight
                Assert.IsTrue(driver.FindElement(By.Name("buyFlights")).Displayed);

                // Screen 4: Purchasing the flight
                string finalPrice = driver.FindElement(By.XPath("//tr/td/font[@size='2']/b")).Text;
                //Console.WriteLine(finalPrice);
                //Assert.IsTrue(finalPrice == "$588");

                IWebElement ticketless = driver.FindElement(By.Name("ticketLess"));
                ticketless.Click();

                IWebElement purchaseButton = driver.FindElement(By.Name("buyFlights"));
                purchaseButton.Click();

                // TEST - looking for "Your itinerary has been booked!" message on page that follows purchase
                Console.WriteLine(driver.FindElement(By.XPath("//font[@size='+1']")).Text);
                Assert.IsTrue(driver.FindElement(By.XPath("//font[@size='+1']")).Text == "Your itinerary has been booked!");

                // TEST - comparing price from previous screen with current screen
                // note: take out the " USD" at the end of new one for a proper comparison
                string finalPrice2 = driver.FindElement(By.XPath("//font/b/font/font/b/font[@size='2']")).Text;
                finalPrice2 = finalPrice2.Substring(0, finalPrice2.Length - 4);
                Console.WriteLine(finalPrice2);
                Assert.IsTrue(finalPrice == finalPrice2);
            }
        }
    }
}