create database ProductDatabase;
go
use ProductDatabase;
go
-- create category table
create table Category(
	Id int identity(1,1) primary key,
	CategoryName varchar(50)
)
go
-- create product table
create table Product(
	Id int identity(1,1) primary key,
	Name varchar(50),
	Category int,
	Price decimal,
	StockQuantity int,
	LastUpdateDate datetime,
	CONSTRAINT FK_Category FOREIGN KEY (Category)
        REFERENCES category(Id)
        ON DELETE no action
        ON UPDATE no action	
)
go
-- insert procedure for category table
create procedure InsertCategoty
@name varchar(50),
@errMsg varchar(100) output
as
begin
	begin try
	declare @vCount int;
	set @errMsg = '';
	set @vCount = 0;
	set @vCount = (select count(*) from category t where t.CategoryName = @name);
	
	if @vCount > 0 
	begin
	   set @errMsg = 'Category Name Already Exits';
	   return;
	end
		insert into [category] ([CategoryName]) values (@name);
	end	try
	begin catch
		set @errMsg = 'Error Happend while inserting';
	end catch
end;
go
-- get all the list of category
create procedure CategoryList
as
begin
   select * from category;
end;
go
-- delete category by id
create procedure DeleteCategory
@id int,
@errMsg varchar(100) output
as
begin
	begin try
		delete from category where Id = @id;
	end try
	begin catch
		set @errMsg = 'Error Happend While Deleting The Category';
	end catch
end;
go 
create procedure deleteProduct
@id int,
@errMsg varchar(100) output
as
begin
	declare @productName varchar(50);
	set @productName = '';
	begin try
		set @productName =  (select t.name from Product t where t.Id = @id);
		delete from Product where Id = @id;
	end try
	begin catch
		set @errMsg = 'Error happend while deleteing product ('+@productName+')';
	end catch
end;
go
create procedure getProductList
as
begin
	select t.Id, t.Name,t.Category,t.Price,t.StockQuantity,t.LastUpdateDate,c.CategoryName
	from Product t,category c
	where t.Category = c.Id;
end;
go
create procedure InsertUpdateProduct
@name varchar(50),
@category int,
@price decimal,
@stockquentity int,
@lastUpdate date,
@id int output,
@errMsg varchar(100) output
as
begin
declare @vCount int;
		set @errMsg = '';
		set @vCount = 0;
		set @vCount = (select count(*) from Product t where t.Name = @name);
if @id = 0
begin
	  begin try		
		if @vCount > 0 
		begin
			set @errMsg = 'Product Name Already Exits';
			return;
		end

		if @price <= 0 
		begin
		   set @errMsg = 'Price is requried for product '+@name;
		   return;
		end

		if @stockquentity < 0 
		begin
		   set @errMsg = 'Product stock cannot be negetive';
		   return;
		end

		INSERT INTO [Product]
				   ([Name]
				   ,[Category]
				   ,[Price]
				   ,[StockQuantity]
				   ,[LastUpdateDate])
			 VALUES
				   (@name,@category,@price,@stockquentity,@lastUpdate);
		SET @id = SCOPE_IDENTITY();
	  end try

	  begin catch
	    set @errMsg = 'Error Happend While Inserting Data';
	  end catch
end
else
begin
  begin try
		
		if @vCount > 1 
		begin
		   set @errMsg = 'Product name can not be update to already exits product';
		   return;
		end
		UPDATE [Product]
		   SET [Name] = @name
			  ,[Category] = @category
			  ,[Price] = @price
			  ,[StockQuantity] = @stockquentity
			  ,[LastUpdateDate] = @lastUpdate
		 WHERE [Id] = @id;
	  end try
	  begin catch
	    set @errMsg = 'Error Happend While Updateing Data';
	  end catch
end
end;