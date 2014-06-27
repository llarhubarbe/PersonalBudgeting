﻿using NUnit.Framework;
using PersonalBudgeting.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgeting.Tests
{
    [TestFixture]

    class RealLifeScenario4
    {
        Core core;
        Budget myBudget;
        double _amountForMainGoalPerPay;

        [TestFixtureSetUp]
        public void TestSetuptheEnvironment()
        {
            core = new Core();
            myBudget = new Budget();
           
        }
        [TestFixtureTearDown]
        public void TestTearDownTheEnvironment()
        {
            core = null;
            myBudget = null;
        }

        [Test]
        public void TestWholeYearScenario_MainGoalSuccessfullyMet_ReturnFalse()
        {
            myBudget.mainGoal.Name = "House Loan";
            myBudget.mainGoal.Description = "MCB";
            myBudget.mainGoal.Cost = 900000;
            myBudget.mainGoal.DurationInNoOfPays = 10;
            myBudget.mainGoal.AmountSaved = 0.0;

            _amountForMainGoalPerPay = core.getMinimumAmountRequiredPerPayToAccomplishGoalBeforeDeadline(myBudget.mainGoal.Cost, myBudget.mainGoal.DurationInNoOfPays);


           Assert.AreEqual(90000, _amountForMainGoalPerPay);
           Assert.AreEqual(10, myBudget.mainGoal.DurationInNoOfPays);
          
            core.updateBankAccount(myBudget.SavingsAccount,
                                     myBudget.TaxRate,
                                     myBudget.SuperannuationRate,
                                     myBudget.ListOfExpenditure,
                                     myBudget.ListOfIncome,
                                     myBudget.NoOfPaysPerYear,
                                     myBudget.mainGoal,
                                     _amountForMainGoalPerPay,
                                     myBudget.ListOfWalletTableItem
                                    );
            Assert.AreEqual(4325,myBudget.SavingsAccount.SavingsForPersonalUse,0.1);
            Assert.AreEqual(35,myBudget.SavingsAccount.SavingsForGoals);
            Assert.AreEqual(false, core.goalPayableBeforeDeadline(myBudget.mainGoal.Cost, _amountForMainGoalPerPay, myBudget.mainGoal.DurationInNoOfPays));
        }

       
    }
}