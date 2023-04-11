if object_id('PR_Customer') is not null drop table PR_Customer
go
create table PR_Customer( 
	[Code] varchar(200) NOT NULL,
	[Name] nvarchar(2000) NULL,
	[Address] nvarchar(2000) NULL,		
	[TaxCode] nvarchar(2000) NULL, 
	[Phone] nvarchar(2000) NULL, 
	[Remark] nvarchar(2000) NULL, 
	[InputTime] datetime2 NULL,
	[ModifyTime] datetime2 NULL,
	[Token] nvarchar(2000) NULL,
 CONSTRAINT [PK_PR_Customer] PRIMARY KEY CLUSTERED 
(
	[Code]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go
select * from PR_Customer

