Create table AdminTable(
AdminId int primary key Identity(1,1),
AdminName varchar(200),
AdminEmail varchar(100),
AdminPassword varchar(100));

select * from AdminTable

Insert Into AdminTable(AdminName,AdminEmail,AdminPassword) Values('James','James@gmail.com','James@123');

--stored procedure for AdminLogin
Create Procedure spAdminLogin
@Email varchar(100),
@Password varchar(100)
AS
Begin
	Begin Try
		Begin Transaction
		If Exists(Select * From AdminTable Where AdminEmail=@Email AND AdminPassword=@Password)
			Begin
				Select AdminId From AdminTable Where AdminEmail=@Email AND AdminPassword=@Password;
				--Print 'Admin Logged In';
			End
		Else
			Begin
				Print 'Invalid Input';
			End
		Commit Transaction
	End Try
	Begin Catch
		Rollback Transaction;
	End Catch
End;

Exec spAdminLogin 'admin','admin';
select * from AdminTable;
--Insert into AdminTable (AdminName,AdminEmail,AdminPassword)Values('admin','admin','admin')
select * from DoctorTable where DoctorEmail='abc@gmail.com';