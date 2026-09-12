/* =====================================================================
   Recruitment Portal - Database schema (SQL Server / MSSQL)
   DB-first approach: run this script to create the database & tables.
   The .NET backend connects with Microsoft.Data.SqlClient + Dapper.
   ===================================================================== */

IF DB_ID('RecruitmentPortal') IS NULL
BEGIN
    CREATE DATABASE RecruitmentPortal;
END
GO

USE RecruitmentPortal;
GO

/* ---------------------------------------------------------------------
   Users - authentication (login / registration)
   Password is stored as a salted hash (never plain text).
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        Id            INT IDENTITY(1,1)  NOT NULL PRIMARY KEY,
        FullName      NVARCHAR(150)      NOT NULL,
        Email         NVARCHAR(256)      NOT NULL,
        PasswordHash  NVARCHAR(512)      NOT NULL,
        Role          NVARCHAR(50)       NOT NULL CONSTRAINT DF_Users_Role DEFAULT('Candidate'),
        CreatedAt     DATETIME2(0)       NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT(SYSUTCDATETIME())
    );

    CREATE UNIQUE INDEX UX_Users_Email ON dbo.Users(Email);
END
GO

/* ---------------------------------------------------------------------
   Candidates - the profile/application form a candidate fills up
   Linked back to the Users table (who created / owns the record).
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.Candidates', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Candidates
    (
        Id                INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        UserId            INT               NULL,
        FirstName         NVARCHAR(100)     NOT NULL,
        LastName          NVARCHAR(100)     NOT NULL,
        Email             NVARCHAR(256)     NOT NULL,
        Phone             NVARCHAR(30)      NULL,
        DateOfBirth       DATE              NULL,
        Gender            NVARCHAR(20)      NULL,
        Address           NVARCHAR(500)     NULL,
        City              NVARCHAR(100)     NULL,
        State             NVARCHAR(100)     NULL,
        Country           NVARCHAR(100)     NULL,
        PostalCode        NVARCHAR(20)      NULL,
        PositionApplied   NVARCHAR(150)     NULL,
        TotalExperience   DECIMAL(4,1)      NULL,   -- years of experience
        CurrentCompany    NVARCHAR(150)     NULL,
        CurrentCtc        DECIMAL(12,2)     NULL,
        ExpectedCtc       DECIMAL(12,2)     NULL,
        NoticePeriodDays  INT               NULL,
        HighestQualification NVARCHAR(150)  NULL,
        Skills            NVARCHAR(1000)    NULL,   -- comma separated skills
        LinkedInUrl       NVARCHAR(300)     NULL,
        ResumeUrl         NVARCHAR(300)     NULL,
        CoverLetter       NVARCHAR(MAX)     NULL,
        Status            NVARCHAR(50)      NOT NULL CONSTRAINT DF_Candidates_Status DEFAULT('Applied'),
        CreatedAt         DATETIME2(0)      NOT NULL CONSTRAINT DF_Candidates_CreatedAt DEFAULT(SYSUTCDATETIME()),
        UpdatedAt         DATETIME2(0)      NULL,
        CONSTRAINT FK_Candidates_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id)
    );

    CREATE INDEX IX_Candidates_UserId ON dbo.Candidates(UserId);
    CREATE INDEX IX_Candidates_Email  ON dbo.Candidates(Email);
END
GO

PRINT 'RecruitmentPortal schema is ready.';
GO
