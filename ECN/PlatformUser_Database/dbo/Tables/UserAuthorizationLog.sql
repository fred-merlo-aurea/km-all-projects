CREATE TABLE [dbo].[UserAuthorizationLog]
(
		UserAuthLogID int IDENTITY (1, 1) not null Primary Key,
        AuthSource varchar(50) not null,
        AuthMode varchar(50) not null,
        AuthAttemptDate date not null,
        AuthAttemptTime time(7) not null,
        IsAuthenticated bit not null default('false'),
        IpAddress varchar(15) null,
        AuthUserName varchar(50) null,
        AuthAccessKey uniqueidentifier null,
        ServerVariables varchar(max),
		AppVersion varchar(50) null,
        UserID int null,
        LogOutDate date null,
        LogOutTime time(7) null
)
