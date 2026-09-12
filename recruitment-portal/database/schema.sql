/* =====================================================================
   Recruitment Portal - Database schema (SQL Server / MSSQL)
   DB-first approach: run this script to create the database & tables.
   The .NET backend connects with Microsoft.Data.SqlClient + Dapper.

   Safe to re-run: tables are created only if missing, and new columns
   are added via guarded ALTER statements for already-existing databases.
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

        -- Personal details
        FirstName         NVARCHAR(100)     NOT NULL,
        MiddleName        NVARCHAR(100)     NULL,
        LastName          NVARCHAR(100)     NOT NULL,
        Email             NVARCHAR(256)     NOT NULL,
        Phone             NVARCHAR(30)      NULL,
        AlternatePhone    NVARCHAR(30)      NULL,
        DateOfBirth       DATE              NULL,
        Gender            NVARCHAR(20)      NULL,
        MaritalStatus     NVARCHAR(20)      NULL,
        Nationality       NVARCHAR(100)     NULL,

        -- Address
        Address           NVARCHAR(500)     NULL,
        City              NVARCHAR(100)     NULL,
        State             NVARCHAR(100)     NULL,
        Country           NVARCHAR(100)     NULL,
        PostalCode        NVARCHAR(20)      NULL,

        -- Professional details
        PositionApplied   NVARCHAR(150)     NULL,
        EmploymentType    NVARCHAR(50)      NULL,
        TotalExperience   DECIMAL(4,1)      NULL,   -- years of experience
        CurrentCompany    NVARCHAR(150)     NULL,
        CurrentCtc        DECIMAL(12,2)     NULL,
        ExpectedCtc       DECIMAL(12,2)     NULL,
        NoticePeriodDays  INT               NULL,
        PreferredLocation NVARCHAR(150)     NULL,
        WillingToRelocate BIT               NULL,
        AvailableFrom     DATE              NULL,
        HighestQualification NVARCHAR(150)  NULL,
        Skills            NVARCHAR(1000)    NULL,   -- comma separated skills

        -- Links & documents
        LinkedInUrl       NVARCHAR(300)     NULL,
        PortfolioUrl      NVARCHAR(300)     NULL,
        GitHubUrl         NVARCHAR(300)     NULL,
        ResumeUrl         NVARCHAR(300)     NULL,
        CoverLetter       NVARCHAR(MAX)     NULL,

        -- References & meta
        ReferenceName     NVARCHAR(150)     NULL,
        ReferenceContact  NVARCHAR(150)     NULL,
        Source            NVARCHAR(50)      NULL,   -- how did you hear about us
        Status            NVARCHAR(50)      NOT NULL CONSTRAINT DF_Candidates_Status DEFAULT('Applied'),
        CreatedAt         DATETIME2(0)      NOT NULL CONSTRAINT DF_Candidates_CreatedAt DEFAULT(SYSUTCDATETIME()),
        UpdatedAt         DATETIME2(0)      NULL,
        CONSTRAINT FK_Candidates_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id)
    );

    CREATE INDEX IX_Candidates_UserId ON dbo.Candidates(UserId);
    CREATE INDEX IX_Candidates_Email  ON dbo.Candidates(Email);
END
GO

/* ---------------------------------------------------------------------
   Add newer columns to an existing dbo.Candidates table (idempotent).
   Lets you upgrade a database created by an earlier version of this script.
   --------------------------------------------------------------------- */
IF COL_LENGTH('dbo.Candidates', 'MiddleName')        IS NULL ALTER TABLE dbo.Candidates ADD MiddleName        NVARCHAR(100) NULL;
IF COL_LENGTH('dbo.Candidates', 'AlternatePhone')    IS NULL ALTER TABLE dbo.Candidates ADD AlternatePhone    NVARCHAR(30)  NULL;
IF COL_LENGTH('dbo.Candidates', 'MaritalStatus')     IS NULL ALTER TABLE dbo.Candidates ADD MaritalStatus     NVARCHAR(20)  NULL;
IF COL_LENGTH('dbo.Candidates', 'Nationality')       IS NULL ALTER TABLE dbo.Candidates ADD Nationality       NVARCHAR(100) NULL;
IF COL_LENGTH('dbo.Candidates', 'EmploymentType')    IS NULL ALTER TABLE dbo.Candidates ADD EmploymentType    NVARCHAR(50)  NULL;
IF COL_LENGTH('dbo.Candidates', 'PreferredLocation') IS NULL ALTER TABLE dbo.Candidates ADD PreferredLocation NVARCHAR(150) NULL;
IF COL_LENGTH('dbo.Candidates', 'WillingToRelocate') IS NULL ALTER TABLE dbo.Candidates ADD WillingToRelocate BIT           NULL;
IF COL_LENGTH('dbo.Candidates', 'AvailableFrom')     IS NULL ALTER TABLE dbo.Candidates ADD AvailableFrom     DATE          NULL;
IF COL_LENGTH('dbo.Candidates', 'PortfolioUrl')      IS NULL ALTER TABLE dbo.Candidates ADD PortfolioUrl      NVARCHAR(300) NULL;
IF COL_LENGTH('dbo.Candidates', 'GitHubUrl')         IS NULL ALTER TABLE dbo.Candidates ADD GitHubUrl         NVARCHAR(300) NULL;
IF COL_LENGTH('dbo.Candidates', 'ReferenceName')     IS NULL ALTER TABLE dbo.Candidates ADD ReferenceName     NVARCHAR(150) NULL;
IF COL_LENGTH('dbo.Candidates', 'ReferenceContact')  IS NULL ALTER TABLE dbo.Candidates ADD ReferenceContact  NVARCHAR(150) NULL;
IF COL_LENGTH('dbo.Candidates', 'Source')            IS NULL ALTER TABLE dbo.Candidates ADD Source            NVARCHAR(50)  NULL;
GO

PRINT 'RecruitmentPortal schema is ready.';
GO
