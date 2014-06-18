﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgeting.BLL
{
    public class Core
    {
        public double getGrossIncome(List<Income> _listofIncome)
        {
            double grossIncome = 0;
            foreach (Income income in _listofIncome)
            {
                grossIncome += income.Amount;
            }
            return grossIncome;
        }

        public double getGrossIncomePerYear(List<Income> _listofIncome, int noOfPayPerYear)
        {
            if (_listofIncome == null)
                throw new NullReferenceException();
            if (!_listofIncome.Any())
                throw new ArgumentException();
            if (noOfPayPerYear <= 0)
                throw new ArgumentException("No of pays per year cannot be negative.");
            return getGrossIncome(_listofIncome) * noOfPayPerYear;
        }

        public double getNetIncomePerYear(float _taxRate, float _superannuationRate, List<Income> _listofIncome, int noOfPayPerYear)
        {
            return getGrossIncomePerYear(_listofIncome, noOfPayPerYear) *  (1 - _taxRate + _superannuationRate);
        }

        public double getTotalExpenditure(List<Expenditure> _listOfExpenditure)
        {
            double totalExpenditure = 0;
            foreach (Expenditure expenditure in _listOfExpenditure)
            {
                totalExpenditure += expenditure.Amount;
            }
            return totalExpenditure;
        }

        public double getTotalExpenditurePerYear(List<Expenditure> _listOfExpenditure, int noOfPayPerYear)
        {
            if (noOfPayPerYear <= 0)
                throw new ArgumentException("No of pays per year less or equal to zero.");

            return getTotalExpenditure(_listOfExpenditure) * noOfPayPerYear;
        }

        public double getTotalExpenditurePerYear(List<Expenditure> _listOfExpenditure)
        {
            return getTotalExpenditure(_listOfExpenditure) * 12;
        }

        public double getAmountAvailableForGoalsPerYear(float _taxRate, float _superannuationRate, List<Expenditure> _listOfExpenditure, List<Income> _listofIncome, int noOfPayPerYear)
        {
            return (getNetIncomePerYear(_taxRate, _superannuationRate, _listofIncome, noOfPayPerYear) - getTotalExpenditurePerYear(_listOfExpenditure));
        }

        public int getNoOfPaysRequiredToAccomplishGoal(double  goalCost, double amountPerPay)
        {
            if (amountPerPay == 0)
                throw new DivideByZeroException();
            if(amountPerPay <0|| goalCost<0)
                throw new ArgumentOutOfRangeException();
            return (int)Math.Ceiling(goalCost / amountPerPay);
        }

        public double getMinimumAmountRequiredPerPayToAccomplishGoalBeforeDeadline(double goalCost, int desiredNoOfPaysForGoalAccomplishment)
        {
            return (goalCost/desiredNoOfPaysForGoalAccomplishment);
        }

        public Boolean goalPayableBeforeDeadline(double goalCost, double amountPerPay, int desiredNoOfPaysForGoalAccomplishment)
        {
            if (desiredNoOfPaysForGoalAccomplishment <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            int noOfPaysRequired = getNoOfPaysRequiredToAccomplishGoal(goalCost, amountPerPay);
            if (noOfPaysRequired <= desiredNoOfPaysForGoalAccomplishment)
                return true;
            else
                return false;
        }

        public double getAmountAvailableForGoalsPerPay(float _taxRate, float _superannuationRate, List<Expenditure> _listOfExpenditure, List<Income> _listofIncome, int noOfPayPerYear)
        {
            if (noOfPayPerYear == 0)
                throw new DivideByZeroException();
            if (noOfPayPerYear < 0)
                throw new ArgumentOutOfRangeException();
            else
                return (getAmountAvailableForGoalsPerYear(_taxRate, _superannuationRate, _listOfExpenditure, _listofIncome, noOfPayPerYear) / noOfPayPerYear);
        }

        public double getRemainingAmountPerPay(double amountPerPay, float _taxRate, float _superannuationRate, List<Expenditure> _listOfExpenditure, List<Income> _listofIncome, int noOfPayPerYear)
        {
            return (getAmountAvailableForGoalsPerPay(_taxRate, _superannuationRate, _listOfExpenditure, _listofIncome, noOfPayPerYear) - amountPerPay);
        }

        public Boolean increment(MainGoal mg,double currentDeposit)
        {
            mg.AmountSaved += currentDeposit;
            if (mg.AmountSaved >= mg.Cost)
            {
                //saving completed for main goal
                return false;
            }
            return true;
        }

        public double calculatePendingAmount(Goal g)
        {
            return g.Cost - g.AmountSaved;
        }

        public Boolean Tick(WalletTableItem wli)
        {
            wli.AmountSaved += wli.ContributionPerTick;
            wli.NoOfTicks++;
            if (wli.AmountSaved >= wli.Cost)
            {
                //Wallet table item saving completed
                return false;
            }
            return true;
        }

    }
}
