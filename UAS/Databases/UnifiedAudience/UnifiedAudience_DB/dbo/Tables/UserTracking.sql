CREATE TABLE [dbo].[UserTracking] (
    [UserTrackingID]   INT            IDENTITY (1, 1) NOT NULL,
    [UserID]           INT            NULL,
    [Activity]         VARCHAR (50)   NULL,
    [ActivityDateTime] DATETIME       NULL,
    [IPAddress]        VARCHAR (100)  NULL,
    [BrowserInfo]      VARCHAR (2048) NULL,
    [PlatformID]       INT            NULL,
    [ClientID]         INT            NULL
);



