DROP TABLE ProductDescriptions
DROP TABLE ProductReviews
--DROP TABLE ProductPriceLists
DROP TABLE Products
--DROP TABLE Currencies
--DROP TABLE Countries
--DROP TABLE PriceUnits
DROP TABLE Languages
DROP TABLE ProductCategories
DROP TABLE Manufacturers

CREATE TABLE Manufacturers
(
	Id int identity not null primary key,
	Manufacturer nvarchar(50) not null unique
)

CREATE TABLE ProductCategories
(
	Id int identity not null primary key,
	Category nvarchar(50) not null unique,
	ParentCategory int null,
)

CREATE TABLE Languages
(
	Id int identity not null primary key,
	LanguageType varchar(20) not null unique
)

--CREATE TABLE PriceUnits
--(
--	Id int identity not null primary key,
--	Unit varchar(10) not null unique
--)
--
--CREATE TABLE Countries
--(
--	Id int identity not null primary key,
--	Country varchar(20) not null unique,
--)
--
--CREATE TABLE Currencies
--(
--	Id int identity not null primary key,
--	Currency char(3) not null unique,
--)

CREATE TABLE Products
(
	ArticleNumber varchar(10) not null primary key,
	Title nvarchar(50) not null,
	ManufacturerId int not null references Manufacturers(Id),
	ProductCategoryId int not null references ProductCategories(Id),
	Price money not null,
	DiscountPrice money null,
)

--CREATE TABLE ProductPriceList
--(
--	Id int identity not null,
--	Price money not null,
--	DiscountPrice money null,
--	PriceUnitId int not null references PriceUnits(Id),
--	CurrencyId int not null references Currencies(Id),
--	CountryId int not null references Countries(Id),
--	ArticleNumber varchar(10) not null references Products(ArticleNumber),
--
--	primary key (Id, ArticleNumber)
--)

CREATE TABLE ProductReviews
(
	Id int identity not null primary key,
	Review nvarchar(max) null,
	Rating tinyint not null,
	Author nvarchar(20) null,
	ArticleNumber varchar(10) not null references Products(ArticleNumber)
)

CREATE TABLE ProductDescriptions
(
	Id int identity not null,
	Description nvarchar(max) not null,
	Specifications nvarchar(max) not null,
	Ingress nvarchar(200) null,
	ArticleNumber varchar(10) not null references Products(ArticleNumber),
	LanguageId int not null references Languages(Id),

	primary key (ArticleNumber, LanguageId)
)
