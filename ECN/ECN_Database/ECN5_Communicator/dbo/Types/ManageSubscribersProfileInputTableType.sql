﻿	CREATE TYPE [dbo].[ManageSubscribersProfileInputTableType] AS TABLE
(
	SubscriberInputTableRowID uniqueidentifier PRIMARY KEY,

	EmailAddress varchar(255),
	GroupID int,
	FormatTypeCode varchar(16),
	SubscribeTypeCode varchar(32),
	
	Title varchar(50) NULL,
	FirstName varchar(50) NULL,
	LastName varchar(50) NULL,
	FullName varchar(50) NULL,
	Company varchar(100) NULL,
	Occupation varchar(50) NULL,
	Address varchar(255) NULL,
	Address2 varchar(255) NULL,
	City varchar(50) NULL,
	State varchar(50) NULL,
	Zip varchar(50) NULL,
	Country varchar(50) NULL,
	Voice varchar(50) NULL,
	Mobile varchar(50) NULL,
	Fax varchar(50) NULL,
	Website varchar(50) NULL,
	Age varchar(50) NULL,
	Income varchar(50) NULL,
	Gender varchar(50) NULL,
	User1 varchar(255) NULL,
	User2 varchar(255) NULL,
	User3 varchar(255) NULL,
	User4 varchar(255) NULL,
	User5 varchar(255) NULL,
	User6 varchar(255) NULL,
	Birthdate datetime NULL,
	UserEvent1 varchar(50) NULL,
	UserEvent1Date datetime NULL,
	UserEvent2 varchar(50) NULL,
	UserEvent2Date datetime NULL,
	Notes varchar(1000) NULL,
	Password varchar(25) NULL
)
