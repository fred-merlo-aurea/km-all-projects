﻿CREATE TABLE [dbo].[PubSubscriptionsExtension] (
    [PubSubscriptionID] INT                   NOT NULL,
    [Field1]         VARCHAR (2048) SPARSE NULL,
    [Field2]         VARCHAR (2048) SPARSE NULL,
    [Field3]         VARCHAR (2048) SPARSE NULL,
    [Field4]         VARCHAR (2048) SPARSE NULL,
    [Field5]         VARCHAR (2048) SPARSE NULL,
    [Field6]         VARCHAR (2048) SPARSE NULL,
    [Field7]         VARCHAR (2048) SPARSE NULL,
    [Field8]         VARCHAR (2048) SPARSE NULL,
    [Field9]         VARCHAR (2048) SPARSE NULL,
    [Field10]        VARCHAR (2048) SPARSE NULL,
    [Field11]        VARCHAR (2048) SPARSE NULL,
    [Field12]        VARCHAR (2048) SPARSE NULL,
    [Field13]        VARCHAR (2048) SPARSE NULL,
    [Field14]        VARCHAR (2048) SPARSE NULL,
    [Field15]        VARCHAR (2048) SPARSE NULL,
    [Field16]        VARCHAR (2048) SPARSE NULL,
    [Field17]        VARCHAR (2048) SPARSE NULL,
    [Field18]        VARCHAR (2048) SPARSE NULL,
    [Field19]        VARCHAR (2048) SPARSE NULL,
    [Field20]        VARCHAR (2048) SPARSE NULL,
    [Field21]        VARCHAR (2048) SPARSE NULL,
    [Field22]        VARCHAR (2048) SPARSE NULL,
    [Field23]        VARCHAR (2048) SPARSE NULL,
    [Field24]        VARCHAR (2048) SPARSE NULL,
    [Field25]        VARCHAR (2048) SPARSE NULL,
    [Field26]        VARCHAR (2048) SPARSE NULL,
    [Field27]        VARCHAR (2048) SPARSE NULL,
    [Field28]        VARCHAR (2048) SPARSE NULL,
    [Field29]        VARCHAR (2048) SPARSE NULL,
    [Field30]        VARCHAR (2048) SPARSE NULL,
    [Field31]        VARCHAR (2048) SPARSE NULL,
    [Field32]        VARCHAR (2048) SPARSE NULL,
    [Field33]        VARCHAR (2048) SPARSE NULL,
    [Field34]        VARCHAR (2048) SPARSE NULL,
    [Field35]        VARCHAR (2048) SPARSE NULL,
    [Field36]        VARCHAR (2048) SPARSE NULL,
    [Field37]        VARCHAR (2048) SPARSE NULL,
    [Field38]        VARCHAR (2048) SPARSE NULL,
    [Field39]        VARCHAR (2048) SPARSE NULL,
    [Field40]        VARCHAR (2048) SPARSE NULL,
    [Field41]        VARCHAR (2048) SPARSE NULL,
    [Field42]        VARCHAR (2048) SPARSE NULL,
    [Field43]        VARCHAR (2048) SPARSE NULL,
    [Field44]        VARCHAR (2048) SPARSE NULL,
    [Field45]        VARCHAR (2048) SPARSE NULL,
    [Field46]        VARCHAR (2048) SPARSE NULL,
    [Field47]        VARCHAR (2048) SPARSE NULL,
    [Field48]        VARCHAR (2048) SPARSE NULL,
    [Field49]        VARCHAR (2048) SPARSE NULL,
    [Field50]        VARCHAR (2048) SPARSE NULL,
    [Field51]        VARCHAR (2048) SPARSE NULL,
    [Field52]        VARCHAR (2048) SPARSE NULL,
    [Field53]        VARCHAR (2048) SPARSE NULL,
    [Field54]        VARCHAR (2048) SPARSE NULL,
    [Field55]        VARCHAR (2048) SPARSE NULL,
    [Field56]        VARCHAR (2048) SPARSE NULL,
    [Field57]        VARCHAR (2048) SPARSE NULL,
    [Field58]        VARCHAR (2048) SPARSE NULL,
    [Field59]        VARCHAR (2048) SPARSE NULL,
    [Field60]        VARCHAR (2048) SPARSE NULL,
    [Field61]        VARCHAR (2048) SPARSE NULL,
    [Field62]        VARCHAR (2048) SPARSE NULL,
    [Field63]        VARCHAR (2048) SPARSE NULL,
    [Field64]        VARCHAR (2048) SPARSE NULL,
    [Field65]        VARCHAR (2048) SPARSE NULL,
    [Field66]        VARCHAR (2048) SPARSE NULL,
    [Field67]        VARCHAR (2048) SPARSE NULL,
    [Field68]        VARCHAR (2048) SPARSE NULL,
    [Field69]        VARCHAR (2048) SPARSE NULL,
    [Field70]        VARCHAR (2048) SPARSE NULL,
    [Field71]        VARCHAR (2048) SPARSE NULL,
    [Field72]        VARCHAR (2048) SPARSE NULL,
    [Field73]        VARCHAR (2048) SPARSE NULL,
    [Field74]        VARCHAR (2048) SPARSE NULL,
    [Field75]        VARCHAR (2048) SPARSE NULL,
    [Field76]        VARCHAR (2048) SPARSE NULL,
    [Field77]        VARCHAR (2048) SPARSE NULL,
    [Field78]        VARCHAR (2048) SPARSE NULL,
    [Field79]        VARCHAR (2048) SPARSE NULL,
    [Field80]        VARCHAR (2048) SPARSE NULL,
    [Field81]        VARCHAR (2048) SPARSE NULL,
    [Field82]        VARCHAR (2048) SPARSE NULL,
    [Field83]        VARCHAR (2048) SPARSE NULL,
    [Field84]        VARCHAR (2048) SPARSE NULL,
    [Field85]        VARCHAR (2048) SPARSE NULL,
    [Field86]        VARCHAR (2048) SPARSE NULL,
    [Field87]        VARCHAR (2048) SPARSE NULL,
    [Field88]        VARCHAR (2048) SPARSE NULL,
    [Field89]        VARCHAR (2048) SPARSE NULL,
    [Field90]        VARCHAR (2048) SPARSE NULL,
    [Field91]        VARCHAR (2048) SPARSE NULL,
    [Field92]        VARCHAR (2048) SPARSE NULL,
    [Field93]        VARCHAR (2048) SPARSE NULL,
    [Field94]        VARCHAR (2048) SPARSE NULL,
    [Field95]        VARCHAR (2048) SPARSE NULL,
    [Field96]        VARCHAR (2048) SPARSE NULL,
    [Field97]        VARCHAR (2048) SPARSE NULL,
    [Field98]        VARCHAR (2048) SPARSE NULL,
    [Field99]        VARCHAR (2048) SPARSE NULL,
    [Field100]       VARCHAR (2048) SPARSE NULL,
	[DateCreated] DATETIME NULL, 
    [DateUpdated] DATETIME NULL, 
    [CreatedByUserID] INT NULL, 
    [UpdatedByUserID] INT NULL, 
    CONSTRAINT [PK_PubSubscriptionsExtension] PRIMARY KEY CLUSTERED ([PubSubscriptionID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_PubSubscriptionsExtension_PubSubscriptions] FOREIGN KEY ([PubSubscriptionID]) REFERENCES [dbo].[PubSubscriptions] ([PubSubscriptionID])
);
