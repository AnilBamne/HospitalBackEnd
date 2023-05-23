--sored procedure for reset password
Alter Procedure spDocResetPassword
@Email varchar(200),
@ConfirmPassword varchar(200)
AS
Begin
	Begin Try
		Begin Transaction
			If Exists(Select * from DoctorTable where DoctorEmail=@Email)
				Begin
					Update DoctorTable Set DoctorPassword=@ConfirmPassword where DoctorEmail=@Email;
				End
			Else
				Begin
					print 'Email not found';
				End
		Commit Transaction
	End Try
	Begin Catch
		Rollback Transaction
	End Catch
End;

select * from DoctorTable
exec spDocResetPassword 'abc@gmail.com','abccc';
------------------------------------------------------------------
