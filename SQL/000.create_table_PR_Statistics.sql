if object_id('PR_Statistics') is not null drop table PR_Statistics
go
create table PR_Statistics(
	[MainID] bigint identity(1,1) NOT NULL,
	[ChargeCharaters] int NULL,
	[Customer] varchar(200) NULL,
	[RequestTime] datetime2 NULL,
	[IpAddress] varchar(200) NULL,
	[UserAgent] nvarchar(2000) NULL,
	[Country] nvarchar(2000) NULL,		
	[Region] nvarchar(2000) NULL,
	[City] nvarchar(2000) NULL,
	[ISP] nvarchar(2000) NULL,
	[Remark] nvarchar(2000) NULL,
 CONSTRAINT [PK_PR_Statistics] PRIMARY KEY CLUSTERED 
(
	[MainID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go
select * from PR_Statistics

