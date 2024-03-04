# What is this?

A project seed for a C# dotnet API ("PaylocityBenefitsCalculator").  It is meant to get you started on the Paylocity BackEnd Coding Challenge by taking some initial setup decisions away.

The goal is to respect your time, avoid live coding, and get a sense for how you work.

# Coding Challenge

**Show us how you work.**

Each of our Paylocity product teams operates like a small startup, empowered to deliver business value in
whatever way they see fit. Because our teams are close knit and fast moving it is imperative that you are able
to work collaboratively with your fellow developers. 

This coding challenge is designed to allow you to demonstrate your abilities and discuss your approach to
design and implementation with your potential colleagues. You are free to use whatever technologies you
prefer but please be prepared to discuss the choices you’ve made. We encourage you to focus on creating a
logical and functional solution rather than one that is completely polished and ready for production.

The challenge can be used as a canvas to capture your strengths in addition to reflecting your overall coding
standards and approach. There’s no right or wrong answer.  It’s more about how you think through the
problem. We’re looking to see your skills in all three tiers so the solution can be used as a conversation piece
to show our teams your abilities across the board.

Requirements will be given separately.

# Implementation Details:

In this seed repo given for assessment we have implmeneted a functionality where use can perform the following activities.

1. Add new employees along with pay details like hourly salary rate, dependent details etc.
2. Have exposed an API which will accept the Year as input and create a Pay Period Schedule of 26 Paychecks in the given year.
3. Have exposed an API which can run the Payroll and generate data for Pay Slips. We need to Pass Period Start Date, Period End Date.

   Technical Details.

   Following are the architecture principles implemented.

   1. Single Responsibility Princple: All the classes have the code which is specific to that class objective.
   2. Dependency Injection:  All the classes are loosely couple and they interact with DI.
   3. Abstract Pattern: Payroll Calculation is performed using 2 Base Classes defined for Calculation & Deductions. This is designed keeping in mind of extensibility.
   4. SQL DB: We have used SQL DB here because the data we maintained are related with FK. We have not gone for No SQL like elastic search because these are ideal for large scale of data where reads are heavy.
   5. Logging: We have implmented logging with Serilog extension.
   6. Repository Pattern: I have implemented a repository pattern to keep all the DB related code here. I have used EF 6 as ORM.
