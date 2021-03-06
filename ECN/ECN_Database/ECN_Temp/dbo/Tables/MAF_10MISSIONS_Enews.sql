﻿CREATE TABLE [dbo].[MAF_10MISSIONS_Enews] (
    [Client_ID]         INT              NOT NULL,
    [ClientName]        VARCHAR (10)     NOT NULL,
    [EmailID]           INT              NOT NULL,
    [EmailAddress]      VARCHAR (255)    NOT NULL,
    [Title]             VARCHAR (50)     NULL,
    [FirstName]         VARCHAR (50)     NULL,
    [LastName]          VARCHAR (50)     NULL,
    [FullName]          VARCHAR (50)     NULL,
    [Company]           VARCHAR (100)    NULL,
    [Occupation]        VARCHAR (50)     NULL,
    [Address]           VARCHAR (255)    NULL,
    [Address2]          VARCHAR (255)    NULL,
    [City]              VARCHAR (50)     NULL,
    [State]             VARCHAR (50)     NULL,
    [Zip]               VARCHAR (50)     NULL,
    [Country]           VARCHAR (50)     NULL,
    [Voice]             VARCHAR (50)     NULL,
    [Mobile]            VARCHAR (50)     NULL,
    [Fax]               VARCHAR (50)     NULL,
    [Website]           VARCHAR (50)     NULL,
    [Age]               VARCHAR (50)     NULL,
    [Income]            VARCHAR (50)     NULL,
    [Gender]            VARCHAR (50)     NULL,
    [User1]             VARCHAR (255)    NULL,
    [User2]             VARCHAR (255)    NULL,
    [User3]             VARCHAR (255)    NULL,
    [User4]             VARCHAR (255)    NULL,
    [User5]             VARCHAR (255)    NULL,
    [User6]             VARCHAR (255)    NULL,
    [Birthdate]         DATETIME         NULL,
    [UserEvent1]        VARCHAR (50)     NULL,
    [UserEvent1Date]    DATETIME         NULL,
    [UserEvent2]        VARCHAR (50)     NULL,
    [UserEvent2Date]    DATETIME         NULL,
    [Notes]             VARCHAR (30)     NULL,
    [CreatedOn]         DATETIME         NULL,
    [LastChanged]       DATETIME         NULL,
    [FormatTypeCode]    VARCHAR (5)      NOT NULL,
    [SubscribeTypeCode] VARCHAR (50)     NULL,
    [GroupId]           INT              NOT NULL,
    [SUDF_GID]          INT              NULL,
    [tmp_EmailID]       INT              NULL,
    [2177_1]            VARCHAR (500)    NULL,
    [2177_10]           VARCHAR (500)    NULL,
    [2177_11]           VARCHAR (500)    NULL,
    [2177_12]           VARCHAR (500)    NULL,
    [2177_13]           VARCHAR (500)    NULL,
    [2177_14]           VARCHAR (500)    NULL,
    [2177_15]           VARCHAR (500)    NULL,
    [2177_16]           VARCHAR (500)    NULL,
    [2177_17]           VARCHAR (500)    NULL,
    [2177_18]           VARCHAR (500)    NULL,
    [2177_19]           VARCHAR (500)    NULL,
    [2177_2]            VARCHAR (500)    NULL,
    [2177_20]           VARCHAR (500)    NULL,
    [2177_21]           VARCHAR (500)    NULL,
    [2177_22]           VARCHAR (500)    NULL,
    [2177_23]           VARCHAR (500)    NULL,
    [2177_3]            VARCHAR (500)    NULL,
    [2177_4]            VARCHAR (500)    NULL,
    [2177_5]            VARCHAR (500)    NULL,
    [2177_6]            VARCHAR (500)    NULL,
    [2177_7]            VARCHAR (500)    NULL,
    [2177_8]            VARCHAR (500)    NULL,
    [2177_8_TEXT]       VARCHAR (500)    NULL,
    [2177_9]            VARCHAR (500)    NULL,
    [2177_blastID]      VARCHAR (500)    NULL,
    [2177_completionDt] VARCHAR (500)    NULL,
    [2241_1]            VARCHAR (500)    NULL,
    [2241_10]           VARCHAR (500)    NULL,
    [2241_11]           VARCHAR (500)    NULL,
    [2241_12]           VARCHAR (500)    NULL,
    [2241_13]           VARCHAR (500)    NULL,
    [2241_14]           VARCHAR (500)    NULL,
    [2241_15]           VARCHAR (500)    NULL,
    [2241_16]           VARCHAR (500)    NULL,
    [2241_17]           VARCHAR (500)    NULL,
    [2241_17_TEXT]      VARCHAR (500)    NULL,
    [2241_18]           VARCHAR (500)    NULL,
    [2241_18_TEXT]      VARCHAR (500)    NULL,
    [2241_19]           VARCHAR (500)    NULL,
    [2241_19_TEXT]      VARCHAR (500)    NULL,
    [2241_2]            VARCHAR (500)    NULL,
    [2241_20]           VARCHAR (500)    NULL,
    [2241_21]           VARCHAR (500)    NULL,
    [2241_21_TEXT]      VARCHAR (500)    NULL,
    [2241_22]           VARCHAR (500)    NULL,
    [2241_22_TEXT]      VARCHAR (500)    NULL,
    [2241_23]           VARCHAR (500)    NULL,
    [2241_24]           VARCHAR (500)    NULL,
    [2241_25]           VARCHAR (500)    NULL,
    [2241_25_TEXT]      VARCHAR (500)    NULL,
    [2241_26]           VARCHAR (500)    NULL,
    [2241_27]           VARCHAR (500)    NULL,
    [2241_28]           VARCHAR (500)    NULL,
    [2241_28_TEXT]      VARCHAR (500)    NULL,
    [2241_29]           VARCHAR (500)    NULL,
    [2241_29_TEXT]      VARCHAR (500)    NULL,
    [2241_3]            VARCHAR (500)    NULL,
    [2241_30]           VARCHAR (500)    NULL,
    [2241_31]           VARCHAR (500)    NULL,
    [2241_32]           VARCHAR (500)    NULL,
    [2241_33]           VARCHAR (500)    NULL,
    [2241_34]           VARCHAR (500)    NULL,
    [2241_35]           VARCHAR (500)    NULL,
    [2241_36]           VARCHAR (500)    NULL,
    [2241_37]           VARCHAR (500)    NULL,
    [2241_4]            VARCHAR (500)    NULL,
    [2241_5]            VARCHAR (500)    NULL,
    [2241_5_TEXT]       VARCHAR (500)    NULL,
    [2241_6]            VARCHAR (500)    NULL,
    [2241_7]            VARCHAR (500)    NULL,
    [2241_8]            VARCHAR (500)    NULL,
    [2241_9]            VARCHAR (500)    NULL,
    [2241_blastID]      VARCHAR (500)    NULL,
    [2241_completionDt] VARCHAR (500)    NULL,
    [2312_1]            VARCHAR (500)    NULL,
    [2312_2]            VARCHAR (500)    NULL,
    [2312_24]           VARCHAR (500)    NULL,
    [2312_25]           VARCHAR (500)    NULL,
    [2312_26]           VARCHAR (500)    NULL,
    [2312_27]           VARCHAR (500)    NULL,
    [2312_28]           VARCHAR (500)    NULL,
    [2312_3]            VARCHAR (500)    NULL,
    [2312_4]            VARCHAR (500)    NULL,
    [2312_5]            VARCHAR (500)    NULL,
    [2312_6]            VARCHAR (500)    NULL,
    [2369_16]           VARCHAR (500)    NULL,
    [2369_17]           VARCHAR (500)    NULL,
    [2369_19]           VARCHAR (500)    NULL,
    [2369_20]           VARCHAR (500)    NULL,
    [2369_21]           VARCHAR (500)    NULL,
    [2369_22]           VARCHAR (500)    NULL,
    [2369_23]           VARCHAR (500)    NULL,
    [2369_24]           VARCHAR (500)    NULL,
    [2369_25]           VARCHAR (500)    NULL,
    [2369_26]           VARCHAR (500)    NULL,
    [2369_27]           VARCHAR (500)    NULL,
    [2369_28]           VARCHAR (500)    NULL,
    [2369_5]            VARCHAR (500)    NULL,
    [2371_16]           VARCHAR (500)    NULL,
    [2371_17]           VARCHAR (500)    NULL,
    [2371_18]           VARCHAR (500)    NULL,
    [2371_19]           VARCHAR (500)    NULL,
    [2371_20]           VARCHAR (500)    NULL,
    [2371_21]           VARCHAR (500)    NULL,
    [2371_22]           VARCHAR (500)    NULL,
    [2371_23]           VARCHAR (500)    NULL,
    [2371_24]           VARCHAR (500)    NULL,
    [2371_25]           VARCHAR (500)    NULL,
    [2371_26]           VARCHAR (500)    NULL,
    [2371_27]           VARCHAR (500)    NULL,
    [2371_28]           VARCHAR (500)    NULL,
    [2371_5]            VARCHAR (500)    NULL,
    [BATCH]             VARCHAR (500)    NULL,
    [BUSINESS]          VARCHAR (500)    NULL,
    [BUSINESS1]         VARCHAR (500)    NULL,
    [BUSINESSTEXT]      VARCHAR (500)    NULL,
    [CAT]               VARCHAR (500)    NULL,
    [Date_Subscribed]   VARCHAR (500)    NULL,
    [Date_Unsubscribed] VARCHAR (500)    NULL,
    [DEMO1]             VARCHAR (500)    NULL,
    [DEMO10]            VARCHAR (500)    NULL,
    [DEMO11]            VARCHAR (500)    NULL,
    [DEMO12]            VARCHAR (500)    NULL,
    [DEMO13]            VARCHAR (500)    NULL,
    [DEMO14]            VARCHAR (500)    NULL,
    [DEMO15]            VARCHAR (500)    NULL,
    [DEMO16]            VARCHAR (500)    NULL,
    [DEMO2]             VARCHAR (500)    NULL,
    [DEMO3]             VARCHAR (500)    NULL,
    [DEMO31]            VARCHAR (500)    NULL,
    [DEMO32]            VARCHAR (500)    NULL,
    [DEMO33]            VARCHAR (500)    NULL,
    [DEMO34]            VARCHAR (500)    NULL,
    [DEMO35]            VARCHAR (500)    NULL,
    [DEMO36]            VARCHAR (500)    NULL,
    [DEMO39]            VARCHAR (500)    NULL,
    [DEMO4]             VARCHAR (500)    NULL,
    [DEMO5]             VARCHAR (500)    NULL,
    [DEMO6]             VARCHAR (500)    NULL,
    [DEMO7]             VARCHAR (500)    NULL,
    [DEMO8]             VARCHAR (500)    NULL,
    [DEMO9]             VARCHAR (500)    NULL,
    [EMPLOY]            VARCHAR (500)    NULL,
    [FORZIP]            VARCHAR (500)    NULL,
    [FUNCTION]          VARCHAR (500)    NULL,
    [FUNCTION1]         VARCHAR (500)    NULL,
    [FUNCTIONTEXT]      VARCHAR (500)    NULL,
    [HISTBATCH]         VARCHAR (500)    NULL,
    [MAILSTOP]          VARCHAR (500)    NULL,
    [PAR3C]             VARCHAR (500)    NULL,
    [PASSALONG]         VARCHAR (500)    NULL,
    [PLUS4]             VARCHAR (500)    NULL,
    [promoCode]         VARCHAR (500)    NULL,
    [PUBCODE]           VARCHAR (500)    NULL,
    [PUBLICATIONCODE]   VARCHAR (500)    NULL,
    [QDATE]             VARCHAR (500)    NULL,
    [QSOURCE]           VARCHAR (500)    NULL,
    [Reason]            VARCHAR (500)    NULL,
    [Status]            VARCHAR (500)    NULL,
    [SUBSCRIBERID]      VARCHAR (500)    NULL,
    [SUBSCRIPTION]      VARCHAR (500)    NULL,
    [SUBSRC]            VARCHAR (500)    NULL,
    [TRANSACTIONTYPE]   VARCHAR (500)    NULL,
    [VERIFY]            VARCHAR (500)    NULL,
    [XACT]              VARCHAR (500)    NULL,
    [TUDF_GID]          INT              NULL,
    [tmp_EmailID1]      INT              NULL,
    [entryID]           UNIQUEIDENTIFIER NULL,
    [Topic]             VARCHAR (500)    NULL,
    [Topics]            VARCHAR (500)    NULL
);



