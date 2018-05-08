﻿CREATE TABLE [dbo].[MAF_MEDTECH_Enews] (
    [Client_ID]                INT              NOT NULL,
    [ClientName]               VARCHAR (7)      NOT NULL,
    [EmailID]                  INT              NOT NULL,
    [EmailAddress]             VARCHAR (255)    NOT NULL,
    [Title]                    VARCHAR (50)     NULL,
    [FirstName]                VARCHAR (50)     NULL,
    [LastName]                 VARCHAR (50)     NULL,
    [FullName]                 VARCHAR (50)     NULL,
    [Company]                  VARCHAR (100)    NULL,
    [Occupation]               VARCHAR (50)     NULL,
    [Address]                  VARCHAR (255)    NULL,
    [Address2]                 VARCHAR (255)    NULL,
    [City]                     VARCHAR (50)     NULL,
    [State]                    VARCHAR (50)     NULL,
    [Zip]                      VARCHAR (50)     NULL,
    [Country]                  VARCHAR (50)     NULL,
    [Voice]                    VARCHAR (50)     NULL,
    [Mobile]                   VARCHAR (50)     NULL,
    [Fax]                      VARCHAR (50)     NULL,
    [Website]                  VARCHAR (50)     NULL,
    [Age]                      VARCHAR (50)     NULL,
    [Income]                   VARCHAR (50)     NULL,
    [Gender]                   VARCHAR (50)     NULL,
    [User1]                    VARCHAR (255)    NULL,
    [User2]                    VARCHAR (255)    NULL,
    [User3]                    VARCHAR (255)    NULL,
    [User4]                    VARCHAR (255)    NULL,
    [User5]                    VARCHAR (255)    NULL,
    [User6]                    VARCHAR (255)    NULL,
    [Birthdate]                DATETIME         NULL,
    [UserEvent1]               VARCHAR (50)     NULL,
    [UserEvent1Date]           DATETIME         NULL,
    [UserEvent2]               VARCHAR (50)     NULL,
    [UserEvent2Date]           DATETIME         NULL,
    [Notes]                    VARCHAR (30)     NULL,
    [CreatedOn]                DATETIME         NULL,
    [LastChanged]              DATETIME         NULL,
    [FormatTypeCode]           VARCHAR (5)      NOT NULL,
    [SubscribeTypeCode]        VARCHAR (50)     NULL,
    [GroupId]                  INT              NOT NULL,
    [SUDF_GID]                 INT              NULL,
    [tmp_EmailID]              INT              NULL,
    [BATCH]                    VARCHAR (500)    NULL,
    [BUDGET_SPECIFY_RECOMMEND] VARCHAR (500)    NULL,
    [BUSINESS]                 VARCHAR (500)    NULL,
    [Business1]                VARCHAR (500)    NULL,
    [Business1TXT]             VARCHAR (500)    NULL,
    [Business2]                VARCHAR (500)    NULL,
    [BUSINESS3]                VARCHAR (500)    NULL,
    [Business3TXT]             VARCHAR (500)    NULL,
    [Business4]                VARCHAR (500)    NULL,
    [Business4TXT]             VARCHAR (500)    NULL,
    [Business5]                VARCHAR (500)    NULL,
    [Business5TXT]             VARCHAR (500)    NULL,
    [Business6]                VARCHAR (500)    NULL,
    [Business6TXT]             VARCHAR (500)    NULL,
    [BUSINESS7]                VARCHAR (500)    NULL,
    [BUSINESS8]                VARCHAR (500)    NULL,
    [BUSINESS9]                VARCHAR (500)    NULL,
    [BusinessDesc]             VARCHAR (500)    NULL,
    [BusinessDescription]      VARCHAR (500)    NULL,
    [BusinessTEXT]             VARCHAR (500)    NULL,
    [CAT]                      VARCHAR (500)    NULL,
    [CategoryID]               VARCHAR (500)    NULL,
    [CGRP_NO]                  VARCHAR (500)    NULL,
    [Code]                     VARCHAR (500)    NULL,
    [Comp]                     VARCHAR (500)    NULL,
    [COUNTY]                   VARCHAR (500)    NULL,
    [DEMO13]                   VARCHAR (500)    NULL,
    [DEMO14]                   VARCHAR (500)    NULL,
    [Demo15]                   VARCHAR (500)    NULL,
    [DEMO16]                   VARCHAR (500)    NULL,
    [Demo31]                   VARCHAR (500)    NULL,
    [Demo32]                   VARCHAR (500)    NULL,
    [Demo33]                   VARCHAR (500)    NULL,
    [Demo34]                   VARCHAR (500)    NULL,
    [Demo35]                   VARCHAR (500)    NULL,
    [Demo36]                   VARCHAR (500)    NULL,
    [Demo7]                    VARCHAR (500)    NULL,
    [Demo8]                    VARCHAR (500)    NULL,
    [EmailsClicked]            VARCHAR (500)    NULL,
    [EmailsOpened]             VARCHAR (500)    NULL,
    [EmailsReceived]           VARCHAR (500)    NULL,
    [EMPLOY]                   VARCHAR (500)    NULL,
    [EMPLOYEE_SIZE]            VARCHAR (500)    NULL,
    [Facebook]                 VARCHAR (500)    NULL,
    [FileKey]                  VARCHAR (500)    NULL,
    [ForZip]                   VARCHAR (500)    NULL,
    [FUNCTION]                 VARCHAR (500)    NULL,
    [Function1]                VARCHAR (500)    NULL,
    [FUNCTION1TXT]             VARCHAR (500)    NULL,
    [Function2]                VARCHAR (500)    NULL,
    [Function2TXT]             VARCHAR (500)    NULL,
    [Function3]                VARCHAR (500)    NULL,
    [Function3TXT]             VARCHAR (500)    NULL,
    [Function4]                VARCHAR (500)    NULL,
    [Function4TXT]             VARCHAR (500)    NULL,
    [Function5]                VARCHAR (500)    NULL,
    [Function5TXT]             VARCHAR (500)    NULL,
    [Function6TXT]             VARCHAR (500)    NULL,
    [FunctionDesc]             VARCHAR (500)    NULL,
    [FunctionDescription]      VARCHAR (500)    NULL,
    [FunctionText]             VARCHAR (500)    NULL,
    [FunctionTXT]              VARCHAR (500)    NULL,
    [HISTBATCH]                VARCHAR (500)    NULL,
    [IGRP_NO]                  VARCHAR (500)    NULL,
    [LastOpenDate]             VARCHAR (500)    NULL,
    [Linkedin]                 VARCHAR (500)    NULL,
    [ListCode]                 VARCHAR (500)    NULL,
    [ListOrigin]               VARCHAR (500)    NULL,
    [LIVES_COVERED]            VARCHAR (500)    NULL,
    [MAILSTOP]                 VARCHAR (500)    NULL,
    [MBR_FLG]                  VARCHAR (500)    NULL,
    [NONAME]                   VARCHAR (500)    NULL,
    [NONAME3]                  VARCHAR (500)    NULL,
    [NUMBER_OF_BEDS]           VARCHAR (500)    NULL,
    [NUMBER_OF_PHYSICIANS]     VARCHAR (500)    NULL,
    [PA1EMAIL]                 VARCHAR (500)    NULL,
    [PA1FNAME]                 VARCHAR (500)    NULL,
    [PA1FUNCTION]              VARCHAR (500)    NULL,
    [PA1FUNCTXT]               VARCHAR (500)    NULL,
    [PA1LFUNCTION]             VARCHAR (500)    NULL,
    [PA1LFUNCTXT]              VARCHAR (500)    NULL,
    [PA1LNAME]                 VARCHAR (500)    NULL,
    [PA2EMAIL]                 VARCHAR (500)    NULL,
    [PA2FNAME]                 VARCHAR (500)    NULL,
    [PA2FUNCTION]              VARCHAR (500)    NULL,
    [PA2FUNCTXT]               VARCHAR (500)    NULL,
    [PA2LNAME]                 VARCHAR (500)    NULL,
    [PAR3C]                    VARCHAR (500)    NULL,
    [PLUS4]                    VARCHAR (500)    NULL,
    [Professional_Role]        VARCHAR (500)    NULL,
    [pswd]                     VARCHAR (500)    NULL,
    [pubids]                   VARCHAR (500)    NULL,
    [QDATE]                    VARCHAR (500)    NULL,
    [QSOURCE]                  VARCHAR (500)    NULL,
    [Specialty]                VARCHAR (500)    NULL,
    [Specialty_]               VARCHAR (500)    NULL,
    [SUBSCRIBERID]             VARCHAR (500)    NULL,
    [SUBSCRIPTION]             VARCHAR (500)    NULL,
    [SUBSRC]                   VARCHAR (500)    NULL,
    [Supplier]                 VARCHAR (500)    NULL,
    [Telemarketing_ID]         VARCHAR (500)    NULL,
    [TOPICSENEW]               VARCHAR (500)    NULL,
    [TransactionDate]          VARCHAR (500)    NULL,
    [TransactionID]            VARCHAR (500)    NULL,
    [TRANSACTIONTYPE]          VARCHAR (500)    NULL,
    [Twitter]                  VARCHAR (500)    NULL,
    [VERIFY]                   VARCHAR (500)    NULL,
    [Worksite]                 VARCHAR (500)    NULL,
    [XACT]                     VARCHAR (500)    NULL,
    [TUDF_GID]                 INT              NULL,
    [tmp_EmailID1]             INT              NULL,
    [entryID]                  UNIQUEIDENTIFIER NULL,
    [DEMO39]                   VARCHAR (500)    NULL,
    [PubAssoc]                 VARCHAR (500)    NULL,
    [PubCode]                  VARCHAR (500)    NULL,
    [PublicationCode]          VARCHAR (500)    NULL,
    [Topics]                   VARCHAR (500)    NULL,
    [webinars]                 VARCHAR (500)    NULL,
    [whitepapers]              VARCHAR (500)    NULL
);

