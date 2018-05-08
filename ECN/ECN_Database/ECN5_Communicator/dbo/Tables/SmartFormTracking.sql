CREATE TABLE [dbo].[SmartFormTracking] (
    [SFTID]        INT      IDENTITY (1, 1) NOT NULL,
	[BlastID] INT NULL,
    [CustomerID]   INT      NULL,
    [SmartFormID]  INT      NULL,
	[GroupID] int Null,
	[ReferringURL] varchar(max) not null,
    [ActivityDate] DATETIME not NULL
   
);

