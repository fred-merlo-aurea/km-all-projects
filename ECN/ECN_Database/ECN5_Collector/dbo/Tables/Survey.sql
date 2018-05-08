CREATE TABLE [dbo].[Survey] (
    [SurveyID]      INT            IDENTITY (1, 1) NOT NULL,
    [SurveyTitle]   VARCHAR (50)   NULL,
    [Description]   VARCHAR (1000) NULL,
    [CustomerID]    INT            NULL,
    [GroupID]       INT            NULL,
    [EnableDate]    DATETIME       NULL,
    [DisableDate]   DATETIME       CONSTRAINT [DF_survey_TimeToLeave] DEFAULT ((60)) NULL,
    [IntroHTML]     TEXT           NULL,
    [ThankYouHTML]  TEXT           NULL,
    [IsActive]      BIT            CONSTRAINT [DF_survey_active] DEFAULT ((1)) NULL,
    [CompletedStep] INT            NULL,
    [CreatedUserID] INT            NULL,
    [CreatedDate]   DATETIME       NULL,
    [UpdatedUserID] INT            NULL,
    [UpdatedDate]   DATETIME       NULL,
    [IsDeleted]     BIT            CONSTRAINT [DF_survey_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_survey] PRIMARY KEY CLUSTERED ([SurveyID] ASC) WITH (FILLFACTOR = 80)
);

